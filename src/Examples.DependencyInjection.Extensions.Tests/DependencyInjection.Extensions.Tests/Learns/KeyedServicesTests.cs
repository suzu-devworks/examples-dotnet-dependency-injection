using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Extensions.Tests.Learns;

/// <summary>
/// Keyed Services Tests
/// </summary>
/// <remarks>
/// Service registration and lookup based on a key is supported, 
/// so you can register multiple services with different keys and use these keys for lookup.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection#keyed-services" />
public partial class KeyedServicesTests
{
    [Fact]
    public void When_RegisteringSameInterfaceWithKey_Then_CanLookUpByKeys()
    {
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IMessageWriter, MemoryMessageWriter>("memory");
        services.AddKeyedSingleton<IMessageWriter, QueueMessageWriter>("queue");

        var serviceProvider = services.BuildServiceProvider();

        var memoryWriter = serviceProvider.GetRequiredKeyedService<IMessageWriter>("memory");
        Assert.IsType<MemoryMessageWriter>(memoryWriter);

        var queueWriter = serviceProvider.GetRequiredKeyedService<IMessageWriter>("queue");
        Assert.IsType<QueueMessageWriter>(queueWriter);

        // If you do not specify a key, you will not be able to retrieve it.
        var writers = serviceProvider.GetServices<IMessageWriter>();
        Assert.Empty(writers);
    }

    [Fact]
    public void When_UseFromKeyedServicesAttributes_Then_CanUseKeyedServices()
    {
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IMessageWriter, MemoryMessageWriter>("memory");
        services.AddKeyedSingleton<IMessageWriter, QueueMessageWriter>("queue");
        services.AddSingleton<ExampleService>();

        var serviceProvider = services.BuildServiceProvider();

        // Via the FromKeyedServices attribute.
        var exampleService = serviceProvider.GetRequiredService<ExampleService>();
        Assert.IsType<QueueMessageWriter>(exampleService.Writer);
    }

    public class ExampleService(
        [FromKeyedServices("queue")] IMessageWriter writer)
    {
        public IMessageWriter Writer { get; } = writer;
    }

}