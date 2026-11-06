using AiConsole.Configurations;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);
var consoleSettings = builder.Configuration.GetConsoleSettings();
builder.Services
    .AddHttpClients(consoleSettings)
    .AddServices();

builder.Build();