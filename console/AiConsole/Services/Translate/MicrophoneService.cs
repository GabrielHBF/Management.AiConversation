using AiConsole.Interfaces;
using NAudio.Wave;

namespace AiConsole.Services.Translate;

public class MicrophoneService : IMicrophoneService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private WaveIn? _microphone;
    private WaveFileWriter? _writer;
    private MemoryStream? _audioStream;
    private TaskCompletionSource<Exception?>? _recordingStopped;
    private bool _stopRequested;

    public async Task StartRecordingAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_microphone is not null)
                throw new InvalidOperationException(
                    "Microphone recording is already active.");

            var audioStream = new MemoryStream();

            var waveFormat = new WaveFormat(16000, 16, 1);
            var writer = new WaveFileWriter(audioStream, waveFormat);

            var microphone = new WaveIn
            {
                DeviceNumber = 0,
                WaveFormat = waveFormat,
                BufferMilliseconds = 100
            };

            _audioStream = audioStream;
            _writer = writer;
            _microphone = microphone;

            _recordingStopped = new TaskCompletionSource<Exception?>(TaskCreationOptions.RunContinuationsAsynchronously);

            _stopRequested = false;

            microphone.DataAvailable += OnDataAvailable;
            microphone.RecordingStopped += OnRecordingStopped;

            try
            {
                microphone.StartRecording();
            }
            catch
            {
                microphone.DataAvailable -= OnDataAvailable;
                microphone.RecordingStopped -= OnRecordingStopped;

                microphone.Dispose();
                writer.Dispose();
                audioStream.Dispose();

                ResetState();

                throw;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Stream> StopRecordingAsync(CancellationToken cancellationToken = default)
    {
        Task<Exception?> stoppedTask;

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_microphone is null || _recordingStopped is null)
                throw new InvalidOperationException(
                    "Microphone is not recording.");

            if (!_stopRequested)
            {
                _stopRequested = true;
                _microphone.StopRecording();
            }

            stoppedTask = _recordingStopped.Task;
        }
        finally
        {
            _semaphore.Release();
        }

        var recordingError = await stoppedTask;

        await _semaphore.WaitAsync(CancellationToken.None);

        try
        {
            var audioStream = _audioStream!;

            _writer!.Dispose();

            _microphone!.DataAvailable -= OnDataAvailable;
            _microphone.RecordingStopped -= OnRecordingStopped;

            _microphone.Dispose();

            ResetState();

            if (recordingError is not null)
            {
                audioStream.Dispose();

                throw new InvalidOperationException(
                    "An error occurred while recording audio.",
                    recordingError);
            }

            audioStream.Position = 0;

            return audioStream;
        }
        finally
        {
            _semaphore.Release();
        }
    }


    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        _writer?.Write(e.Buffer, 0, e.BytesRecorded);
    }

    private void OnRecordingStopped(object? sender, StoppedEventArgs e)
    {
        _recordingStopped?.TrySetResult(e.Exception);
    }

    private void ResetState()
    {
        _microphone = null;
        _writer = null;
        _audioStream = null;
        _recordingStopped = null;
        _stopRequested = false;
    }
}