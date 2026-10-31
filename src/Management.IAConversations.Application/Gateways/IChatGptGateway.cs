using Management.IAConversations.Application.OpenAI.Models.Agent.Events;
using Management.IAConversations.Domain.Results;
using Management.IAConversations.Application.OpenAI.Models.Session;

namespace Management.IAConversations.Application.Gateways;

public interface IChatGptGateway
{
	Task<Result> SendMessageToAgentSessionAsync(string message, string? sessionId, CancellationToken cancellationToken = default);
	Task<Result<SessionResponse>> GetNewSessionIdAsync(string agenteId, CancellationToken cancellationToken = default);
	Task<Result<AgentEventStream>> OpenEventStreamAsync(string sessionId, CancellationToken cancellationToken);
	Task<Result> SendToolResultAsync(AgentTool agentTool, CancellationToken cancellationToken);
}