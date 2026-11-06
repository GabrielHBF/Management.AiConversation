using AiConsole.Interfaces;

namespace AiConsole.Services.Translate;

public class TextToSpeechService : ITextToSpeechService
{
    public Task SpeakAsync(string text, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}