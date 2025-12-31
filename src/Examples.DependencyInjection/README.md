# Examples.DependencyInjection

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Features](#features)
  - [Auto Service Registration Attribute](#auto-service-registration-attribute)

## Overview

This project is a utility I created to help you when using `Microsoft.Extensions.DependencyInjection`.

## Features

### Auto Service Registration Attribute

Since running AddService individually is tedious, I thought it might be useful to use reflection to automatically add classes with the attribute.

The problem is that the scope is fixed when you declare it.

I don't think this will be a problem for most patterns, but there are times when you want to have types with different scopes.

It's best to define the registration attribute within the assembly to guarantee that the assembly is loaded when it's registered.

```cs
using Examples.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MyServiceRegistrationAttribute(ServiceLifetime scope)
    : ServiceRegistrationAttribute(scope)
{
  public MyServiceRegistrationAttribute(): this(ServiceLifetime.Scoped)
  {
  }
}
```

If you specify something like this somewhere in the referenced assembly...

```cs
  [MyServiceRegistration]
  public class MyRepository
  {
   // ...
  }
```

This is registered with the service provider using the following code:

```cs
    var services = new ServiceCollection();    
    var provider = services
        .AddServiceWithAttributes<MyServiceRegistration>()
        .BuildServiceProvider();
```
