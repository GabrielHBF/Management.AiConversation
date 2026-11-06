namespace AiConsole.Interfaces;

public interface IAsaConversationService
{
    Task StartAsync(CancellationToken cancellationToken = default);
}