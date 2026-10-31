using Management.IAConversations.Infrastructure.OpenAI.Models.Response.Types;
using System.Text.Json.Serialization;

namespace Management.IAConversations.Infrastructure.OpenAI.Models.Response;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(OpenAiInputText), "input_text")]
[JsonDerivedType(typeof(OpenAiInputFile), "input_file")]
[JsonDerivedType(typeof(OpenAiInputImage), "input_image")]
public abstract record OpenAiInputContent;
