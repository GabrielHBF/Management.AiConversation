using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response;

public sealed record OpenAiInputMessage(
	[property: JsonPropertyName("role")]
	string Role,

	[property: JsonPropertyName("content")]
	IReadOnlyList<OpenAiInputContent> Content
);
