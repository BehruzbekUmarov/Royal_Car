using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Repositories;

public interface IEntityDbSet
{
	public DbSet<Car> Car { get; set; }
}
