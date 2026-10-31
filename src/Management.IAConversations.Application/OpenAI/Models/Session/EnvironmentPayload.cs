using System.Text.Json.Serialization;

namespace Management.IAConversations.Application.OpenAI.Models.Session;

public class EnvironmentPayload
{
	[JsonPropertyName("type")]
	public required string Type { get; set; }
}