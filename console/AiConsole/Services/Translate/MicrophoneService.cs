using AiConsole.Interfaces;

namespace AiConsole.Services.Translate;

public class MicrophoneService : IMicrophoneService
{
    public Task StartRecordingAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> StopRecordingAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}