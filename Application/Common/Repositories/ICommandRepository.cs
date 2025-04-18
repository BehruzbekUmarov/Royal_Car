using Domain.Common;

namespace Application.Common.Repositories;

public interface ICommandRepository<T> where T : BaseEntity
{
	Task CreateAsync(T entity, CancellationToken cancellationToken = default);
	void Create(T entity);
	void Update(T entity);
	void Delete(T entity);
	Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);
	T? Get(Guid id);
	IQueryable<T> GetQuery();
}
