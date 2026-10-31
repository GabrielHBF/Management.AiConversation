using System.Linq.Expressions;

namespace Management.IAConversations.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
	Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
	Task<T> UpdateAsync(Expression<Func<T, bool>> predicate, T entity, CancellationToken cancellationToken = default);
	Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
	Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}