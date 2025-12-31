using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Tests.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TestTransientServiceRegistrationAttribute()
    : ServiceRegistrationAttribute(ServiceLifetime.Transient)
{

}