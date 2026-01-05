namespace Examples.DependencyInjection.Extensions.Tests.Articles.HowToRegisterGenericTypes;

public static class GenericTypeExtensions
{
    public static string GetSimpleTypeName(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var genericTypes = string.Join(",", type.GenericTypeArguments.Select(x => x.Name));
        return $"{type.Name}[{genericTypes}]";
    }
}