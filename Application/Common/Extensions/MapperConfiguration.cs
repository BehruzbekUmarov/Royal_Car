using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions;

public static class MapperConfiguration
{
	public static void RegisterMappingProfiles(this IServiceCollection services)
	{
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();

		var profiles = assemblies
			.SelectMany(a => a.GetTypes())
			.Where(c => c is { IsClass: true, IsAbstract: false } && c.IsSubclassOf(typeof(Profile)));

		services.AddAutoMapper(profiles.ToArray());
	}
}
