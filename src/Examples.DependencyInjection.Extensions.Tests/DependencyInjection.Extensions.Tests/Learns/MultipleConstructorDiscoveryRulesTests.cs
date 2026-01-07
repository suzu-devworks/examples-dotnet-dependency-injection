using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.DependencyInjection.Extensions.Tests.Learns;

/// <summary>
/// Multiple constructor discovery rules.
/// </summary>
/// <remarks>
/// When a type defines more than one constructor, the service provider has logic 
/// for determining which constructor to use.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection#multiple-constructor-discovery-rules"/>
public partial class MultipleConstructorDiscoveryRulesTests
{
    [Fact]
    public void When_ConstructorHasUnResolvedParameters_Then_UnresolvedConstructorNotUsed()
    {
        var services = new ServiceCollection()
            .AddLogging(logger => logger.AddDebug());
        services.AddSingleton<FooService>();
        services.AddSingleton<UnresolvableParameterService>();

        using (var provider = services.BuildServiceProvider())
        {
            // Use T(ILogger<T> logger) constructor.
            var instance = provider.GetService<UnresolvableParameterService>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.Logger);
            Assert.Null(instance.Foo);
            Assert.Null(instance.Bar);
        }

        services.AddSingleton<BarService>();
        using (var provider = services.BuildServiceProvider())
        {
            // If there is another constructor that can be resolved, an exception will be thrown.
            var ex = Assert.Throws<InvalidOperationException>(() =>
                       provider.GetService<UnresolvableParameterService>());
            Assert.Contains("The following constructors are ambiguous:", ex.Message);
        }
    }

    [Fact]
    public void When_ConstructorHasAmbiguousParameters_Then_ThrowsException()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();
        services.AddSingleton<AmbiguousParametersService>();

        using var provider = services.BuildServiceProvider();

        // It appears that `ActivatorUtilitiesConstructor` is not used in ServiceProvider.
        var ex = Assert.Throws<InvalidOperationException>(
            () => provider.GetService<AmbiguousParametersService>());
        Assert.Contains("The following constructors are ambiguous:", ex.Message);
    }

    [Fact]
    public void When_ConstructorHasAmbiguousParametersUseImplementationFactory_Then_ReturnsExpectedInstance()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();
        services.AddSingleton(provider =>
            new AmbiguousParametersService(provider.GetRequiredService<ILogger<AmbiguousParametersService>>()));

        using var provider = services.BuildServiceProvider();

        var instance = provider.GetService<AmbiguousParametersService>();
        Assert.NotNull(instance!.Logger);   // ILogger<T> is resolved
        Assert.Null(instance.Options);      // IOptions<T> is not specified
    }

    [Fact]
    public void When_ConstructorHasAmbiguousParametersUseAttribute_Then_ReturnsExpectedInstance()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();
        services.AddSingleton<AmbiguousParametersService>();

        using var provider = services.BuildServiceProvider();

        var instance = ActivatorUtilities.CreateInstance<AmbiguousParametersService>(provider);
        Assert.Null(instance!.Logger);          // ILogger<T> is not specified
        Assert.NotNull(instance.Options);       // IOptions<T> is resolved
    }

    [Fact]
    public void When_ConstructorDisambiguates_Then_UseThatConstructor()
    {
        var services = new ServiceCollection()
            .AddLogging()
            .AddOptions();
        services.AddSingleton<DisambiguatesParametersService>();

        using var provider = services.BuildServiceProvider();

        var instance = provider.GetService<DisambiguatesParametersService>();
        Assert.NotNull(instance!.Logger);   // ILogger<T> is resolved
        Assert.NotNull(instance.Options);   // IOptions<T> is resolved
    }

}