using Application.Common.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DataAccessManager.EFCore.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessManager.EFCore.Context;

public class DataContext : DbContext, IEntityDbSet
{
    public DataContext(DbContextOptions<DataContext> options) 
		: base(options)
    {  
    }
    public DbSet<Car> Car { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new CarConfiguration());
	}
}
