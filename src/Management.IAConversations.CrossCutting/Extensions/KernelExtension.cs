using Management.IAConversations.Application.Constants;
using Management.IAConversations.CrossCutting.Models;
using Management.IAConversations.Infrastructure.Integrations.Plugins.Management;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class KernelExtension
{
	public static IServiceCollection AddKernelPlugins(this IServiceCollection services, ISettings settings)
	{
		var builder = services.AddKernel();

		var httpClient = new HttpClient();

		builder.AddOpenAIChatCompletion(
			modelId: settings.NvidiaAiSettings.ModelID,
			endpoint: new Uri(settings.NvidiaAiSettings.Url),
			apiKey: settings.NvidiaAiSettings.ApiKey,
			serviceId: IaConversationsConstants.NvidiaAiClient,
			httpClient: httpClient);

		builder.AddOllamaChatCompletion(
			modelId: settings.OllamaSettings.TextModel,
			endpoint: new Uri(settings.OllamaSettings.Endpoint),
			serviceId: IaConversationsConstants.OllamaClient);

		services.AddScoped<KernelPluginsFunctions>();

		services.AddTransient<KernelPluginCollection>(sp =>
		{
			var plugins = new KernelPluginCollection();

			var functionsPlugin =
				sp.GetRequiredService<KernelPluginsFunctions>();

			plugins.AddFromObject(functionsPlugin, "Functions", sp);

			return plugins;
		});

		return services;
	}
}