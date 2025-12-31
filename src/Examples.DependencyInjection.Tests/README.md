# Examples.DependencyInjection.Tests

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This is a test project for [Examples.DependencyInjection](../Examples.DependencyInjection/README.md).

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.DependencyInjection
dotnet new classlib -o src/Examples.DependencyInjection
dotnet sln add src/Examples.DependencyInjection/
cd src/Examples.DependencyInjection
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Abstractions
cd ../../

## Examples.DependencyInjection.Tests
dotnet new xunit3 -o src/Examples.DependencyInjection.Tests
dotnet sln add src/Examples.DependencyInjection.Tests/
cd src/Examples.DependencyInjection.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.v3
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
cd ../../

# Update outdated package
dotnet list package --outdated
```
