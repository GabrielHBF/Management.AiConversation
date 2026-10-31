using Management.IAConversations.Application.Request;
using Management.IAConversations.Domain.Entities;

namespace Management.IAConversations.Application.Mapper;

public static class AgentMapper
{
	public static AgentEntity ToNewEntity(this AddAgentRequest request)
	{
		return new AgentEntity().ToBuilder()
			.SetId(Guid.NewGuid())
			.SetAgentId(request.AgentId)
			.SetAgentName(request.Name)
			.Build();
	}
}