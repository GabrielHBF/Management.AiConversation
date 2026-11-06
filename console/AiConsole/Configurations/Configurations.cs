using Microsoft.Extensions.Configuration;

namespace AiConsole.Configurations;

public static class Configurations
{
    public static ConsoleSettings GetConsoleSettings(IConfiguration configuration)
    {
        var consoleSettingsSection = configuration.GetSection("Settings:ConsoleSettings");

        return consoleSettingsSection.Get<ConsoleSettings>()!;
    }
}