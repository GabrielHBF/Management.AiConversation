using System.Text.Json.Serialization;

namespace Management.IAConversations.Application.OpenAI.Models.Agent.Message;

public class AgentSessionContent
{
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	[JsonPropertyName("text")]
	public string Text { get; set; } = string.Empty;
}