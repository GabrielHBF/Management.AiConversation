using Management.IAConversations.Domain.Entities;

namespace Management.IAConversations.Application.Mapper;

public static class SessionMapper
{
	public static Session ToNewEntity(string sessionId)
	{
		return new Session().ToBuilder()
			.SetSessionId(sessionId)
			.SetId(Guid.NewGuid())
			.Build();
	}
}