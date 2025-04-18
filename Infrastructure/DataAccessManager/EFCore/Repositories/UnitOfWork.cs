using Application.Common.Repositories;
using Infrastructure.DataAccessManager.EFCore.Context;

namespace Infrastructure.DataAccessManager.EFCore.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly DataContext _context;
    public UnitOfWork(DataContext context)
    {
        _context = context;
    }
    public void Save()
	{
		_context.SaveChanges();
	}

	public async Task SaveAsync(CancellationToken cancellationToken = default)
	{
		await _context.SaveChangesAsync(cancellationToken);
	}
}
