namespace System.Linq;

public static partial class QueryableExtensions
{
    public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> expression)
        => condition ? source.Where(expression) : source;

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, string condition, Expression<Func<TSource, bool>> expression)
        => condition.IsNotNullOrWhiteSpace() ? source.Where(expression) : source;

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, string condition, Expression<Func<TSource, int, bool>> expression)
        => condition.IsNotNullOrWhiteSpace() ? source.Where(expression) : source;

    public static IQueryable<TSource> WhereIf<TSource, TProperty>(this IQueryable<TSource> source, TProperty? condition, Expression<Func<TSource, bool>> expression) where TProperty : struct
        => condition.HasValue ? source.Where(expression) : source;
}
