using Management.IAConversations.CrossCutting.Extensions;
using Management.IAConversations.CrossCutting.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("appsettings.json", false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, reloadOnChange: true)
	.AddEnvironmentVariables();

var applicationSettings = builder.Configuration.ApplyEnvironmentOverridesToSettings();

builder.Services.AddSingleton<ISettings>(applicationSettings);

builder.Services
	.AddProblemDetails()
	.AddHttpClients(applicationSettings)
	.AddMongo(applicationSettings.MongoSettings)
	.AddApiSpecification()
	.AddDomainServices()
	.AddRepositories()
	.AddEndpointsApiExplorer()
	.AddKernelPlugins(applicationSettings)
	.AddMediator()
	.AddControllers();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.MapOpenApi();
app.UseSpecification();
app.MapControllers();

await app.RunAsync();
