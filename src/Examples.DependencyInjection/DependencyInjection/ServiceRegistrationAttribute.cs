using System;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection;

/// <summary>
/// Attribute to mark services for registration in Dependency Injection.
/// </summary>
/// <param name="scope">The lifetime scope of the service.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class ServiceRegistrationAttribute(ServiceLifetime scope) : Attribute
{
    /// <summary>
    /// The lifetime scope of the service.
    /// </summary>
    public ServiceLifetime Scope { get; } = scope;

    /// <summary>
    /// The type of the service to register.
    /// </summary>
    public Type? ServiceType { get; init; }
}