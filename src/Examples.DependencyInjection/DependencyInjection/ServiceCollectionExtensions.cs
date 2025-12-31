using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection;

/// <summary>
/// Extension for Dependency Injection service collection.
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services marked with the specified attribute to the service collection.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServiceWithAttributes<TAttribute>(this IServiceCollection services)
        where TAttribute : ServiceRegistrationAttribute
    {
        foreach (var (serviceType, implementType, scope) in EnumerateServiceTypes<TAttribute>())
        {
            services.Add(new ServiceDescriptor(serviceType, implementType, scope));
        }

        return services;
    }

    private static IEnumerable<(Type, Type, ServiceLifetime)> EnumerateServiceTypes<TAttribute>()
        where TAttribute : ServiceRegistrationAttribute
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var tuples = assembly.GetTypes()
                .SelectMany(t => t.GetCustomAttributes<TAttribute>()
                    .Select(a => (Attribute: a, ImplementType: t)))
                .Select(x => (x.Attribute.ServiceType ?? x.ImplementType, x.ImplementType, x.Attribute.Scope));

            foreach (var tuple in tuples)
            {
                yield return tuple;
            }
        }
    }

}