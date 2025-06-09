namespace Dotnet.MVC.Razor.Keycloak.Persistance.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string property)
        => source.OrderBy(ExpressionBuilder<T>.GetPropertyLambda(property));

    public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string property)
        => source.OrderByDescending(ExpressionBuilder<T>.GetPropertyLambda(property));
}
