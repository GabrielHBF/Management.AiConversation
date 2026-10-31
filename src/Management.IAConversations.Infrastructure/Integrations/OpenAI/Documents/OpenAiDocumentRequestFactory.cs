using Management.IAConversations.Infrastructure.OpenAI.Models.Response;
using Management.IAConversations.Infrastructure.OpenAI.Models.Response.Types;
using OpenAiDocumentConstants = Management.IAConversations.Application.Constants.IaConversationsConstants.OpenAiDocumentValidation;

namespace Management.IAConversations.Infrastructure.Integrations.OpenAI.Documents;

internal static class OpenAiDocumentRequestFactory
{
	private const string ValidationPromptResourceName = "Management.IAConversations.Infrastructure.Integrations.OpenAI.Documents.Prompts.DocumentValidationPrompt.md";

	private static readonly string ValidationPrompt = LoadValidationPrompt();

	private static readonly OpenAiJsonSchema ResponseSchema = new(
		Type: "object",
		Properties: new Dictionary<string, OpenAiJsonSchemaProperty>
		{
			["isValid"] = new(Type: "boolean")
		},
		Required: ["isValid"],
		AdditionalProperties: false);

	internal static OpenAiResponsesRequest Create(
		string fileName,
		string contentType,
		byte[] fileContent)
	{
		var base64 = Convert.ToBase64String(fileContent);
		var dataUrl = $"data:{contentType};base64,{base64}";

		OpenAiInputContent documentInput = contentType switch
		{
			OpenAiDocumentConstants.PdfContentType => new OpenAiInputFile(fileName, dataUrl),
			OpenAiDocumentConstants.JpegContentType or
			OpenAiDocumentConstants.PngContentType or
			OpenAiDocumentConstants.WebpContentType => new OpenAiInputImage(dataUrl),
			_ => throw new NotSupportedException("Formato de arquivo não suportado.")
		};

		var inputMessage = new OpenAiInputMessage(
			Role: "user",
			Content:
			[
				documentInput,
				new OpenAiInputText(ValidationPrompt)
			]);

		return new OpenAiResponsesRequest(
			Model: OpenAiDocumentConstants.Model,
			Store: false,
			MaxOutputTokens: OpenAiDocumentConstants.MaxOutputTokens,
			Input: [inputMessage],
			Text: new OpenAiResponseTextOptions(
				Format: new OpenAiJsonSchemaFormat(
					Type: "json_schema",
					Name: "document_validation",
					Strict: true,
					Schema: ResponseSchema)));
	}

	private static string LoadValidationPrompt()
	{
		var assembly = typeof(OpenAiDocumentRequestFactory).Assembly;
		using var stream = assembly.GetManifestResourceStream(ValidationPromptResourceName)
			?? throw new InvalidOperationException(
				$"O recurso do prompt de validação não foi encontrado: {ValidationPromptResourceName}.");
		using var reader = new StreamReader(stream);

		return reader.ReadToEnd();
	}
}
