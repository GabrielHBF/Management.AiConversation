using System.Text.Json;

namespace Management.IAConversations.Application.Helpers;

public static class JsonResponseHelper
{
	public static string ToJson(
		bool success,
		string message,
		object? data = null,
		string? error = null)
	{
		return JsonSerializer.Serialize(new
		{
			success,
			message,
			data,
			error
		});
	}
}