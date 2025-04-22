using Application.Common.Mappings;
using AutoMapper;

namespace Infrastructure.Mapper;

public class MapperService(IMapper mapper) : IMapperService
{
	public TDestination Map<TDestination>(object source)
	{
		return mapper.Map<TDestination>(source);
	}

	public void Map<TDestination>(object source, TDestination destination)
	{
		mapper.Map(source, destination);
	}
}
