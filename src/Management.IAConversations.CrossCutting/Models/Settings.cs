namespace Management.IAConversations.CrossCutting.Models;

public interface ISettings
{
	public MongoSettings MongoSettings { get; }
	public ChatGptSettings ChatGptSettings { get; }
	public NvidiaAiSettings NvidiaAiSettings { get; }
	public OllamaSettings OllamaSettings { get; }
}

public class Settings : ISettings
{
	public required MongoSettings MongoSettings { get; set; }
	public required ChatGptSettings ChatGptSettings { get; set; }
	public required NvidiaAiSettings NvidiaAiSettings { get; set; }
	public required OllamaSettings OllamaSettings { get; set; }
}