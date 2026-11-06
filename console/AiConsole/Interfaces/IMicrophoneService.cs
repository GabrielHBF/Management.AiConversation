namespace AiConsole.Interfaces;

public interface IMicrophoneService
{
    Task StartRecordingAsync(CancellationToken cancellationToken = default);
    Task<Stream> StopRecordingAsync(CancellationToken cancellationToken = default);
}