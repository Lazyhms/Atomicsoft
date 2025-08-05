﻿using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalEntityFrameworkCoreQueryableExtensions
{
    private static readonly EntityQueryRootExpressionVisitor _entityQueryRootExpressionVisitor = new();

    internal static readonly MethodInfo IgnoreNamedQueryFiltersMethodInfo
        = typeof(RelationalEntityFrameworkCoreQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(IgnoreQueryFilters))
            .Single(
                mi => mi.GetParameters().Any(
                    pi => pi.Name == "filterKeys" && pi.ParameterType == typeof(IEnumerable<string>)));

    public static IQueryable<TEntity> IgnoreQueryFilters<TEntity>(
        this IQueryable<TEntity> source,
        [NotParameterized] params string[] filterKeys) where TEntity : class
        => source.IgnoreQueryFilters((IEnumerable<string>)filterKeys);

    public static IQueryable<TEntity> IgnoreQueryFilters<TEntity>(
        this IQueryable<TEntity> source,
        [NotParameterized] IEnumerable<string> filterKeys) where TEntity : class
        => source.Provider is EntityQueryProvider
            ? source.Provider.CreateQuery<TEntity>(
                Expression.Call(
                    instance: null,
                    method: IgnoreNamedQueryFiltersMethodInfo.MakeGenericMethod(typeof(TEntity)),
                    arg0: source.Expression,
                    arg1: Expression.Constant(filterKeys)))
            : source;

    public static IQueryable<TEntity> IgnoreQueryFilters<TEntity, TProperty>(
        this IQueryable<TEntity> source,
        Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class
        => source.IgnoreQueryFilters(selector.GetMemberAccessList().Select(s => s.Name));

    /// <summary>
    /// Soft deletes database rows for the entity instances which match the LINQ query from the database.
    /// </summary>
    /// <param name="source">The source query.</param>
    /// <returns>The total number of rows deleted in the database.</returns>
    /// <exception cref="InvalidOperationException">
    /// This is usually because soft deletion is not enabled
    /// </exception>
    public static int ExecuteSoftDelete<TSource>(this IQueryable<TSource> source) where TSource : class
        => TryGetSoftDeleteProperty(source.Expression, out var columnName)
            ? source.ExecuteUpdate(setPropertyCalls =>
                setPropertyCalls.SetProperty(
                    property =>
                        EF.Property<bool>(property, columnName),
                    value => true))
            : throw new InvalidOperationException("Soft delete not enabled");

    /// <summary>
    /// Asynchronously soft deletes database rows for the entity instances which match the LINQ query from the database.
    /// </summary>
    /// <param name="source">The source query.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The total number of rows deleted in the database.</returns>
    /// <exception cref="InvalidOperationException">
    /// This is usually because soft deletion is not enabled
    /// </exception>
    public static Task<int> ExecuteSoftDeleteAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default) where TSource : class
        => TryGetSoftDeleteProperty(source.Expression, out var columnName)
            ? source.ExecuteUpdateAsync(setPropertyCalls =>
                setPropertyCalls.SetProperty(
                    property =>
                        EF.Property<bool>(property, columnName),
                    value => true),
                cancellationToken)
            : throw new InvalidOperationException("Soft delete not enabled");

    /// <summary>
    /// Deletes or soft deletes database rows for the entity instances which match the LINQ query from the database.
    /// </summary>
    /// <param name="source">The source query.</param>
    /// <returns>The total number of rows deleted in the database.</returns>
    public static int ExecuteDeleteOrSoftDelete<TSource>(this IQueryable<TSource> source) where TSource : class
        => TryGetSoftDeleteProperty(source.Expression, out var columnName)
            ? source.ExecuteUpdate(setPropertyCalls =>
                setPropertyCalls.SetProperty(
                    property =>
                        EF.Property<bool>(property, columnName),
                    value => true))
            : source.ExecuteDelete();

    /// <summary>
    /// Asynchronously deletes or soft deletes database rows for the entity instances which match the LINQ query from the database.
    /// </summary>
    /// <param name="source">The source query.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The total number of rows deleted in the database.</returns>
    public static Task<int> ExecuteDeleteOrSoftDeleteAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default) where TSource : class
        => TryGetSoftDeleteProperty(source.Expression, out var columnName)
            ? source.ExecuteUpdateAsync(setPropertyCalls =>
                setPropertyCalls.SetProperty(
                    property =>
                        EF.Property<bool>(property, columnName),
                    value => true),
                cancellationToken)
            : source.ExecuteDeleteAsync(cancellationToken);

    private static bool TryGetSoftDeleteProperty(Expression expression, out string columnName)
    {
        if (_entityQueryRootExpressionVisitor.Visit(expression) is EntityQueryRootExpression entityQueryRootExpression and not null
            && entityQueryRootExpression.EntityType.GetSoftDelete() is string propertyName and not null
            && entityQueryRootExpression.EntityType.GetProperty(propertyName) is IProperty softDeleteProperty and not null)
        {
            columnName = softDeleteProperty.Name;
            return true;
        }
        else
        {
            columnName = string.Empty;
            return false;
        }
    }

    private sealed class EntityQueryRootExpressionVisitor : ExpressionVisitor
    {
        [return: NotNullIfNotNull(nameof(node))]
        public override Expression? Visit(Expression? node) =>
            node is EntityQueryRootExpression ? node : base.Visit(node);

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Arguments.Count > 0)
            {
                var source = Visit(node.Arguments[0]);
                if (source is EntityQueryRootExpression)
                {
                    return source;
                }
            }

            return base.VisitMethodCall(node);
        }
    }
}
