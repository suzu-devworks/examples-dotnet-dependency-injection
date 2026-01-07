using Autofac;
using Examples.DependencyInjection.Autofac.Tests.Fixtures.Greeting;

namespace Examples.DependencyInjection.Autofac.Tests.Articles.ServiceLifetimes;

public partial class ServiceLifetimesTests
{
    [Fact]
    public void When_RegisteredAsInstancePerDependency_Then_ReturnsDifferentInstanceEachTime()
    {
        var printer = new MockMessagePrinter();

        var builder = new ContainerBuilder();

        builder.RegisterInstance(printer)
            .As<IMessagePrinter>()
            .ExternallyOwned();
        builder.RegisterType<HelloMessageGenerator>()
            .As<IMessageGenerator>()
            //.InstancePerDependency() // default
            ;
        builder.RegisterType<GreetingService>()
            .As<IGreetingService>()
            //.InstancePerDependency() // default
            ;

        using var container = builder.Build();

        var outScope = container.Resolve<IGreetingService>();
        outScope?.Greet();

        using (var scope = container.BeginLifetimeScope())
        {
            var inScope1 = scope.Resolve<IGreetingService>();
            inScope1?.Greet();

            Assert.NotSame(inScope1, outScope);

            var inScope2 = scope.Resolve<IGreetingService>();
            inScope2?.Greet();

            Assert.NotSame(inScope2, outScope);
            Assert.NotSame(inScope2, inScope1);
        }

        Assert.Equal(3, printer?.Messages.Count);
        Assert.Equal(3, printer?.Messages.Distinct().Count());
    }

    [Fact]
    public void When_RegisteredAsInstancePerLifetimeScope_Then_ReturnsSameInstanceWithinScope()
    {
        var printer = new MockMessagePrinter();

        var builder = new ContainerBuilder();

        builder.RegisterInstance(printer)
            .As<IMessagePrinter>()
            .ExternallyOwned();
        builder.RegisterType<HelloMessageGenerator>()
            .As<IMessageGenerator>()
            .InstancePerLifetimeScope();
        builder.RegisterType<GreetingService>()
            .As<IGreetingService>()
            .InstancePerLifetimeScope();

        using var container = builder.Build();

        var outScope = container.Resolve<IGreetingService>();
        outScope?.Greet();

        using (var scope = container.BeginLifetimeScope())
        {
            var inScope1 = scope.Resolve<IGreetingService>();
            inScope1?.Greet();

            Assert.NotSame(inScope1, outScope);

            var inScope2 = scope.Resolve<IGreetingService>();
            inScope2?.Greet();

            Assert.NotSame(inScope2, outScope);
            Assert.Same(inScope2, inScope1);
        }

        Assert.Equal(3, printer?.Messages.Count);
        Assert.Equal(2, printer?.Messages.Distinct().Count());
    }

    [Fact]
    public void When_RegisteredAsSingleInstance_Then_AlwaysReturnsSameInstance()
    {
        var printer = new MockMessagePrinter();

        var builder = new ContainerBuilder();

        builder.RegisterInstance(printer)
            .As<IMessagePrinter>()
            .ExternallyOwned();
        builder.RegisterType<HelloMessageGenerator>()
            .As<IMessageGenerator>()
            .SingleInstance();
        builder.RegisterType<GreetingService>()
            .As<IGreetingService>()
            .SingleInstance();

        using var container = builder.Build();

        var outScope = container.Resolve<IGreetingService>();
        outScope?.Greet();

        using (var scope = container.BeginLifetimeScope())
        {
            var inScope1 = scope.Resolve<IGreetingService>();
            inScope1?.Greet();

            Assert.Same(inScope1, outScope);

            var inScope2 = scope.Resolve<IGreetingService>();
            inScope2?.Greet();

            Assert.Same(inScope2, outScope);
            Assert.Same(inScope2, inScope1);
        }

        Assert.Equal(3, printer?.Messages.Count);
        Assert.Equal(1, printer?.Messages.Distinct().Count());
    }
}

file class HelloMessageGenerator : IMessageGenerator
{
    public string Generate() => $"Hello Autofac world in {GetHashCode()}";
}