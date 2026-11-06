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

            var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            EventHandler<SpeakCompletedEventArgs>? handler = null;

            handler = (_, args) =>
            {
                if (args.Error is not null)
                {
                    completion.TrySetException(args.Error);
                }
                else if (args.Cancelled)
                {
                    completion.TrySetCanceled(cancellationToken);
                }
                else
                {
                    completion.TrySetCanceled(cancellationToken);
                }
            };

            _synthesizer.SpeakCompleted += handler;

            using var registration = cancellationToken.Register(() => _synthesizer.SpeakAsyncCancelAll());

            try
            {
                _synthesizer.SpeakAsync(text);

                await completion.Task;
            }
            finally
            {
                _synthesizer.SpeakCompleted -= handler;
            }
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