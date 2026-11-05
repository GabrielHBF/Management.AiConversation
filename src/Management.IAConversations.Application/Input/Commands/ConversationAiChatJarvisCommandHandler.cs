using HelpWaiter.Abstractions.Command;
using Management.IAConversations.Application.Constants;
using Management.IAConversations.Application.Errors;
using Management.IAConversations.Application.Response;
using Management.IAConversations.Domain.Enums;
using Management.IAConversations.Domain.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Management.IAConversations.Application.Input.Commands;

public class ConversationAiChatJarvisCommandHandler(
	[FromKeyedServices(IaConversationsConstants.OllamaClient)]
	IChatCompletionService chatCompletionService,
	Kernel kernel) : ICommandHandler<ConversationAiChatJarvisCommand, Result<ConversationChatResponse>>
{
	public async Task<Result<ConversationChatResponse>> HandleAsync(ConversationAiChatJarvisCommand request, CancellationToken cancellationToken = default(CancellationToken))
	{
		var history = new ChatHistory();

		history.AddSystemMessage(Path.Combine(AppContext.BaseDirectory, "Agents", "Prompts", "JarvisPrompt.md"));
		history.AddUserMessage(request.SendConversationRequest.Message);

		var response = await chatCompletionService.GetChatMessageContentAsync(
			history,
			kernel: kernel,
			cancellationToken: cancellationToken);

		if (response is null || string.IsNullOrWhiteSpace(response.Content))
		{
			return Result<ConversationChatResponse>.Failure(
				AiErrors.JarvisResponseNotGenerated);
		}

		var converationResponse = new ConversationChatResponse(
			MessageResponse: response.Content,
			ConversationId: Guid.NewGuid().ToString(),
			FileActions: FileActions.Pending);

		return Result<ConversationChatResponse>.Success(converationResponse);
	}
}