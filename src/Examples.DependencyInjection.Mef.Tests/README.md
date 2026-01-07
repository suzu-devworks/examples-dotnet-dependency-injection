# Examples.DependencyInjection.Mef.Tests

This is a project for learning Ioc using The Managed Extensibility Framework.

## Table of Contents <!-- omit in toc -->

- [What is MEF?](#what-is-mef)
- [Attributed programming model](#attributed-programming-model)
- [A convention-based extension model](#a-convention-based-extension-model)
- [MEF2 (System.Composition)](#mef2-systemcomposition)
- [References](#references)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## What is MEF?

The Managed Extensibility Framework (MEF) is a library for creating lightweight and extensible applications. It allows application developers to discover and use extensions with no configuration required. It also lets extension developers easily encapsulate code and avoid fragile hard dependencies. MEF not only allows extensions to be reused within applications, but across applications as well.

- <https://learn.microsoft.com/ja-jp/dotnet/framework/mef/>

## Attributed programming model

The default programming model used in MEF is the attributed programming model. In the attributed programming model parts, imports, exports, and other objects are defined with attributes that decorate ordinary .NET Framework classes.

- [Attributed programming model overview (MEF)](https://learn.microsoft.com/ja-jp/dotnet/framework/mef/attributed-programming-model-overview-mef)

> Since .NET Framework 4.0

The basic concepts of MEF:

- [Parts, catalogs, and the composition container](<https://learn.microsoft.com/en-us/dotnet/framework/mef/#composition-container-and-catalogs>)
- [Imports and exports](<https://learn.microsoft.com/en-us/dotnet/framework/mef/#imports-and-exports-with-attributes>)

Typically called just MEF.

```shell
dotnet add package System.ComponentModel.Composition
```

## A convention-based extension model

The .NET Framework 4.5 provides a way to centralize configuration so that a set of rules can be written on how extension points and components are created and composed

- [CLR - An Attribute-Free Approach to Configuring MEF - Microsoft Learn](https://docs.microsoft.com/ja-jp/archive/msdn-magazine/2012/june/clr-an-attribute-free-approach-to-configuring-mef)

This namespace provides classes that constitute the core of the Managed Extensibility Framework, or MEF.

> Since .NET Framework 4.5

Support for:

- Attribute-less registration ([RegistrationBuilder](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition.registration.registrationbuilder))
- Finer-Grained Control Over Lifetime ([ExportFactory&lt;T&gt;](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition.exportfactory-1))
- Diagnostics improvements ([CompositionOptions.DisableSilentRejection](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition.hosting.compositionoptions))

Sometimes called MEF2 or MEF.

```shell
dotnet add package System.ComponentModel.Composition.Registration
```

## MEF2 (System.Composition)

This packages provides a version of the Managed Extensibility Framework (MEF) that is lightweight and specifically optimized for high throughput scenarios, such as the web.

> Since .NET Framework 4.5

Key Features:

- Components are discovered and composed using attributes.
- Provides dependency injection capabilities for loosely coupled modules.

Lightweight version of MEF typically called MEF2.

```shell
dotnet add package System.Composition
```

## References

- [System.ComponentModel.Composition Namespace - Microsoft Learn](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition)
- [System.Composition Namespace - Microsoft Learn](https://docs.microsoft.com/ja-jp/dotnet/api/system.composition)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection.Mef.Tests
dotnet new xunit3 -o src/Examples.DependencyInjection.Mef.Tests
dotnet sln add src/Examples.DependencyInjection.Mef.Tests/
cd src/Examples.DependencyInjection.Mef.Tests
dotnet add package NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package System.ComponentModel.Composition
dotnet add package System.ComponentModel.Composition.Registration
dotnet add package System.Composition
cd ../../

# Update outdated package
dotnet list package --outdated
```
