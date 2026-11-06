using Whisper.net.Ggml;

namespace AiConsole.Services.Audio;

public static class WhisperModelInitializer
{
    public static async Task<string> EnsureModelExistsAsync(
        CancellationToken cancellationToken = default)
    {
        var modelDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ASA",
            "Models");

        Directory.CreateDirectory(modelDirectory);

        var modelPath = Path.Combine(modelDirectory, "ggml-base.bin");

        if (File.Exists(modelPath))
            return modelPath;

        Console.WriteLine("Baixando modelo de reconhecimento de voz...");

        var temporaryPath = $"{modelPath}.download";

        try
        {
            using var modelStream =
                await WhisperGgmlDownloader.Default.GetGgmlModelAsync(
                    GgmlType.Base);

            await using (var fileWriter = File.Create(temporaryPath))
            {
                await modelStream.CopyToAsync(
                    fileWriter,
                    cancellationToken);
            }

            File.Move(temporaryPath, modelPath, overwrite: true);

            return modelPath;
        }
        catch
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);

            throw;
        }
    }
}