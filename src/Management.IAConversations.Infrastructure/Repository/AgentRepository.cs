using Management.IAConversations.Infrastructure.Repository;
using MongoDB.Driver;
using Management.IAConversations.Domain.Entities;
using Management.IAConversations.Domain.Interfaces;

namespace Management.IAConversations.Infrastructure.Repository;

public class AgentRepository(IMongoDatabase mongoDatabase) : BaseRepository<AgentEntity>(mongoDatabase, "agents"), IAgentRepository;