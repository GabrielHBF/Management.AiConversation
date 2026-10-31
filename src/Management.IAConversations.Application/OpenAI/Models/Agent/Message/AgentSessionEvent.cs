using System.Text.Json.Serialization;

namespace Management.IAConversations.Application.OpenAI.Models.Agent.Message;

public class AgentSessionEvent
{
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	[JsonPropertyName("input")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public IEnumerable<AgentSessionInput>? Input { get; set; }

	[JsonPropertyName("turn_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? TurnId { get; set; }

	[JsonPropertyName("call_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? CallId { get; set; }

	[JsonPropertyName("success")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public bool? Success { get; set; }

	[JsonPropertyName("output")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Output { get; set; }
}