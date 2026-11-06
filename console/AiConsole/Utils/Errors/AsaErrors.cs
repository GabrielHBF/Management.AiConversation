using Management.IAConversations.Domain.Results;

namespace AiConsole.Utils.Errors;

public static class AsaErrors
{
    public static readonly Error FailedToSendMessage = new("FailedToSendMessage", "Message delivery failed.");
}