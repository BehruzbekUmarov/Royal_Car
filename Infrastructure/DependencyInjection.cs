using Infrastructure.DataAccessManager.EFCore;
using Infrastructure.LogManager.Serilogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		// DataAccess
		services.RegisterDataAccess(configuration);

		// Serilog
		services.RegisterSerilog(configuration);

		return services;
	}
}
