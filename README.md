# ForEvolve.DependencyInjection

![Build, Test, and Deploy master to feedz.io](https://github.com/ForEvolve/ForEvolve.DependencyInjection/workflows/Build,%20Test,%20and%20Deploy%20master%20to%20feedz.io/badge.svg)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fforevolve%2Fdependencyinjection%2Fshield%2FForEvolve.DependencyInjection.ContextualBindings%2Flatest)](https://f.feedz.io/forevolve/dependencyinjection/packages/ForEvolve.DependencyInjection.ContextualBindings/latest/download)

Asp.Net Core dependency injection utilities. This project aims at adding a few missing pieces to the Asp.Net Core container, without the need to replace it by a third-party.

The current plan is:

-   Contextual dependency injection
-   Automatic factory creation, based on interfaces

## Contextual Bindings

_This is an early work in progress, and the API will likely change over time._

The goal of this library is to add support for contextual dependency injection.
The extended goal would be to allow building complex object trees, conditionally, allowing for more flexibility than a single binding per service.

What is currently working:

-   Transient contextual injection
-   Scoped contextual injection
-   Singleton contextual injection

What is not working yet:

-   Contextual injection in MVC controllers
-   Contextual object trees building (using a service providers tree to manage the complexity)

_A few other mixed use-cases could also work, but use at your own risks!_

## AutoFactory

_The work on this project is not yet started_

The goal of this library is to generate factories automatically, based on an interface.
For example, the idea would be to implement the following interface automatically.

```csharp
public interface IFactory
{
    IService CreateService();
}
```

The initial implementation would be a simple service locator, that gets `IService` from the container. The design may change along the way to support more complex scenarios.

# How to use

Feel free to look at the tests and the sample projects to get an idea of the possibilities.

## Install

At some point there will be some packages; until then, here is the name they will have:

```PowerShell
dotnet add package ForEvolve.DependencyInjection.AutoFactory
dotnet add package ForEvolve.DependencyInjection.ContextualBindings
```

## Simple Contextual Bindings

You can inject contextual implementation into different classes, like:

```csharp
// Inject a LastNameGenerator instance into the INameGenerator parameter of FirstNameService
services
    .AddContextualBinding<IFirstNameService, FirstNameService>()
    .WithConstructorArgument<INameGenerator, FirstNameGenerator>()
;
// Inject a LastNameGenerator instance into the INameGenerator parameter of LastNameService
services
    .AddContextualBinding<ILastNameService, LastNameService>()
    .WithConstructorArgument<INameGenerator, LastNameGenerator>()
;
// Inject a FirstNameGenerator in the first INameGenerator parameter and
// inject a LastNameGenerator in the second INameGenerator parameter of FullNameGenerator.
services
    .AddContextualBinding<INameGenerator, FullNameGenerator>()
    .WithConstructorArgument<INameGenerator, FirstNameGenerator>()
    .WithConstructorArgument<INameGenerator, LastNameGenerator>()
;
```

# How to contribute?

If you would like to contribute to the project, first, thank you for your interest, and please read [Contributing to ForEvolve open source projects](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CONTRIBUTING.md) for more information.

## Contributor Covenant Code of Conduct

Also, please read the [Contributor Covenant Code of Conduct](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CODE_OF_CONDUCT.md) that applies to all ForEvolve repositories.
