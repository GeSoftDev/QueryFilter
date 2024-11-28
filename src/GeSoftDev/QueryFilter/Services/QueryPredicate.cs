using System.Linq.Expressions;
using System.Reflection;
using GeSoftDev.QueryFilter.Helpers;
using GeSoftDev.QueryFilter.Interfaces;

namespace GeSoftDev.QueryFilter.Services;

public class QueryPredicate<T> : IQueryPredicate<T>
{
    protected readonly ParameterExpression _parameter;
    public BinaryExpression? PredicateExpression { get; private set; }

    protected internal QueryPredicate(ParameterExpression parameter) => _parameter = parameter;

    public IQueryPredicate<T> Set(Expression<Func<T, bool>> predicate)
        => Set(true, predicate);
    public IQueryPredicate<T> Set(bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            PredicateExpression = GetBinaryExpression(predicate);
        }
        return this;
    }

    public IQueryPredicate<T> AndAll<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => All(ExpressionType.AndAlso, member, generatePredicates);
    public IQueryPredicate<T> AndAll<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => condition ? All(ExpressionType.AndAlso, member, generatePredicates) : this;
    public IQueryPredicate<T> OrAll<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => All(ExpressionType.OrElse, member, generatePredicates);
    public IQueryPredicate<T> OrAll<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => condition ? All(ExpressionType.OrElse, member, generatePredicates) : this;

    public IQueryPredicate<T> AndAny<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => Any(ExpressionType.AndAlso, member, generatePredicates);
    public IQueryPredicate<T> AndAny<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => condition ? Any(ExpressionType.AndAlso, member, generatePredicates) : this;
    public IQueryPredicate<T> OrAny<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => Any(ExpressionType.OrElse, member, generatePredicates);
    public IQueryPredicate<T> OrAny<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => condition ? Any(ExpressionType.OrElse, member, generatePredicates) : this;

    public IQueryPredicate<T> And(Expression<Func<T, bool>> predicate)
        => And(true, predicate);
    public IQueryPredicate<T> And(bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            var newPredicateExpression = GetBinaryExpression(predicate);
            PredicateExpression = PredicateExpression == null
                ? newPredicateExpression
                : Expression.AndAlso(PredicateExpression, newPredicateExpression);
        }
        return this;
    }

    public IQueryPredicate<T> Or(Expression<Func<T, bool>> predicate)
        => Or(true, predicate);
    public IQueryPredicate<T> Or(bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            var newPredicateExpression = GetBinaryExpression(predicate);
            PredicateExpression = PredicateExpression == null
                ? newPredicateExpression
                : Expression.OrElse(PredicateExpression, newPredicateExpression);
        }
        return this;
    }

    #region Private methods
    protected BinaryExpression GetBinaryExpression(Expression<Func<T, bool>> predicate)
    {
        var expression = UpdateParameter(predicate);
        if (expression is BinaryExpression binaryExpression)
        {
            return binaryExpression;
        }
        else if (expression is MethodCallExpression methodCallExpression)
        {
            if (
                methodCallExpression.Method.ReturnType != typeof(bool)
                && methodCallExpression.Method.ReturnType != typeof(bool?)
            )
            {
                throw new ArgumentException("The method must return a boolean value.", nameof(predicate));
            }
            return Expression.Equal(methodCallExpression, Expression.Constant(true));
        }
        else if (expression is MemberExpression memberExpression)
        {
            if (
                memberExpression.Type != typeof(bool)
                && memberExpression.Type != typeof(bool?)
            )
            {
                throw new ArgumentException("The member must return a boolean value.", nameof(predicate));
            }
            return Expression.Equal(memberExpression, Expression.Constant(true));
        }
        else
        {
            throw new InvalidOperationException("The expression must be a binary expression or a method call that returns a boolean value.");
        }
    }

    protected Expression UpdateParameter<TProp>(Expression<Func<T, TProp>> original)
        => new ExpressionParameterReplacer(original.Parameters[0], _parameter)
            .Visit(original.Body);

    protected static MethodInfo GetEnumerableMethod<TProp>(string name, int? parametersCount)
        => typeof(Enumerable).GetMethods()
            .FirstOrDefault(m => m.Name == name && (parametersCount == null || m.GetParameters().Length == parametersCount.Value))
            ?.MakeGenericMethod(typeof(TProp))
            ?? throw new InvalidOperationException($"Method '{name}' not found.");

    private static MethodCallExpression MethodCallExpression<TProp>(string methodName, MemberExpression property, ParameterExpression innerParameter, IQueryPredicate<TProp>[] predicates)
    {
        var predicatesExpressions = predicates
            ?.Where(c => c.PredicateExpression != null)
            ?.Select(c => c.PredicateExpression!)
            .ToArray();
        var hasPredicate = predicatesExpressions?.Length > 0;
        var parametersCount = hasPredicate ? 2 : 1;
        var method = GetEnumerableMethod<TProp>(methodName, parametersCount);
        return hasPredicate
            ? Expression.Call(method, property, Expression.Lambda(predicatesExpressions!.Aggregate(Expression.AndAlso), innerParameter))
            : Expression.Call(method, property);
    }

    private QueryPredicate<T> GenerateMethodPredicate<TProp>(ExpressionType type, string methodName, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
    {
        if (member.Body.NodeType != ExpressionType.MemberAccess)
        {
            throw new ArgumentException("The navigation property path must be a member access expression.", nameof(member));
        }
        var property = (MemberExpression)UpdateParameter(member);

        var innerType = typeof(TProp);
        var innerParameter = Expression.Parameter(innerType, property.Member.Name.ToLower());
        var predicates = generatePredicates.Select(generate => generate(new QueryPredicate<TProp>(innerParameter))).ToArray();
        var methodCall = MethodCallExpression(methodName, property, innerParameter, predicates);

        var methodExpression = Expression.Equal(methodCall, Expression.Constant(true));

        PredicateExpression = PredicateExpression == null ? methodExpression
            : Expression.MakeBinary(type, PredicateExpression, methodExpression);
        return this;
    }

    private QueryPredicate<T> All<TProp>(ExpressionType type, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => GenerateMethodPredicate(type, "All", member, generatePredicates);
    private QueryPredicate<T> Any<TProp>(ExpressionType type, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates)
        => GenerateMethodPredicate(type, "Any", member, generatePredicates);
    #endregion
}