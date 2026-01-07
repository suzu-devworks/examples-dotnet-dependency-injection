using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Reflection;
using Examples.DependencyInjection.Mef.Tests.Fixtures.Greeting;

namespace Examples.DependencyInjection.Mef.Tests.System.ComponentModel.Composition.Registration;

/// <summary>
/// Tests to study dependency injection using MEF 4.5 (<see cref="System.ComponentModel.Composition.Registration" />).
/// </summary>
public class RegistrationBuilderTests
{
    [Fact]
    public void When_RegisteredMultipleServicesWithSameInterface_Then_CanInjectedIntoEnumerable()
    {
        var printer = new MockMessagePrinter();

        // Convention-Driven configuration.
        var builder = new RegistrationBuilder();

        // builder.ForType* ... Define class or a set of classes to be operated on.
        //  .Export* ... Export classes .
        //  ... the other specifies the attributes, metadata and sharing policies to apply to
        //      the selected classes, properties of the classes or constructors of the classes.

        // builder
        //     .ForType<MyMessageGenerator1>()
        //     .Export<IMessageGenerator>()
        //     .SetCreationPolicy(CreationPolicy.Shared);
        // builder
        //     .ForType<MyMessageGenerator2>()
        //     .Export<IMessageGenerator>()
        //     .SetCreationPolicy(CreationPolicy.Shared);
        // builder
        //     .ForType<MyMessageGenerator3>()
        //     .Export<IMessageGenerator>()
        //     .SetCreationPolicy(CreationPolicy.Shared);
        builder
            .ForTypesDerivedFrom<IMessageGenerator>()
            .ExportInterfaces()
            .SetCreationPolicy(CreationPolicy.Shared);

        builder
            .ForType<GreetingService>()
            .Export<IGreetingService>()
            .SetCreationPolicy(CreationPolicy.NonShared);
        // builder
        //     .ForTypesDerivedFrom<IGreetingService>()
        //     .ExportInterfaces()
        //     .SetCreationPolicy(CreationPolicy.NonShared);

        // Create Aggregate.
        var aggregate = new AggregateCatalog();

        // Create AssemblyCatalog.
        var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly(), builder);
        aggregate.Catalogs.Add(assemblyCatalog);

        // // Create DirectoryCatalog for plugins.
        // var pluginDir = new DirectoryInfo("plugins");
        // if (!pluginDir.Exists) { pluginDir.Create(); }
        // var dirCatalog = new DirectoryCatalog(pluginDir.Name);
        // aggregate.Catalogs.Add(dirCatalog);

        // Create container.
        using var container = new CompositionContainer(aggregate);

        // Append mock instance.
        container.ComposeExportedValue<IMessagePrinter>(printer);

        var service = container.GetExportedValue<IGreetingService>();
        service?.Greet();

        var other = container.GetExportedValue<IGreetingService>();
        other?.Greet();

        // The instance is also different because CreationPolicy.NonShared is specified.
        Assert.NotSame(service, other);

        var messages = printer.Messages.ToArray();
        Assert.Equal(6, messages.Length);
        Assert.Contains("Hello MEF' DI world 1st.", messages[0]);
        Assert.Contains("Hello MEF' DI world 2nd.", messages[1]);
        Assert.Contains("Hello MEF' DI world 3rd.", messages[2]);
    }
}

file interface IMessageGenerator
{
    string Generate();
}

file class MyMessageGenerator1 : IMessageGenerator
{
    public string Generate() => "Hello MEF' DI world 1st.";
}

file class MyMessageGenerator2 : IMessageGenerator
{
    public string Generate() => "Hello MEF' DI world 2nd.";
}

file class MyMessageGenerator3 : IMessageGenerator
{
    public string Generate() => "Hello MEF' DI world 3rd.";
}

file interface IGreetingService
{
    void Greet();
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