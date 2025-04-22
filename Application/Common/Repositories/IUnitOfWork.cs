using Application.Common.Repositories.Cars;

namespace Application.Common.Repositories;

public interface IUnitOfWork
{
	ICarRepository CarRepository { get; }
	Task SaveAsync(CancellationToken cancellationToken = default);
	void Save();
}
