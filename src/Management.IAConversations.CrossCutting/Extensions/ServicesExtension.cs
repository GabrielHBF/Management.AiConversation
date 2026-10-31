using Management.IAConversations.Application.Context;
using Management.IAConversations.Application.Gateways;
using Management.IAConversations.Application.OpenAI.Services;
using Management.IAConversations.Application.Services.KernelFunctions;
using Management.IAConversations.Domain.Services;
using Management.IAConversations.Domain.Services.KernelFunction;
using Management.IAConversations.Infrastructure.Integrations.Gateways;
using Management.IAConversations.Infrastructure.Integrations.OpenAI.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace Management.IAConversations.CrossCutting.Extensions
{
	public static class ServicesExtension
	{
		public static IServiceCollection AddDomainServices(this IServiceCollection servicesCollection)
		{
			servicesCollection.AddScoped<IOpenAiServices, OpenAiServices>();
			servicesCollection.AddScoped<IOpenAiDocumentValidator, OpenAiDocumentValidator>();
			servicesCollection.AddScoped<IKernelFunctionService, KernelFunctionService>();
			servicesCollection.AddScoped<IChatGptGateway, ChatGptGateway>();
			servicesCollection.AddScoped<ConversationContext>();
			return servicesCollection;
		}
	}
}