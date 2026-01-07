# Examples.DependencyInjection.Autofac.Tests

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [What is Autofac?](#what-is-autofac)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This is a project for learning Ioc using `Autofac`

## What is Autofac?

Autofac is an addictive Inversion of Control container for .NET Core, ASP.NET Core, .NET 4.5.1+, Universal Windows apps, and more.

- <https://autofac.org/>

> for .NET Core, .NET Framework 4.5.1+

```shell
dotnet add package Autofac
```

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection.Autofac.Tests
dotnet new xunit3 -o src/Examples.DependencyInjection.Autofac.Tests
dotnet sln add src/Examples.DependencyInjection.Autofac.Tests/
cd src/Examples.DependencyInjection.Autofac.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Autofac
cd ../../

# Update outdated package
dotnet list package --outdated
```
