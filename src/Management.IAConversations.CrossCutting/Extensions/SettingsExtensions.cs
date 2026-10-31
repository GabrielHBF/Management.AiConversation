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

		return settings;
	}

	private static string GetEnvOrDefault(string key, string? fallback)
	{
		var value = Environment.GetEnvironmentVariable(key);
		return string.IsNullOrWhiteSpace(value) ? fallback ?? "" : value;
	}
}