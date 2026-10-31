using HelpWaiter.Abstractions.Command;
using Management.IAConversations.Application.Input.Commands;
using Management.IAConversations.Domain.Results;
using Microsoft.Extensions.Logging;
using Management.IAConversations.Application.Errors;
using Management.IAConversations.Application.Mapper;
using Management.IAConversations.Domain.Interfaces;

namespace Management.IAConversations.Application.Input.Commands;

public class AddAgentCommandHandler(IAgentRepository agentRepository,
	ILogger<AddAgentCommandHandler> logger) : ICommandHandler<AddAgentCommand, Result>
{
	public async Task<Result> HandleAsync(AddAgentCommand command, CancellationToken cancellationToken = default(CancellationToken))
	{
		var agentExist = await agentRepository.FindOneAsync(x => x.AgentId == command.AddAgentRequest.AgentId, cancellationToken);

		if (agentExist != null)
		{
			return Result.Failure(AiErrors.AgentAlreadyExistsError);
		}
		var entity = command.AddAgentRequest.ToNewEntity();

		await agentRepository.CreateAsync(entity, cancellationToken);

		logger.LogInformation("Agent {AgentId} was successfully added.", entity.Id);

		return Result.Success();
	}
}