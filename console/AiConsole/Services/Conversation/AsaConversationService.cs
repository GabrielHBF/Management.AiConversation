using AiConsole.Interfaces;

namespace AiConsole.Services.Conversation;

public sealed class AsaConversationService(IAsaClient asaClient, ITextToSpeechService textToSpeechService) : IAsaConversationService
{
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("A$A iniciado!");
        Console.WriteLine("Digite sua mensagem ou 'sair' para encerrar.");

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("\nVocê: ");

            var message = Console.ReadLine();

            if (string.Equals(message, "sair", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(message))
                continue;

            var response = await asaClient.SendMessageAsync(message, cancellationToken);

            if (response is null || string.IsNullOrWhiteSpace(response.Value.MessageResponse))
                continue;

            Console.WriteLine($"\nA$A: {response.Value.MessageResponse}");

            await textToSpeechService.SpeakAsync(response.Value.MessageResponse, cancellationToken);
        }
    }
}