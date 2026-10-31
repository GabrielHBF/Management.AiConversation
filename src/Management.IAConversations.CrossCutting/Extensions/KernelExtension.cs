using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Management.IAConversations.Infrastructure.Integrations.Plugins.Governaments;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class KernelExtension
{
	public static IServiceCollection AddKernelPlugins(this IServiceCollection services)
	{
		services.AddKernel();

		services.AddScoped<Kernel>(sp =>
		{
			var kernel = new Kernel(sp);

			kernel.Plugins.AddFromType<KenelPluginsGovernaments>("governments_leads", sp);

			return kernel;
		});

		return services;
	}
}