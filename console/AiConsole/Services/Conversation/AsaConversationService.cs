using AiConsole.Interfaces;
using AiConsole.UI;

namespace AiConsole.Services.Conversation;

public sealed class AsaConversationService(IAsaClient asaClient, ITextToSpeechService textToSpeechService) : IAsaConversationService
{
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        Console.Clear();

        ConsoleInterface.ShowHeader();

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsoleInterface.ShowPrompt();

            var message = Console.ReadLine();

            if (string.Equals(message, "sair",
                StringComparison.OrdinalIgnoreCase))
                break;

            if (string.IsNullOrWhiteSpace(message))
                continue;

            ConsoleInterface.ShowUserMessage(message);
            ConsoleInterface.ShowThinking();

            var response = await asaClient.SendMessageAsync(message, cancellationToken);

            if (response is null ||
                string.IsNullOrWhiteSpace(response.Value.MessageResponse))
                continue;

            ConsoleInterface.ShowJarvisMessage(response.Value.MessageResponse);

            await textToSpeechService.SpeakAsync(response.Value.MessageResponse, cancellationToken);
        }

        Console.WriteLine("\nJarvis encerrado.");
    }
}