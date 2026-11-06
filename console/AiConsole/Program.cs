using AiConsole.Configurations;
using AiConsole.Services.Conversation;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);
var consoleSettings = builder.Configuration.GetConsoleSettings();
builder.Services
    .AddHttpClients(consoleSettings)
    .AddServices();

builder.Build();

var asa = host.Services.GetRequiredService<IJarvisConversationService>();

await asa.StartAsync();