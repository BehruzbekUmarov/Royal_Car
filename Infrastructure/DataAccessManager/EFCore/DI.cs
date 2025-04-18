using Application.Common.CQS.Commands;
using Application.Common.CQS.Queries;
using Application.Common.Repositories;
using Infrastructure.DataAccessManager.EFCore.Context;
using Infrastructure.DataAccessManager.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataAccessManager.EFCore;

public static class DI
{

	public static IServiceCollection RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		
		services.AddDbContext<DataContext>(options => 
		options.UseNpgsql(connectionString));

		services.AddScoped<ICommandContext, CommandContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
		services.AddScoped<IQueryContext, QueryContext>();

		return services;
	}
}
