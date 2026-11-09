using Management.IAConversations.Application.Constants;
using Management.IAConversations.CrossCutting.Models;
using Management.IAConversations.Infrastructure.Integrations.Kernels.Agents;
using Management.IAConversations.Infrastructure.Integrations.Plugins.Management;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class KernelExtension
{
	public static IServiceCollection AddKernelPlugins(this IServiceCollection services, ISettings settings)
	{
		var factory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();

		var builder = services.AddKernel();

		var httpClientNvidea = factory.CreateClient(IaConversationsConstants.NvidiaAiClient);
		var httpClientOllama = factory.CreateClient(IaConversationsConstants.OllamaClient);

		builder.AddOpenAIChatCompletion(
			modelId: settings.NvidiaAiSettings.ModelID,
			apiKey: settings.NvidiaAiSettings.ApiKey,
			httpClient: httpClientNvidea,
			serviceId: IaConversationsConstants.NvidiaAiClient);

		builder.AddOllamaChatCompletion(
			modelId: settings.OllamaSettings.TextModel,
			httpClient: httpClientOllama,
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