using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Tests.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TestScopedServiceRegistrationAttribute()
    : ServiceRegistrationAttribute(ServiceLifetime.Scoped)
{

}