using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Extensions.Tests.Articles.HowToRegisterGenericTypes;

/// <summary>
/// How to Register Generic Types.
/// </summary>
/// <seealso href="https://www.stevejgordon.co.uk/asp-net-core-dependency-injection-how-to-register-generic-types" />
public class HowToRegisterGenericTypesTests
{
    [Fact]
    public void When_RegisteringOpenGenericType_Then_CanGetsClosedConstructedTypes()
    {
        var services = new ServiceCollection();

        // Register open generic types.
        services.AddSingleton(typeof(IThing<,>), typeof(GenericThing<,>));
        services.AddSingleton(typeof(IProvider<>), typeof(GenericProvider<>));

        using var provider = services.BuildServiceProvider();

        // Gets primitive provider.
        var intProvider = provider.GetService<IProvider<int>>();
        var intResult = intProvider!.GetThing().GetName(Enumerable.Empty<int>());
        Assert.Equal("Type of Int32, IEnumerable`1[Int32]", intResult);

        // Gets string provider.
        var stringProvider = provider.GetService<IProvider<string>>();
        var stringResult = stringProvider!.GetThing().GetName(Enumerable.Empty<string>());
        Assert.Equal("Type of String, IEnumerable`1[String]", stringResult);

        // Gets struct provider.
        var dateTimeProvider = provider.GetService<IProvider<DateTime>>();
        var dateTimeResult = dateTimeProvider!.GetThing().GetName(Enumerable.Empty<DateTime>());
        Assert.Equal("Type of DateTime, IEnumerable`1[DateTime]", dateTimeResult);

        // Gets class provider.
        var eventArgsProvider = provider.GetService<IProvider<EventArgs>>();
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