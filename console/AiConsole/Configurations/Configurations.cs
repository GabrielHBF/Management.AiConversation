using AiConsole.Interfaces;
using AiConsole.Services.Http.Clients;
using AiConsole.Services.Translate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiConsole.Configurations;

public static class Configurations
{
    public static ConsoleSettings GetConsoleSettings(this IConfiguration configuration)
    {
        var consoleSettingsSection = configuration.GetSection("Settings:ConsoleSettings");

        return consoleSettingsSection.Get<ConsoleSettings>()!;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, ConsoleSettings consoleSettings)
    {
        services.AddHttpClient<IAsaClient, AsaClient>(client =>
        {
            client.BaseAddress = new Uri(consoleSettings.Url);
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services )
    {
        services.AddSingleton<IMicrophoneService, MicrophoneService>();
        services.AddSingleton<ITextToSpeechService, TextToSpeechService>();

        return services;
    }
}