using Management.IAConversations.Application.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using System.ComponentModel;

namespace Management.IAConversations.Infrastructure.Integrations.Plugins.Governaments;

public class KenelPluginsFunctions(
	[FromKeyedServices(IaConversationsConstants.NvidiaAiClient)]
	IChatCompletionService chatCompletionService,
	Kernel kernel)
{
	[KernelFunction("gerar codigo")]
	[Description("Gera código com base em uma descrição.")]
	public async Task<string> GenerateCodeAsync(
		[Description("A descrição do código a ser gerado.")]
		string description,
		CancellationToken cancellationToken)
	{
		var history = new ChatHistory();

		var agent = new ChatCompletionAgent
		{
			Name = "A$A_Code",

			Instructions =

			Kernel = kernel
		};
	}
}