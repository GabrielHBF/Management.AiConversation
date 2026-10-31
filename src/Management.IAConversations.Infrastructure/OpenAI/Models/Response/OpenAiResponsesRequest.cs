using Management.IAConversations.Infrastructure.OpenAI.Models.Response.Types;
using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response;

public sealed record OpenAiResponsesRequest(
	[property: JsonPropertyName("model")]
	string Model,

	[property: JsonPropertyName("store")]
	bool Store,

	[property: JsonPropertyName("max_output_tokens")]
	int MaxOutputTokens,

	[property: JsonPropertyName("input")]
	IReadOnlyList<OpenAiInputMessage> Input,

	[property: JsonPropertyName("text")]
	OpenAiResponseTextOptions Text
);
