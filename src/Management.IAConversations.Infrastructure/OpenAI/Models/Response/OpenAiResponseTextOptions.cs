using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response;

public sealed record OpenAiResponseTextOptions([property: JsonPropertyName("format")] OpenAiJsonSchemaFormat Format);
