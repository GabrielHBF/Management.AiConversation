using Management.IAConversations.Application.Response;
using System.Text.Json;

namespace Management.IAConversations.Infrastructure.Integrations.OpenAI.Documents;

internal static class OpenAiDocumentResponseParser
{
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNameCaseInsensitive = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	internal static string Parse(string responseBody)
	{
		using var json = JsonDocument.Parse(responseBody);
		var root = json.RootElement;

		if (root.ValueKind != JsonValueKind.Object)
		{
			return "A resposta da OpenAI não possui o formato esperado.";
		}

		var responseError = EnsureCompletedResponse(root);
		if (responseError is not null)
		{
			return responseError;
		}

		if (!TryExtractOutputText(root, out var outputText, out var outputError))
		{
			return outputError;
		}

		using var resultJson = JsonDocument.Parse(outputText);
		var resultElement = resultJson.RootElement;

		if (resultElement.ValueKind != JsonValueKind.Object ||
			!resultElement.TryGetProperty("isValid", out var isValidElement) ||
			isValidElement.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
		{
			return "A resposta da validação não contém um campo isValid booleano.";
		}

		var result = JsonSerializer.Deserialize<DocumentValidationResponse>(
			outputText,
			JsonOptions);

		return result is null
			? "Não foi possível interpretar o resultado da validação."
			: JsonSerializer.Serialize(result, JsonOptions);
	}

	private static string? EnsureCompletedResponse(JsonElement root)
	{
		if (!root.TryGetProperty("status", out var statusElement) ||
			statusElement.ValueKind != JsonValueKind.String)
		{
			return "A resposta da OpenAI não contém o status da análise.";
		}

		var status = statusElement.GetString();

		if (!string.Equals(status, "completed", StringComparison.OrdinalIgnoreCase))
		{
			return $"A análise do documento não foi concluída. Status: {status}.";
		}

		return null;
	}

	private static bool TryExtractOutputText(
		JsonElement root,
		out string outputText,
		out string errorMessage)
	{
		if (!root.TryGetProperty("output", out var outputItems) ||
			outputItems.ValueKind != JsonValueKind.Array)
		{
			outputText = string.Empty;
			errorMessage = "A resposta da OpenAI não contém o campo output esperado.";
			return false;
		}

		foreach (var item in outputItems.EnumerateArray())
		{
			if (item.ValueKind != JsonValueKind.Object ||
				!item.TryGetProperty("content", out var contents) ||
				contents.ValueKind != JsonValueKind.Array)
			{
				continue;
			}

			foreach (var content in contents.EnumerateArray())
			{
				if (content.ValueKind != JsonValueKind.Object ||
					!content.TryGetProperty("type", out var typeElement) ||
					typeElement.ValueKind != JsonValueKind.String)
				{
					continue;
				}

				var type = typeElement.GetString();

				if (type == "refusal")
				{
					outputText = string.Empty;
					errorMessage = "A OpenAI recusou a análise do documento.";
					return false;
				}

				if (type == "output_text" &&
					content.TryGetProperty("text", out var textElement) &&
					textElement.ValueKind == JsonValueKind.String)
				{
					var textValue = textElement.GetString();

					if (!string.IsNullOrWhiteSpace(textValue))
					{
						outputText = textValue;
						errorMessage = string.Empty;
						return true;
					}
				}
			}
		}

		outputText = string.Empty;
		errorMessage = "A OpenAI não retornou um resultado textual válido.";
		return false;
	}
}
