using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response;

public sealed record OpenAiJsonSchemaFormat(
	[property: JsonPropertyName("type")]
	string Type,

	[property: JsonPropertyName("name")]
	string Name,

	[property: JsonPropertyName("strict")]
	bool Strict,

	[property: JsonPropertyName("schema")]
	OpenAiJsonSchema Schema
);
