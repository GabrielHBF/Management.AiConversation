using AiConsole.Interfaces;
using AiConsole.Utils.Errors;
using Management.IAConversations.Application.Response;
using Management.IAConversations.Domain.Results;
using System.Net.Http.Json;

namespace AiConsole.Services.Http.Clients;

public class AsaClient(HttpClient httpClient) : IAsaClient
{
    public async Task<Result<ConversationChatResponse>> SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/v1/chat/jarvis", new { Message = message }, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return Result<ConversationChatResponse>.Failure(AsaErrors.FailedToSendMessage);
        }

        var result = await response.Content.ReadFromJsonAsync<ConversationChatResponse>(cancellationToken: cancellationToken);

        return Result<ConversationChatResponse>.Success(result!);
    }
}