using HelpWaiter.Abstractions.Command;
using Management.IAConversations.Application.Constants;
using Management.IAConversations.Application.Response;
using Management.IAConversations.Domain.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Management.IAConversations.Application.Input.Commands;

public class ConversationAiChatJarvisCommandHandler(
	[FromKeyedServices(IaConversationsConstants.OllamaClient)]
	IChatCompletionService chatCompletionService) : ICommandHandler<ConversationAiChatJarvisCommand, Result<ConversationChatResponse>>
{
	public Task<Result<ConversationChatResponse>> HandleAsync(ConversationAiChatJarvisCommand request, CancellationToken cancellationToken = default(CancellationToken))
	{
		var history = new ChatHistory();

		history.AddSystemMessage();
	}
}
