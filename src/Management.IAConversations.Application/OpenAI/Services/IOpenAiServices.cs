using Management.IAConversations.Application.OpenAI.Models.Agent.Events;
using Management.IAConversations.Domain.Results;

namespace Management.IAConversations.Domain.Services;

public interface IOpenAiServices
{
	Task<Result<string?>> ReadResponseAsync(AgentEventStream stream, string sessionId, CancellationToken cancellationToken = default);
}