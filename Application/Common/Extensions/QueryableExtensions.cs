using Domain.Common;

namespace Application.Common.Extensions;

public static class QueryableExtensions
{
	public static IQueryable<T> ApplyIsDeletedFilter<T>(this IQueryable<T> query, bool isDleted = false) where T : class
	{
		if (typeof(IHasIsDeleted).IsAssignableFrom(typeof(T)))
		{
			query = query.Where(x => (x as IHasIsDeleted)!.IsDeleted == isDleted);
		}

		return query;
	}
}
