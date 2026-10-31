using System.Text.Json.Serialization;

namespace Management.IAConversations.Application.OpenAI.Models.Session;

public class SessionPayload
{
	[JsonPropertyName("agent_id")]
	public required string AgentId { get; set; }
	[JsonPropertyName("environment")]
	public required EnvironmentPayload Environment { get; set; }
}