using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response;

public sealed record OpenAiJsonSchema(
	[property: JsonPropertyName("type")]
	string Type,

	[property: JsonPropertyName("properties")]
	IReadOnlyDictionary<string, OpenAiJsonSchemaProperty> Properties,

	[property: JsonPropertyName("required")]
	IReadOnlyList<string> Required,

	[property: JsonPropertyName("additionalProperties")]
	bool AdditionalProperties
);

public sealed record OpenAiJsonSchemaProperty(
	[property: JsonPropertyName("type")]
	string Type
);
