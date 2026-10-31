using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using Management.IAConversations.CrossCutting.Models;

namespace Management.IAConversations.CrossCutting.Extensions;

public static class MongoExtensions
{
	public static IServiceCollection AddMongo(this IServiceCollection services, MongoSettings mongoSettings)
	{
		BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
		var clientSettings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionString);
		var options = new InstrumentationOptions { CaptureCommandText = true };
		clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber(options));
		var mongoClient = new MongoClient(clientSettings);
		services.AddSingleton<IMongoClient>(_ => mongoClient);

		services.AddSingleton(sp =>
		{
			var mongoDbClient = sp.GetService<IMongoClient>();
			var db = mongoDbClient!.GetDatabase(mongoSettings.Database);

			db.CreateIndexes();

			return db;
		});

		return services;
	}

	private static void CreateIndexes(this IMongoDatabase database)
	{
		var options = new CreateIndexOptions { Background = true };
	}
}