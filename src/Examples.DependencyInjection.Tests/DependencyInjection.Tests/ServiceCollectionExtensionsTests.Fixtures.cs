using Examples.DependencyInjection.Tests.Attributes;

namespace Examples.DependencyInjection.Tests;

public partial class ServiceCollectionExtensionsTests
{
    public interface IDependency
    {
    }

    public interface IExtendsDependency : IDependency
    {
    }

    [TestSingletonServiceRegistration(ServiceType = typeof(IDependency))]
    [TestSingletonServiceRegistration(ServiceType = typeof(IExtendsDependency))]
    [TestSingletonServiceRegistration]
    public class DependsOnSingleton : IExtendsDependency
    {
    }

    [TestScopedServiceRegistration(ServiceType = typeof(IDependency))]
    [TestScopedServiceRegistration]
    public class DependsOnScoped : IDependency
    {
    }

    [TestTransientServiceRegistration(ServiceType = typeof(IDependency))]
    [TestTransientServiceRegistration]
    public class DependsOnTransient : IDependency
    {
    }

}