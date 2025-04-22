using Application.Common.Mappings;
using Infrastructure.DataAccessManager.EFCore;
using Infrastructure.LogManager.Serilogs;
using Infrastructure.Mapper;
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

		// AutoMapper
		services.AddSingleton<IMapperService, MapperService>();

		return services;
	}
}
