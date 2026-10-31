using Management.IAConversations.Application.Gateways;
using Management.IAConversations.Application.Constants;
using System.Net.Http.Json;
using OpenAiDocumentConstants = Management.IAConversations.Application.Constants.IaConversationsConstants.OpenAiDocumentValidation;

namespace Management.IAConversations.Infrastructure.Integrations.OpenAI.Documents;

public sealed class OpenAiDocumentValidator(IHttpClientFactory httpClientFactory) : IOpenAiDocumentValidator
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient(IaConversationsConstants.ChatGptClient);

	public async Task<string> ValidateAsync(string fileName, string contentType, byte[] fileContent, CancellationToken cancellationToken)
	{
		var inputError = ValidateInput(fileName, contentType, fileContent);
		if (inputError is not null)
		{
			return inputError;
		}

		var normalizedContentType = NormalizeContentType(contentType);

		var contentTypeError = EnsureSupportedContentType(normalizedContentType);
		if (contentTypeError is not null)
		{
			return contentTypeError;
		}

		var safeFileName = Path.GetFileName(
			fileName.Replace('\\', '/'));

		var payload = OpenAiDocumentRequestFactory.Create(
			safeFileName,
			normalizedContentType,
			fileContent);

		using var response = await _httpClient.PostAsJsonAsync(
			OpenAiDocumentConstants.Endpoint,
			payload,
			cancellationToken);

		var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			return CreateApiErrorMessage(
				response.StatusCode,
				responseBody);
		}

		return OpenAiDocumentResponseParser.Parse(responseBody);
	}

	private static string? ValidateInput(string fileName, string contentType, byte[] fileContent)
	{
		if (fileContent is null)
		{
			return "O conteúdo do arquivo é obrigatório.";
		}

		if (string.IsNullOrWhiteSpace(fileName))
		{
			return "O nome do arquivo é obrigatório.";
		}

		if (string.IsNullOrWhiteSpace(contentType))
		{
			return "O tipo de conteúdo é obrigatório.";
		}

		if (fileContent.Length == 0)
		{
			return "O arquivo está vazio.";
		}

		if (fileContent.Length > OpenAiDocumentConstants.MaxFileSizeBytes)
		{
			return "O arquivo excede o limite permitido de 10 MB.";
		}

		return null;
	}

	private static string NormalizeContentType(string contentType)
	{
		return contentType
			.Split(';', 2)[0]
			.Trim()
			.ToLowerInvariant();
	}

	private static string? EnsureSupportedContentType(
		string contentType)
	{
		if (contentType is not (
			OpenAiDocumentConstants.PdfContentType or
			OpenAiDocumentConstants.JpegContentType or
			OpenAiDocumentConstants.PngContentType or
			OpenAiDocumentConstants.WebpContentType))
		{
			return "Formato não suportado. Envie um PDF, JPG, PNG ou WebP.";
		}

		return null;
	}

	private static string CreateApiErrorMessage(
		System.Net.HttpStatusCode statusCode,
		string responseBody)
	{
		var details = responseBody
			.Replace('\r', ' ')
			.Replace('\n', ' ')
			.Trim();

		if (details.Length > OpenAiDocumentConstants.MaxErrorDetailsLength)
		{
			details = details[..OpenAiDocumentConstants.MaxErrorDetailsLength] + "...";
		}

		return
			$"Erro ao validar documento na OpenAI. " +
			$"HTTP {(int) statusCode} ({statusCode}). " +
			$"Detalhes: {details}";
	}
}
