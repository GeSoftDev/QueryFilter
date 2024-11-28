using System.Linq.Expressions;
using GeSoftDev.QueryFilter.Interfaces;

namespace GeSoftDev.QueryFilter.Services;

public class QueryFilter<T> : IQueryFilter<T>
{
    protected readonly IQueryable<T> _query;
    protected readonly ParameterExpression _parameter = Expression.Parameter(typeof(T), typeof(T).Name.ToLower());
    protected readonly List<IQueryPredicate<T>> _predicates = [];

    protected internal QueryFilter(IQueryable<T> query) => _query = query;

    public IQueryPredicate<T> Add()
    {
        var predicate = new QueryPredicate<T>(_parameter);
        _predicates.Add(predicate);
        return predicate;
    }

    public IQueryPredicate<T> Add(Expression<Func<T, bool>> predicate)
        => Add().Set(predicate);
    public IQueryPredicate<T> Add(bool condition, Expression<Func<T, bool>> predicate)
        => Add().Set(condition, predicate);

    public IQueryable<T> GetQuery()
    {
        var expression = GetFilterExpression();
        return expression == null ? _query
            : _query.Where(Expression.Lambda<Func<T, bool>>(expression, _parameter));
    }

    protected BinaryExpression? GetFilterExpression()
        => _predicates.Count switch
        {
            0 => null,
            1 => _predicates[0].PredicateExpression,
            _ => _predicates
                .Where(c => c.PredicateExpression != null)
                .Select(c => c.PredicateExpression!)
                .Aggregate(Expression.AndAlso)
        };
}