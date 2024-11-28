using GeSoftDev.QueryFilter.Interfaces;
using GeSoftDev.QueryFilter.Services;

namespace GeSoftDev.QueryFilter.Extensions;
public static class QueryFilterExtension
{
    /// <summary>
    /// Create a query filter for the specified model.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to filter.</typeparam>
    /// <param name="query">The IQueryable source to apply the filter.</param>
    /// <returns>An instance of IQueryFilter for the specified model.</returns>
    public static IQueryFilter<TModel> CreateFilter<TModel>(this IQueryable<TModel> query) => new QueryFilter<TModel>(query);
}