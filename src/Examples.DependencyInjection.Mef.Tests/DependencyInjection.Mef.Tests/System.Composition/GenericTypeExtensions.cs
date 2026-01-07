namespace Examples.DependencyInjection.Mef.Tests.System.Composition;

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