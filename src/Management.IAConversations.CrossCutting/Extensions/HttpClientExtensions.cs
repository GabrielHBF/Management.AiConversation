using Microsoft.Extensions.DependencyInjection;
using Management.IAConversations.Application.Constants;
using Management.IAConversations.CrossCutting.Models;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class HttpClientExtensions
{
	public static IServiceCollection AddHttpClients(this IServiceCollection services, ISettings settings)
	{
		services.AddHttpClient(IaConversationsConstants.ChatGptClient, client =>
		{
			client.BaseAddress = new Uri(settings.ChatGptSettings.Url);
			client.Timeout = Timeout.InfiniteTimeSpan;
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ChatGptSettings.ApiKey}");
			client.DefaultRequestHeaders.Add("OpenAI-Beta", settings.ChatGptSettings.Model);
		});

		services.AddHttpClient(IaConversationsConstants.NvidiaAiClient, client =>
		{
			client.Timeout = Timeout.InfiniteTimeSpan;
		});

		return services;
	}
}