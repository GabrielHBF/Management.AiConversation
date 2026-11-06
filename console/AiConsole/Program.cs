using AiConsole.Configurations;
using AiConsole.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);
var consoleSettings = builder.Configuration.GetConsoleSettings();
builder.Services
    .AddHttpClients(consoleSettings)
    .AddServices();

var host = builder.Build();

var asa = host.Services.GetRequiredService<IAsaConversationService>();

await asa.StartAsync();