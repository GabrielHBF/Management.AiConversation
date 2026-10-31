using HelpWaiter.Abstractions.Command;
using Management.IAConversations.Domain.Results;
using Microsoft.Extensions.Logging;
using Management.IAConversations.Application.Context;
using Management.IAConversations.Application.Errors;
using Management.IAConversations.Application.Gateways;
using Management.IAConversations.Application.Mapper;
using Management.IAConversations.Application.Response;
using Management.IAConversations.Domain.Interfaces;
using Management.IAConversations.Domain.Services;

namespace Management.IAConversations.Application.Input.Commands;

public class SendConversationMessageCommandHandler(
	IAgentRepository agentRepository,
	ISessionRespository leadSessionRespository,
	IChatGptGateway chatGptGateway,
	IOpenAiServices openAiServices,
	ILogger<SendConversationMessageCommandHandler> logger,
	ConversationContext conversationContext) : ICommandHandler<SendConversationMessageCommand, Result<ConversationChatResponse>>
{
	public async Task<Result<ConversationChatResponse>> HandleAsync(SendConversationMessageCommand command, CancellationToken cancellationToken = default(CancellationToken))
	{
		var agent = await agentRepository.FindOneAsync(x => x.AgentName == command.SendConversationRequest.AgentName, cancellationToken);
		var conversationId = command.ConversationId;

		if (agent is null)
		{
			logger.LogError("Error when retrieving agent. Agent with name {AgentName} - Error {ErrorCode}",
				command.SendConversationRequest.AgentName,
				AiErrors.AgentNotFoundError.Code);

			return Result<ConversationChatResponse>.Failure(AiErrors.AgentNotFoundError);
		}

		if (conversationId is null)
		{
			logger.LogInformation("No active session found for ConversationId {ConversationId}. Initializing a new session.",
				command.ConversationId);

			var resultNewSession = await chatGptGateway.GetNewSessionIdAsync(agent.AgentId, cancellationToken);

			if (resultNewSession.IsFailure)
			{
				return Result<ConversationChatResponse>.Failure(resultNewSession.Error);
			}

			var sessionId = resultNewSession.Value.Id;

			var leadEntity = LeadMapper.ToNewEntity(sessionId);

			conversationId = leadEntity.Id.ToString();

			await leadSessionRespository.CreateAsync(leadEntity, cancellationToken);

			logger.LogInformation("New session created successfully for ConversationId {ConversationId}.", command.ConversationId);
		}

		var lead = await leadSessionRespository.FindOneAsync(x => x.Id == Guid.Parse(conversationId), cancellationToken);

		conversationContext.SessionId = lead!.SessionId;
		conversationContext.ConversationId = lead.Id;

		var streamResult = await chatGptGateway.OpenEventStreamAsync(
			lead!.SessionId!,
			cancellationToken);

		if (streamResult.IsFailure)
		{
			return Result<ConversationChatResponse>.Failure(streamResult.Error);
		}

		var messageResult = await chatGptGateway.SendMessageToAgentSessionAsync(
			command.SendConversationRequest.Message,
			lead.SessionId!,
			cancellationToken);

		if (messageResult.IsFailure)
		{
			return Result<ConversationChatResponse>.Failure(
				messageResult.Error);
		}

		var responseResult = await openAiServices.ReadResponseAsync(
			streamResult.Value,
			lead.SessionId!,
			cancellationToken);

		if (responseResult.IsFailure)
		{
			logger.LogError("Error when reading agent response for ConversationId {ConversationId} - SessionId {SessionId} - Error {Error}",
				command.ConversationId,
				lead.SessionId,
				responseResult.Error);

			return Result<ConversationChatResponse>.Failure(responseResult.Error);
		}

		var finalResult = await leadSessionRespository.FindOneAsync(x => x.Id == lead.Id, cancellationToken);

		return Result<ConversationChatResponse>.Success(
			MessageMapper.ToMessageResponse(conversationId, responseResult.Value, finalResult!.FileActions));
	}
}