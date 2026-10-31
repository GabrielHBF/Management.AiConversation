using System.Text.Json.Serialization;

namespace Management.IAConversations.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FileActions
{
	Pending,
	Requested,
	Sent
}