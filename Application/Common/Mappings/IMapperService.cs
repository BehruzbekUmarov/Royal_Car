namespace Application.Common.Mappings;

public interface IMapperService
{
	TDestination Map<TDestination>(object source);
	void Map<TDestination>(object source ,TDestination destination);
}
