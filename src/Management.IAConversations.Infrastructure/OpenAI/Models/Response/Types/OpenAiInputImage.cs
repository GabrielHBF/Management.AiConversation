using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response.Types;

public sealed record OpenAiInputImage(
	[property: JsonPropertyName("image_url")]
	string ImageUrl,

	[property: JsonPropertyName("detail")]
	string? Detail = "low"
) : OpenAiInputContent;
