using AiConsole.Interfaces;
using AiConsole.Services.Translate;
using AiConsole.UI;
using AiConsole.Utils.ResponseVoice;

namespace AiConsole.Services.Conversation;

public sealed class AsaConversationService(
    IAsaClient asaClient,
    ITextToSpeechService textToSpeechService,
    IMicrophoneService microphoneService,
    ISpeechRecognitionService speechRecognitionService) : IAsaConversationService
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
            {
                await microphoneService.StartRecordingAsync(cancellationToken);

                ConsoleInterface.ShowListening();

                Console.WriteLine("Pressione ENTER para finalizar a gravação.");

                Console.ReadLine();

                await using var audio =
                    await microphoneService.StopRecordingAsync(cancellationToken);

                Console.WriteLine("Transcrevendo áudio...");

                message = await speechRecognitionService.TranscribeAsync(audio, cancellationToken);

                if (string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine("Não consegui identificar sua fala.");
                    continue;
                }
            }

            ConsoleInterface.ShowUserMessage(message);
            ConsoleInterface.ShowThinking();

            var response = await asaClient.SendMessageAsync(
                message,
                cancellationToken);

            if (response is null ||
                string.IsNullOrWhiteSpace(response.Value.MessageResponse))
                continue;

            ConsoleInterface.ShowJarvisMessage(
                response.Value.MessageResponse);

            var speechText = SpeechTextSanitizer.RemoveEmojis(
                response.Value.MessageResponse);

            await textToSpeechService.SpeakAsync(
                speechText,
                cancellationToken);
        }

        Console.WriteLine("\nA$A encerrado.");
    }
}