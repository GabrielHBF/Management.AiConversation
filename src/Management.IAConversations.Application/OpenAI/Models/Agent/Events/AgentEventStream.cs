namespace Management.IAConversations.Application.OpenAI.Models.Agent.Events;

public sealed class AgentEventStream(HttpResponseMessage response, StreamReader reader) : IDisposable
{
	public StreamReader Reader { get; } = reader;

	public void Dispose()
	{
		Reader.Dispose();
		response.Dispose();
	}
}