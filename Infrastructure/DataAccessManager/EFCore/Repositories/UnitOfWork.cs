using Application.Common.Repositories;
using Application.Common.Repositories.Cars;
using Infrastructure.DataAccessManager.EFCore.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataAccessManager.EFCore.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly DataContext _context;
	private readonly IServiceProvider _serviceProvider;
	public UnitOfWork(DataContext context, IServiceProvider serviceProvider)
	{
		_context = context;
		_serviceProvider = serviceProvider;
	}

	private ICarRepository? _carRepository;

	public ICarRepository CarRepository
	{
		get
		{
			_carRepository ??= GetRepository<ICarRepository>();
			return _carRepository;
		}
	}

	public void Dispose() => _context.Dispose();
	public void Save() => _context.SaveChanges();

	public async Task SaveAsync(CancellationToken cancellationToken = default)
		=> await _context.SaveChangesAsync(cancellationToken);

	private T GetRepository<T>() where T : class => _serviceProvider.GetRequiredService<T>();
}
