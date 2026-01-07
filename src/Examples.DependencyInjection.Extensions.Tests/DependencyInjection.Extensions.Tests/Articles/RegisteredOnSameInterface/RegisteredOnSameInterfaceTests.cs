using Examples.DependencyInjection.Extensions.Tests.Fixtures.Greeting;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Extensions.Tests.Articles.RegisteredOnSameInterface;

public class RegisteredOnSameInterfaceTests
{
    [Fact]
    public void When_RegisteredMultipleServicesWithSameInterface_Then_CanInjectedIntoEnumerable()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMessagePrinter, MockMessagePrinter>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator1>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator2>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator3>();
        services.AddSingleton<IGreetingService, GreetingService>();

        using var provider = services.BuildServiceProvider();

        var service = provider.GetService<IGreetingService>();
        service?.Greet();

        var printer = (MockMessagePrinter?)provider.GetService<IMessagePrinter>();
        Assert.NotNull(printer);
        Assert.Equal(3, printer!.Messages.Count);
    }

    [Fact]
    public void When_UsingGetServices_Then_ReturnsAllServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator1>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator2>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator3>();

        using var provider = services.BuildServiceProvider();

        var actual = provider.GetServices<IMessageGenerator>();

        Assert.Equal(3, actual.Count());
    }

    [Fact]
    public void When_UsingGetService_Then_ReturnsLastRegisteredService()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator1>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator2>();
        services.AddSingleton<IMessageGenerator, MyMessageGenerator3>();

        using var provider = services.BuildServiceProvider();

        var actual = provider.GetRequiredService<IMessageGenerator>();

        Assert.Equal("Hello DI world 3rd.", actual?.Generate());
    }


}

file class MyMessageGenerator1 : IMessageGenerator
{
    public string Generate() => "Hello DI world 1st.";
}

file class MyMessageGenerator2 : IMessageGenerator
{
    public string Generate() => "Hello DI world 2nd.";
}

file class MyMessageGenerator3 : IMessageGenerator
{
    public string Generate() => "Hello DI world 3rd.";
}