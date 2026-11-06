using AiConsole.Configurations;
using AiConsole.Interfaces;
using AiConsole.Services.Audio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();

var modelPath = await WhisperModelInitializer.EnsureModelExistsAsync();

var consoleSettings = builder.Configuration.GetConsoleSettings();

builder.Services
    .AddHttpClients(consoleSettings)
    .AddServices(modelPath);

var host = builder.Build();

Console.Title = "A.$.A";
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Clear();

var asa = host.Services.GetRequiredService<IAsaConversationService>();

await asa.StartAsync();