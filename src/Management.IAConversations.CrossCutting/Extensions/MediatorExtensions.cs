using HelpWaiter.Extension;
using Microsoft.Extensions.DependencyInjection;
using Management.IAConversations.Application.Input.Commands;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class MediatorExtensions
{
	public static IServiceCollection AddMediator(this IServiceCollection services)
	{

		services.AddWaiter(typeof(AddAgentCommandHandler).Assembly);

		return services;

	}
}