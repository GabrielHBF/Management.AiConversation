namespace Management.IAConversations.CrossCutting.Models;

public class MongoSettings
{
	public required string Database { get; set; }
	public required string ConnectionString { get; set; }
}
