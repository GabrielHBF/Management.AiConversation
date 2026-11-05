namespace Management.IAConversations.CrossCutting.Models;

public class OllamaSettings
{
	public string Endpoint { get; set; } = string.Empty;
	public string TextModel { get; set; } = string.Empty;
	public string EmbeddingModel { get; set; } = string.Empty;
}