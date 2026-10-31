namespace Management.IAConversations.Domain.Results;

public sealed record Error(string Code, string Message)
{
	public static Error None { get; } = new(string.Empty, string.Empty);
}