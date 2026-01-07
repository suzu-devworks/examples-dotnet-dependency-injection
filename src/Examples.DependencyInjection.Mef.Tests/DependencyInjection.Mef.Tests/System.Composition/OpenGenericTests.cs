using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;

namespace Examples.DependencyInjection.Mef.Tests.System.Composition;

/// <summary>
/// Tests to study dependency injection using open generic classes.
/// </summary>
public class OpenGenericTests
{
    [Fact]
    public void WhenMultipleServicesRegisteredWithSameInterface_ReturnsAsIEnumerable()
    {
        var conventions = new ConventionBuilder();

        // register open generic types.
        conventions.ForTypesMatching(type =>
                type.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IProvider<>)))
            .Export(builder => builder.AsContractType(typeof(IProvider<>)))
            .SelectConstructor(ctors => ctors.First(c => c.GetParameters().Length == 1))
            .Shared();

        conventions.ForTypesMatching(type =>
                type.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IThing<,>)))
            .Export(builder => builder.AsContractType(typeof(IThing<,>)))
            .Shared();

        // Create ContainerConfiguration from assembly.
        var configuration = new ContainerConfiguration()
            .WithAssembly(Assembly.GetExecutingAssembly(), conventions);

        using var container = configuration.CreateContainer();

        // Gets primitive provider.
        var intProvider = container.GetExport<IProvider<int>>();
        var intResult = intProvider!.GetThing().GetName(Enumerable.Empty<int>());
        Assert.Equal("Type of Int32, IEnumerable`1[Int32]", intResult);

        // Gets string provider.
        var stringProvider = container.GetExport<IProvider<string>>();
        var stringResult = stringProvider!.GetThing().GetName(Enumerable.Empty<string>());
        Assert.Equal("Type of String, IEnumerable`1[String]", stringResult);

        // Gets struct provider.
        var dateTimeProvider = container.GetExport<IProvider<DateTime>>();
        var dateTimeResult = dateTimeProvider!.GetThing().GetName(Enumerable.Empty<DateTime>());
        Assert.Equal("Type of DateTime, IEnumerable`1[DateTime]", dateTimeResult);

        // Gets class provider.
        var eventArgsProvider = container.GetExport<IProvider<EventArgs>>();
        var eventArgsResult = eventArgsProvider!.GetThing().GetName(Enumerable.Empty<EventArgs>());
        Assert.Equal("Type of EventArgs, IEnumerable`1[EventArgs]", eventArgsResult);
    }
}


file interface IThing<T1, T2>
{
    string GetName(T2 element);
}

file interface IProvider<T>
{
    IThing<T, IEnumerable<T>> GetThing();
}

file class GenericThing<T1, T2>() : IThing<T1, T2>
{
    public string GetName(T2 element)
    {
        return $"Type of {typeof(T1).GetSimpleTypeName()}, {typeof(T2).GetSimpleTypeName()}";
    }
}

file class GenericProvider<T>(IThing<T, IEnumerable<T>> thing) : IProvider<T>
{
    public IThing<T, IEnumerable<T>> GetThing()
    {
        return thing;
    }
}