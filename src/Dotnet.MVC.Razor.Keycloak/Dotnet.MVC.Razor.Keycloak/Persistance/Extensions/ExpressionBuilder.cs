using System.Linq.Expressions;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Extensions;

public static class ExpressionBuilder<T>
{
    public static Expression<Func<T, object>> GetPropertyLambda(string propertyName)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.Convert(
            Expression.PropertyOrField(param, propertyName),
            typeof(object));
        return Expression.Lambda<Func<T, object>>(body, param);
    }
}
