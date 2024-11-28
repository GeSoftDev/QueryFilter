using System.Linq.Expressions;

namespace GeSoftDev.QueryFilter.Interfaces;

public interface IQueryFilter<T>
{
    /// <summary>
    /// Adds a new query predicate to the filter.
    /// </summary>
    /// <returns>An instance of <see cref="IQueryPredicate{T}"/> representing the added predicate.</returns>
    IQueryPredicate<T> Add();

    /// <summary>
    /// Adds a new predicate to the query filter with a specified condition.
    /// </summary>
    /// <param name="predicate">An expression representing the condition of the predicate.</param>
    /// <returns>An instance of <see cref="IQueryPredicate{T}"/> representing the added predicate.</returns>
    IQueryPredicate<T> Add(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Adds a new predicate to the query filter with a specified condition.
    /// </summary>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="predicate">An expression representing the condition of the predicate.</param>
    /// <returns>An instance of <see cref="IQueryPredicate{T}"/> representing the added predicate.</returns>
    IQueryPredicate<T> Add(bool condition, Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves the constructed query based on the added predicates.
    /// </summary>
    /// <returns>An instance of <see cref="IQueryable{T}"/> that represents the query.</returns>
    IQueryable<T> GetQuery();
}