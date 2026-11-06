using Management.IAConversations.Application.Response;
using Management.IAConversations.Domain.Results;

namespace AiConsole.Interfaces;

public interface IAsaClient
{
    Task<Result<ConversationChatResponse>> SendMessageAsync(string message, CancellationToken cancellationToken = default);
}