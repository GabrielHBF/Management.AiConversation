namespace Management.IAConversations.Application.Constants;

public static class IaConversationsConstants
{
	public const string ChatGptClient = "Management.OpenAI.Api";

	public static class OpenAiDocumentValidation
	{
		public const string Endpoint = "responses";
		public const string Model = "gpt-4.1-mini";
		public const int MaxFileSizeBytes = 10 * 1024 * 1024;
		public const int MaxOutputTokens = 250;
		public const int MaxErrorDetailsLength = 500;

		public const string PdfContentType = "application/pdf";
		public const string JpegContentType = "image/jpeg";
		public const string PngContentType = "image/png";
		public const string WebpContentType = "image/webp";
	}

	public const string DataPrefix = "data:";
	public const string OutputTextDone = "agent.session.turn.output_text.done";
	public const string RequiredAction = "agent.session.requires_action";
	public const string TurnFailed = "agent.session.turn.failed";
	public const string TurnCancelled = "agent.session.turn.cancelled";
	public const string SessionFailed = "agent.session.failed";
	public const string EnvironmentFailed = "agent.session.environment.failed";
	public const string Error = "error";
}