using Management.IAConversations.Domain.Results;

namespace Management.IAConversations.CrossCutting.Models;

public interface ISettings
{
	public MongoSettings MongoSettings { get; }
	public ChatGptSettings ChatGptSettings { get; }
}

public class Settings : ISettings
{
	public required MongoSettings MongoSettings { get; set; }
	public required ChatGptSettings ChatGptSettings { get; set; }
}