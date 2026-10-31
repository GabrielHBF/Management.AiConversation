using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response.Types;

public sealed record OpenAiInputFile(
	[property: JsonPropertyName("filename")]
	string FileName,

	[property: JsonPropertyName("file_data")]
	string FileData,

	[property: JsonPropertyName("detail")]
	string? Detail = "low"
) : OpenAiInputContent;
