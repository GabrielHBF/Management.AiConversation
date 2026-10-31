using Microsoft.Extensions.DependencyInjection;
using Management.IAConversations.Domain.Interfaces;
using Management.IAConversations.Infrastructure.Repository;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class RepositoriesExtensions
{
	public static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<ISessionRespository, SessionRespository>();
		services.AddScoped<IAgentRepository, AgentRepository>();
		return services;
	}
}