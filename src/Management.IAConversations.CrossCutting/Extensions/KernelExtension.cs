using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Management.IAConversations.Infrastructure.Integrations.Plugins.Governaments;
using Management.IAConversations.CrossCutting.Models;
using Management.IAConversations.Application.Constants;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class KernelExtension
{
	public static IServiceCollection AddKernelPlugins(this IServiceCollection services, ISettings settings)
	{
		services.AddKernel();

		services.AddTransient<Kernel>(sp =>
		{
			var httpClient = sp.GetRequiredService<IHttpClientFactory>()
			.CreateClient(IaConversationsConstants.NvidiaAiClient);

			var builder = Kernel.CreateBuilder();

			builder.AddOpenAIChatCompletion(
				modelId: settings.NvidiaAiSettings.ModelID,
				endpoint: new Uri(settings.NvidiaAiSettings.Url),
				apiKey: settings.NvidiaAiSettings.ApiKey,
				serviceId: IaConversationsConstants.NvidiaAiClient,
				httpClient: httpClient);

			var kernel = builder.Build();

			kernel.Plugins.AddFromType<KenelPluginsGovernaments>("governments_leads", sp);

			return kernel;
		});

		return services;
	}
}