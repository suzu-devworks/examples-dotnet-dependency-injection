using Examples.DependencyInjection.Extensions.Tests.Fixtures.Greeting;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Extensions.Tests.Articles.ServiceLifetimes;

public class ServiceLifetimesTests
{
    [Fact]
    public void When_RegisteredAsTransient_Then_ReturnsDifferentInstanceEachTime()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter, MockMessagePrinter>();
        services.AddTransient<IMessageGenerator, HelloMessageGenerator>();
        services.AddTransient<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();

        var outScope = provider.GetService<IGreetingService>();
        outScope?.Greet();

        using (var scope = provider.CreateScope())
        {
            var inScope1 = scope.ServiceProvider.GetService<IGreetingService>();
            inScope1?.Greet();

            Assert.NotSame(inScope1, outScope);

            var inScope2 = scope.ServiceProvider.GetService<IGreetingService>();
            inScope2?.Greet();

            Assert.NotSame(inScope2, outScope);
            Assert.NotSame(inScope2, inScope1);
        }

        var printer = (MockMessagePrinter?)provider.GetService<IMessagePrinter>();
        Assert.Equal(3, printer?.Messages.Count);
        Assert.Equal(3, printer?.Messages.Distinct().Count());
    }

    [Fact]
    public void When_RegisteredAsScoped_Then_ReturnsSameInstanceWithinScope()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter, MockMessagePrinter>();
        services.AddScoped<IMessageGenerator, HelloMessageGenerator>();
        services.AddScoped<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();

        var outScope = provider.GetService<IGreetingService>();
        outScope?.Greet();

        using (var scope = provider.CreateScope())
        {
            var inScope1 = scope.ServiceProvider.GetService<IGreetingService>();
            inScope1?.Greet();

            Assert.NotSame(inScope1, outScope);

            var inScope2 = scope.ServiceProvider.GetService<IGreetingService>();
            inScope2?.Greet();

            Assert.NotSame(inScope2, outScope);
            Assert.Same(inScope2, inScope1);
        }

        var printer = (MockMessagePrinter?)provider.GetService<IMessagePrinter>();
        Assert.Equal(3, printer?.Messages.Count);
        Assert.Equal(2, printer?.Messages.Distinct().Count());
    }

    [Fact]
    public void When_RegisteredAsSingleton_Then_AlwaysReturnsSameInstance()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter, MockMessagePrinter>();
        services.AddSingleton<IMessageGenerator, HelloMessageGenerator>();
        services.AddSingleton<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();


        var outScope = provider.GetService<IGreetingService>();
        outScope?.Greet();

        using (var scope = provider.CreateScope())
        {
            var inScope1 = scope.ServiceProvider.GetService<IGreetingService>();
            inScope1?.Greet();

            Assert.Same(inScope1, outScope);

            var inScope2 = scope.ServiceProvider.GetService<IGreetingService>();
            inScope2?.Greet();

            Assert.Same(inScope2, outScope);
            Assert.Same(inScope2, inScope1);
        }

        var printer = (MockMessagePrinter?)provider.GetService<IMessagePrinter>();
        Assert.Equal(3, printer?.Messages.Count);
        Assert.Equal(1, printer?.Messages.Distinct().Count());
    }

}

file class HelloMessageGenerator : IMessageGenerator
{
    public string Generate() => $"Hello Ioc world in {GetHashCode()}.";
}