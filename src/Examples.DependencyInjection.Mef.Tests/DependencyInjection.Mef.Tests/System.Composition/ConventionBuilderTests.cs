using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;
using Examples.DependencyInjection.Mef.Tests.Fixtures.Greeting;

namespace Examples.DependencyInjection.Mef.Tests.System.Composition;

/// <summary>
/// Tests to study dependency injection using MEF 2 (<see cref="System.Composition" />).
/// </summary>
public partial class ConventionBuilderTests
{
    [Fact]
    public void When_RegisteredMultipleServicesWithSameInterface_Then_CanInjectedIntoEnumerable()
    {
        var printer = new MockMessagePrinter();

        // Convention-Driven "lightweight " configuration.
        var conventions = new ConventionBuilder();

        // conventions
        //     .ForType<MyMessageGenerator1>()
        //     .Export<IMessageGenerator>()
        //     .Shared();
        // conventions
        //     .ForType<MyMessageGenerator2>()
        //     .Export<IMessageGenerator>()
        //     .Shared();
        // conventions
        //     .ForType<MyMessageGenerator3>()
        //     .Export<IMessageGenerator>()
        //     .Shared();
        conventions
            .ForTypesDerivedFrom<IMessageGenerator>()
            .ExportInterfaces()
            .Shared();

        conventions
            .ForType<GreetingService>()
            .Export<IGreetingService>();
        // conventions
        //     .ForTypesDerivedFrom<IGreetingService>()
        //     .ExportInterfaces();

        // Create ContainerConfiguration from assembly.
        var configuration = new ContainerConfiguration()
            .WithAssembly(Assembly.GetExecutingAssembly(), conventions);

        // Append mock instance.
        configuration.WithExport<IMessagePrinter>(printer);

        // Create container.
        using var container = configuration.CreateContainer();

        var service = container.GetExport<IGreetingService>();
        service!.Greet();

        var other = container.GetExport<IGreetingService>();
        other!.Greet();

        // The instance is also different because CreationPolicy.NonShared is specified.
        Assert.NotSame(service, other);

        var messages = printer.Messages.ToArray();
        Assert.Equal(6, messages.Length);
        Assert.Contains("Hello MEF2 DI world 1st.", messages[0]);
        Assert.Contains("Hello MEF2 DI world 2nd.", messages[1]);
        Assert.Contains("Hello MEF2 DI world 3rd.", messages[2]);
    }
}

file interface IMessageGenerator
{
    string Generate();
}

file class MyMessageGenerator1 : IMessageGenerator
{
    public string Generate() => "Hello MEF2 DI world 1st.";
}

file class MyMessageGenerator2 : IMessageGenerator
{
    public string Generate() => "Hello MEF2 DI world 2nd.";
}

file class MyMessageGenerator3 : IMessageGenerator
{
    public string Generate() => "Hello MEF2 DI world 3rd.";
}

file sealed class GreetingService(
    IMessagePrinter printer,
    IEnumerable<IMessageGenerator> generators) : IGreetingService
{
    private readonly IMessagePrinter _printer = printer;
    private readonly IEnumerable<IMessageGenerator> _generators = generators;

    public void Greet()
    {
        foreach (var greater in _generators)
        {
            _printer.Print(greater.Generate());
        }
    }
}