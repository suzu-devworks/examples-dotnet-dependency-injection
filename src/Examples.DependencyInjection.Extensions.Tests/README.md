# Examples.DependencyInjection.Extensions.Tests

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [What is Microsoft.Extensions.DependencyInjection?](#what-is-microsoftextensionsdependencyinjection)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This is a project for learning Ioc using `Microsoft.Extensions.DependencyInjection`

## What is Microsoft.Extensions.DependencyInjection?

Provides classes that support the implementation of the dependency injection software design pattern.

- [.NET dependency injection - Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection)

> Since .NET Platform Extensions 1.0

```shell
dotnet add package Microsoft.Extensions.DependencyInjection
```

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection.Extensions.Tests
dotnet new xunit3 -o src/Examples.DependencyInjection.Extensions.Tests
dotnet sln add src/Examples.DependencyInjection.Extensions.Tests/
cd src/Examples.DependencyInjection.Extensions.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Debug
cd ../../

# Update outdated package
dotnet list package --outdated
```
