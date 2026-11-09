using Management.IAConversations.Application.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Management.IAConversations.Infrastructure.Integrations.Kernels.Agents;

public class AsaCodeAgent(
	[FromKeyedServices(IaConversationsConstants.NvidiaAiClient)]
	IChatCompletionService chatCompletionService)
{
	public async Task<string> ExecuteAsync(string description, CancellationToken cancellationToken = default)
	{
		var promptStream = Path.Combine(AppContext.BaseDirectory, "Agents", "Prompts", "ASA_CodePrompt.md");

		var systemPrompt = await File.ReadAllTextAsync(promptStream, cancellationToken);

		var history = new ChatHistory();

		history.AddSystemMessage(systemPrompt);
		history.AddUserMessage(description);

		var response = await chatCompletionService.GetChatMessageContentAsync(
			history,
			cancellationToken: cancellationToken);

		return response.Content ?? string.Empty;
	}
}