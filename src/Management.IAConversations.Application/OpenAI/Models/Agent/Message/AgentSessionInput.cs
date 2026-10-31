using System.Text.Json.Serialization;

namespace Management.IAConversations.Application.OpenAI.Models.Agent.Message;

public class AgentSessionInput
{
	[JsonPropertyName("role")]
	public string Role { get; set; } = string.Empty;

	[JsonPropertyName("content")]
	public IEnumerable<AgentSessionContent> Content { get; set; } = [];
}