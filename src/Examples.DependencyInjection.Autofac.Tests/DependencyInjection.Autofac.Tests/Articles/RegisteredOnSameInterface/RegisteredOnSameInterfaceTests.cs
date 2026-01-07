using Autofac;
using Examples.DependencyInjection.Autofac.Tests.Fixtures.Greeting;

namespace Examples.DependencyInjection.Autofac.Tests.Articles.RegisteredOnSameInterface;

public class RegisteredOnSameInterfaceTests
{
    [Fact]
    public void When_RegisteredMultipleServicesWithSameInterface_Then_CanInjectedIntoEnumerable()
    {
        var printer = new MockMessagePrinter();

        var builder = new ContainerBuilder();

        builder.RegisterInstance(printer)
            .As<IMessagePrinter>()
            .ExternallyOwned();
        builder.RegisterTypes(
                typeof(MyMessageGenerator1),
                typeof(MyMessageGenerator2),
                typeof(MyMessageGenerator3))
            .As<IMessageGenerator>()
            .SingleInstance();
        builder
            .RegisterType<GreetingService>()
            .As<IGreetingService>()
            .SingleInstance();

        using var container = builder.Build();

        var instance = container.Resolve<IGreetingService>();
        instance?.Greet();

        var messages = printer.Messages.ToArray();
        Assert.Equal(3, messages.Length);
        Assert.Contains("Hello Autofac world 1st.", messages[0]);
        Assert.Contains("Hello Autofac world 2nd.", messages[1]);
        Assert.Contains("Hello Autofac world 3rd.", messages[2]);
    }

    [Fact]
    public void When_UsingGetServices_Then_ReturnsAllServices()
    {
        var builder = new ContainerBuilder();
        builder.RegisterTypes(
                typeof(MyMessageGenerator1),
                typeof(MyMessageGenerator2),
                typeof(MyMessageGenerator3))
            .As<IMessageGenerator>()
            .SingleInstance();

        using var container = builder.Build();

        var actual = container.Resolve<IEnumerable<IMessageGenerator>>();
        Assert.Equal(3, actual.Count());
    }

    [Fact]
    public void When_UsingGetService_Then_ReturnsLastRegisteredService()
    {
        var builder = new ContainerBuilder();
        builder.RegisterTypes(
                typeof(MyMessageGenerator1),
                typeof(MyMessageGenerator2),
                typeof(MyMessageGenerator3))
            .As<IMessageGenerator>()
            .SingleInstance();

        using var container = builder.Build();

        var actual = container.Resolve<IMessageGenerator>();

        Assert.Equal("Hello Autofac world 3rd.", actual?.Generate());
    }
}

file class MyMessageGenerator1 : IMessageGenerator
{
    public string Generate() => "Hello Autofac world 1st.";
}

file class MyMessageGenerator2 : IMessageGenerator
{
    public string Generate() => "Hello Autofac world 2nd.";
}

file class MyMessageGenerator3 : IMessageGenerator
{
    public string Generate() => "Hello Autofac world 3rd.";
}