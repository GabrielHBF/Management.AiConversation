using Management.IAConversations.Infrastructure.Integrations.Kernels.Agents;
using Microsoft.Extensions.DependencyInjection;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class AgentsExtension
{
	public static IServiceCollection AddAgents(this IServiceCollection services)
	{
		services.AddScoped<AsaCodeAgent>();

		return services;
	}
}
