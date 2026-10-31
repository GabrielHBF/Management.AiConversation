using Management.IAConversations.Application.Response;
using Management.IAConversations.Domain.Enums;

namespace Management.IAConversations.Application.Mapper;

public static class MessageMapper
{
	public static ConversationChatResponse ToMessageResponse(string conversationId, string? response, FileActions fileActions)
	{
		return new ConversationChatResponse
		(
			MessageResponse: response,
			ConversationId: conversationId,
			FileActions: fileActions
		);
	}
}