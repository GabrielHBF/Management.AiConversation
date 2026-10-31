using Microsoft.Extensions.Configuration;
using Management.IAConversations.CrossCutting.Models;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class SettingsExtensions
{
	public static Settings ApplyEnvironmentOverridesToSettings(this IConfiguration configuration)
	{
		var settings = configuration.GetSection("Settings").Get<Settings>()!;

		settings.MongoSettings!.ConnectionString = GetEnvOrDefault("ConnectionString_Mongo", settings.MongoSettings!.ConnectionString);
		settings.MongoSettings!.Database = GetEnvOrDefault("Database_Mongo", settings.MongoSettings!.Database);
		settings.ChatGptSettings.ApiKey = GetEnvOrDefault("Chat_Gpt_Api_Key", settings.ChatGptSettings.ApiKey);
		settings.ChatGptSettings.Url = GetEnvOrDefault("Chat_Gtp_Url", settings.ChatGptSettings.Url);
		settings.ChatGptSettings.Model = GetEnvOrDefault("Chat_Gpt_Model", settings.ChatGptSettings.Model);
		settings.NvidiaAiSettings.ModelID = GetEnvOrDefault("Nvidia_Ai_Model_ID", settings.NvidiaAiSettings.ModelID);
		settings.NvidiaAiSettings.ApiKey = GetEnvOrDefault("Nvidia_Ai_Api_Key", settings.NvidiaAiSettings.ApiKey);
		settings.NvidiaAiSettings.Url = GetEnvOrDefault("Nvidia_Ai_Url", settings.NvidiaAiSettings.Url);

		return settings;
	}

	private static string GetEnvOrDefault(string key, string? fallback)
	{
		var value = Environment.GetEnvironmentVariable(key);
		return string.IsNullOrWhiteSpace(value) ? fallback ?? "" : value;
	}
}