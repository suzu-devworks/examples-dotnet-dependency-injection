using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Examples.DependencyInjection.Mef.Tests.Fixtures.Greeting;

namespace Examples.DependencyInjection.Mef.Tests.System.ComponentModel.Composition;

/// <summary>
/// Tests to study dependency injection using MEF 4.0 (<see cref="System.ComponentModel.Composition" />).
/// </summary>
public class CompositionContainerTests
{
    [Fact]
    public void When_RegisteredMultipleServicesWithSameInterface_Then_CanInjectedIntoEnumerable()
    {
        var printer = new MockMessagePrinter();

        // Create AggregateCatalog.
        var aggregate = new AggregateCatalog();

        // Create AssemblyCatalog.
        var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
        aggregate.Catalogs.Add(assemblyCatalog);

        // Create DirectoryCatalog for plugins.
        var pluginDir = new DirectoryInfo("plugins");
        if (!pluginDir.Exists) { pluginDir.Create(); }
        var dirCatalog = new DirectoryCatalog(pluginDir.Name);
        aggregate.Catalogs.Add(dirCatalog);

        using var container = new CompositionContainer(aggregate);
        container.ComposeParts(this);

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
        Assert.Contains("Hello MEF DI world 1st.", messages[0]);
        Assert.Contains("Hello MEF DI world 2nd.", messages[1]);
        Assert.Contains("Hello MEF DI world 3rd.", messages[2]);
    }
}

[Export(typeof(IMessageGenerator))]
[PartCreationPolicy(CreationPolicy.Shared)]
file class MyMessageGenerator1 : IMessageGenerator
{
    public string Generate() => "Hello MEF DI world 1st.";
}

[Export(typeof(IMessageGenerator))]
[PartCreationPolicy(CreationPolicy.NonShared)]
file class MyMessageGenerator2 : IMessageGenerator
{
    public string Generate() => "Hello MEF DI world 2nd.";
}

[Export(typeof(IMessageGenerator))]
[PartCreationPolicy(CreationPolicy.Any)]
file class MyMessageGenerator3 : IMessageGenerator
{
    public string Generate() => "Hello MEF DI world 3rd.";
}

[Export(typeof(IGreetingService))]
[PartCreationPolicy(CreationPolicy.NonShared)]
[method: ImportingConstructor]
file class MyService(
    [Import] IMessagePrinter messagePrinter,
    [ImportMany] IEnumerable<IMessageGenerator> messageGenerators) : IGreetingService
{
    private readonly IMessagePrinter _messagePrinter = messagePrinter;
    private readonly IEnumerable<IMessageGenerator> _messageGenerators = messageGenerators;

    public void Greet()
    {
        foreach (var greater in _messageGenerators)
        {
            _messagePrinter.Print(greater.Generate());
        }
    }
}