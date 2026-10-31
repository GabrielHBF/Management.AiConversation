using Management.IAConversations.Domain.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Management.IAConversations.Infrastructure.Repository;

public class BaseRepository<T>(IMongoDatabase mongoDatabase, string collectionName) : IBaseRepository<T> where T : class
{
	private readonly IMongoCollection<T> _collection = mongoDatabase.GetCollection<T>(collectionName);

	public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
	{
		await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
		return entity;
	}

	public async Task<T> UpdateAsync(Expression<Func<T, bool>> predicate, T entity, CancellationToken cancellationToken = default(CancellationToken))
	{
		await _collection.ReplaceOneAsync(predicate, entity, cancellationToken: cancellationToken);
		return entity;
	}

	public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		var result = await _collection.DeleteOneAsync(predicate, cancellationToken);
		return result.DeletedCount > 0;
	}

	public async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
	{
		var agent = await _collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);
		return agent;
	}
}