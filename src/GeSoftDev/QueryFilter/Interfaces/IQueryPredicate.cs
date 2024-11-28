using System.Linq.Expressions;

namespace GeSoftDev.QueryFilter.Interfaces;
public interface IQueryPredicate<T>
{
    /// <summary>
    /// Gets the binary expression that represents the current predicate.
    /// </summary>
    BinaryExpression? PredicateExpression { get; }

    /// <summary>
    /// Sets the predicate expression for the current instance using the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate expression to set.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> Set(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Sets the predicate expression for the current instance using the specified predicate.
    /// </summary>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="predicate">The predicate expression to set.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> Set(bool condition, Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical AND operation
    /// for each element in a collection property.
    /// Each element must satisfy all predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with AND logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> AndAll<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical AND operation
    /// for each element in a collection property.
    /// Each element must satisfy all predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with AND logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> AndAll<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical OR operation
    /// for each element in a collection property.
    /// At least one element must satisfy all predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with OR logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> OrAll<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical OR operation
    /// for each element in a collection property.
    /// At least one element must satisfy all predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with OR logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> OrAll<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical AND operation
    /// for at least one element in a collection property.
    /// At least one element must satisfy the predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with AND logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> AndAny<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical AND operation
    /// for at least one element in a collection property.
    /// At least one element must satisfy the predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with AND logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> AndAny<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical OR operation
    /// for at least one element in a collection property.
    /// At least one element must satisfy at least one of the predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with OR logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> OrAny<TProp>(Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical OR operation
    /// for at least one element in a collection property.
    /// At least one element must satisfy at least one of the predicates generated for the collection.
    /// </summary>
    /// <typeparam name="TProp">The type of the elements in the collection property.</typeparam>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="member">An expression that represents the collection property for which predicates are to be generated.</param>
    /// <param name="generatePredicates">A set of functions that each take an IQueryPredicate for a single element and return an updated IQueryPredicate, defining the predicates to be combined with OR logic for the collection elements.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> OrAny<TProp>(bool condition, Expression<Func<T, IEnumerable<TProp>>> member, params Func<IQueryPredicate<TProp>, IQueryPredicate<TProp>>[] generatePredicates);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical AND operation.
    /// Both predicates must be satisfied for the combined predicate to be true.
    /// </summary>
    /// <param name="predicate">The predicate expression to combine with the current predicate using AND logic.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> And(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical AND operation.
    /// Both predicates must be satisfied for the combined predicate to be true.
    /// </summary>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="predicate">The predicate expression to combine with the current predicate using AND logic.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> And(bool condition, Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical OR operation.
    /// At least one of the predicates must be satisfied for the combined predicate to be true.
    /// </summary>
    /// <param name="predicate">The predicate expression to combine with the current predicate using OR logic.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> Or(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Combines the current predicate with another predicate using a logical OR operation.
    /// At least one of the predicates must be satisfied for the combined predicate to be true.
    /// </summary>
    /// <param name="condition">A boolean indicating whether the predicate should be added to the filter.</param>
    /// <param name="predicate">The predicate expression to combine with the current predicate using OR logic.</param>
    /// <returns>The current instance with updated predicate logic applied.</returns>
    IQueryPredicate<T> Or(bool condition, Expression<Func<T, bool>> predicate);
}