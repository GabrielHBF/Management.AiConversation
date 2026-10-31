using Management.IAConversations.Domain.Enums;

namespace Management.IAConversations.Application.Response;

public record ConversationChatResponse(string? MessageResponse, string ConversationId, FileActions FileActions);