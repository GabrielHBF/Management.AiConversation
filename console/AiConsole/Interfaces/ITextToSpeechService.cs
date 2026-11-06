namespace AiConsole.Interfaces;

public interface ITextToSpeechService
{
    Task SpeakAsync(string text, CancellationToken cancellationToken = default);
}