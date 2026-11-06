namespace AiConsole.Interfaces;

public interface ISpeechRecognitionService
{
    Task<string> TranscribeAsync(Stream audio, CancellationToken cancellationToken = default);
}