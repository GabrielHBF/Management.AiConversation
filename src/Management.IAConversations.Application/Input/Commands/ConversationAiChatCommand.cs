using HelpWaiter.Abstractions.Command;
using Management.IAConversations.Application.Request;
using Management.IAConversations.Application.Response;
using Management.IAConversations.Domain.Results;

namespace Management.IAConversations.Application.Input.Commands;

public record class SendConversationMessageCommand(SendConversationRequest SendConversationRequest, string? ConversationId) : ICommand<Result<ConversationChatResponse>>;