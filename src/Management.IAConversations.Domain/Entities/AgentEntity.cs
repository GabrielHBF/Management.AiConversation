using MongoDB.Bson.Serialization.Attributes;

namespace Management.IAConversations.Domain.Entities;

public class AgentEntity
{
	[BsonId]
	public Guid Id { get; private set; } = Guid.NewGuid();

	public string AgentId { get; private set; } = string.Empty;

	public string AgentName { get; private set; } = string.Empty;

	public AgentEntity SetAgentId(string agentId)
	{
		AgentId = agentId;
		return this;
	}

	public AgentEntity SetAgentName(string agentName)
	{
		AgentName = agentName;
		return this;
	}

	public Builder ToBuilder() => new()
	{
		Id = Id,
		AgentId = AgentId,
		AgentName = AgentName
	};

	public sealed class Builder
	{
		internal Guid Id { get; set; } = Guid.NewGuid();

		internal string AgentId { get; set; } = string.Empty;

		internal string AgentName { get; set; } = string.Empty;

		public static Builder Create() => new();

		public Builder SetId(Guid id)
		{
			Id = id;
			return this;
		}

		public Builder SetAgentId(string agentId)
		{
			AgentId = agentId;
			return this;
		}

		public Builder SetAgentName(string agentName)
		{
			AgentName = agentName;
			return this;
		}

		public AgentEntity Build()
		{
			return new AgentEntity
			{
				Id = Id,
				AgentId = AgentId,
				AgentName = AgentName
			};
		}
	}
}