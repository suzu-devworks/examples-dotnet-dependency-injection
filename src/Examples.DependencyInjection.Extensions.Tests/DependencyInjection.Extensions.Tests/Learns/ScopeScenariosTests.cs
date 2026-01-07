using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Extensions.Tests.Learns;

/// <summary>
/// Scope Scenarios Tests
/// </summary>
/// <remarks>
/// To achieve a scoped service within an implementation of BackgroundService, 
/// such as IHostedService, do not inject service dependencies via constructor injection. 
/// Instead, inject an IServiceScopeFactory, create a scope, and then resolve dependencies 
/// from that scope to use the appropriate service lifetime.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection#scope-scenarios"/>
public partial class ScopeScenariosTests
{
    [Fact]
    public async Task When_UsingIServiceScopeFactoryFromSingleton_Then_GetsTheServiceWithAppropriateScope()
    {
        var services = new ServiceCollection();
        services.AddScoped<IObjectStore, ObjectStore>();
        services.AddScoped<IObjectProcessor, ObjectProcessor>();
        services.AddScoped<IObjectRelay, ObjectRelay>();

        var provider = services.BuildServiceProvider();
        var serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
        Entry? previous = null;

        // while (!stoppingToken.IsCancellationRequested)
        foreach (var index in Enumerable.Range(0, 3))
        {
            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                var store = scope.ServiceProvider.GetRequiredService<IObjectStore>();
                var next = await store.GetNextAsync();
                // logger.LogInformation("{next}", next);
                Assert.NotNull(next);
                Assert.Equal("Entry: 0", next.Name);

                var processor = scope.ServiceProvider.GetRequiredService<IObjectProcessor>();
                await processor.ProcessAsync(next);
                // logger.LogInformation("Processing {name}.", next.Name);
                Assert.True(next.Processor == processor);
                Assert.False(previous?.Processor == processor);

                var relay = scope.ServiceProvider.GetRequiredService<IObjectRelay>();
                await relay.RelayAsync(next);
                // logger.LogInformation("Processed results have been relayed.");
                Assert.True(next.Relay == relay);
                Assert.False(previous?.Relay == relay);

                var marked = await store.MarkAsync(next);
                // logger.LogInformation("Marked as processed: {next}", marked);

                previous = next;
            }
        }
    }
}