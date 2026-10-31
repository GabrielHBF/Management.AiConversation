using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response.Types;

public sealed record OpenAiInputText([property: JsonPropertyName("text")] string Text) : OpenAiInputContent;
