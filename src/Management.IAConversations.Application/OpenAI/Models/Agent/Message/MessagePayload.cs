using System.Text.Json.Serialization;

namespace Management.IAConversations.Application.OpenAI.Models.Agent.Message;

public class MessagePayload
{
	[JsonPropertyName("events")]
	public IEnumerable<AgentSessionEvent> Events { get; set; } = [];
}