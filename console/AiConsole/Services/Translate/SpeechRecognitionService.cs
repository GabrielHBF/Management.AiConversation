using AiConsole.Interfaces;
using System.Text;
using Whisper.net;

namespace AiConsole.Services.Translate;

public class SpeechRecognitionService : ISpeechRecognitionService
{
    private readonly WhisperFactory _factory;
    private readonly WhisperProcessor _processor;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public SpeechRecognitionService(string modelPath)
    {
        _factory = WhisperFactory.FromPath(modelPath);

        _processor = _factory.CreateBuilder()
            .WithLanguage("pt")
            .Build();
    }

    public async Task<string> TranscribeAsync(
        Stream audio,
        CancellationToken cancellationToken = default)
    {
        if (audio.CanSeek)
            audio.Position = 0;

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            var transcription = new StringBuilder();

            await foreach (var segment in _processor.ProcessAsync(audio))
            {
                cancellationToken.ThrowIfCancellationRequested();

                transcription.Append(segment.Text);
            }

            return transcription.ToString().Trim();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _processor.Dispose();
        _factory.Dispose();
        _semaphore.Dispose();
    }
}