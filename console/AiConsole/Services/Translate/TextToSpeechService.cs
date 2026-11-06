using AiConsole.Interfaces;
using System.Speech.Synthesis;

namespace AiConsole.Services.Translate;

public class TextToSpeechService : ITextToSpeechService
{
    private readonly SpeechSynthesizer _synthesizer = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public TextToSpeechService()
    {
        _synthesizer.SetOutputToDefaultAudioDevice();
        _synthesizer.SelectVoice("Microsoft Daniel");

        _synthesizer.Rate = 0;
        _synthesizer.Volume = 100;
    }

    public async Task SpeakAsync(string text, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            _synthesizer.Speak(text);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _synthesizer.Dispose();
        _semaphore.Dispose();
    }
}