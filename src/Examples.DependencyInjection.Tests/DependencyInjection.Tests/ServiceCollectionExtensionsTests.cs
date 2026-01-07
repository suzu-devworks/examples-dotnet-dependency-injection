using Examples.DependencyInjection.Tests.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Tests;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void When_AddServiceWithAttributesWithAnyServices_Then_CanGetAutoRegisteredServices()
    {
        var services = new ServiceCollection();
        using var provider = services
            .AddServiceWithAttributes<TestSingletonServiceRegistrationAttribute>()
            .AddServiceWithAttributes<TestScopedServiceRegistrationAttribute>()
            .AddServiceWithAttributes<TestTransientServiceRegistrationAttribute>()
            .BuildServiceProvider();

        // Gets implement types
        Assert.IsType<DependsOnSingleton>(provider.GetService<DependsOnSingleton>());
        Assert.IsType<DependsOnScoped>(provider.GetService<DependsOnScoped>());
        Assert.IsType<DependsOnTransient>(provider.GetService<DependsOnTransient>());

        // If registered with the same interface, the last registered service is valid.
        Assert.IsType<DependsOnTransient>(provider.GetService<IDependency>());

        // You can get multiple services.
        Assert.Equal(3, provider.GetServices<IDependency>().Count());
        Assert.Single(provider.GetServices<IExtendsDependency>());
    }

    [Fact]
    public void When_AddServiceWithAttributesWithAnyServices_Then_CanGetWithinScopeSpecifiedAtRegistration()
    {
        var services = new ServiceCollection();
        using var provider = services
            .AddServiceWithAttributes<TestSingletonServiceRegistrationAttribute>()
            .AddServiceWithAttributes<TestScopedServiceRegistrationAttribute>()
            .AddServiceWithAttributes<TestTransientServiceRegistrationAttribute>()
            .BuildServiceProvider();

        var singleton = provider.GetService<DependsOnSingleton>();
        var scoped = provider.GetService<DependsOnScoped>();
        var transient = provider.GetService<DependsOnTransient>();

        using (var scope = provider.CreateScope())
        {
            // singleton 
            var singletonInScope1 = scope.ServiceProvider.GetService<DependsOnSingleton>();
            var singletonInScope2 = scope.ServiceProvider.GetService<DependsOnSingleton>();

            Assert.Same(singletonInScope1, singleton);
            Assert.Same(singletonInScope1, singletonInScope2);

            // scoped
            var scopedInScope1 = scope.ServiceProvider.GetService<DependsOnScoped>();
            var scopedInScope2 = scope.ServiceProvider.GetService<DependsOnScoped>();

            Assert.NotSame(scopedInScope1, scoped);
            Assert.Same(scopedInScope1, scopedInScope2);

            // transient
            var transientInScope1 = scope.ServiceProvider.GetService<DependsOnTransient>();
            var transientInScope2 = scope.ServiceProvider.GetService<DependsOnTransient>();

            Assert.NotSame(transientInScope1, transient);
            Assert.NotSame(transientInScope1, transientInScope2);
        }
    }
}