using HelpWaiter.Abstractions.Command;
using Management.IAConversations.Application.Request;
using Management.IAConversations.Domain.Results;

namespace Management.IAConversations.Application.Input.Commands;

public record AddAgentCommand(AddAgentRequest AddAgentRequest) : ICommand<Result>;