namespace Management.IAConversations.Application.OpenAI.Models.Agent.Events;

public record AgentTool(string SessionId, string TurnId, string CallId, string Output);