namespace Management.IAConversations.Application.Context;

public sealed class ConversationContext
{
	public Guid ConversationId { get; set; }
	public string SessionId { get; set; } = string.Empty;
}