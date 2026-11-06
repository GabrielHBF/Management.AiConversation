using AiConsole.Interfaces;
using AiConsole.Services.Http.Clients;
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
}