# ForEvolve DependencyInjection

![Build, Test, and Deploy master to feedz.io](https://github.com/ForEvolve/ForEvolve.DependencyInjection/workflows/Build,%20Test,%20and%20Deploy%20master%20to%20feedz.io/badge.svg)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fforevolve%2Fdependencyinjection%2Fshield%2FForEvolve.DependencyInjection.ContextualBindings%2Flatest)](https://f.feedz.io/forevolve/dependencyinjection/packages/ForEvolve.DependencyInjection.ContextualBindings/latest/download)
[![NuGet.org](https://img.shields.io/nuget/vpre/ForEvolve.DependencyInjection.ContextualBindings)](https://www.nuget.org/packages/ForEvolve.DependencyInjection.ContextualBindings/)

Asp.Net Core dependency injection utilities. This project aims at adding a few missing pieces to the Asp.Net Core container, without the need to replace it by a third-party.

**What is supported:**

-   Contextual dependency injection
-   Module-based dependency registration with auto-discovery

**What I'd like to add:**

-   Automatic factory creation, based on interfaces

# Bundles

The following packages are bundling multiple packages as convenience.

## ForEvolve.DependencyInjection

Bundles containing the following packages:

-   `ForEvolve.DependencyInjection.ContextualBindings`
-   `ForEvolve.DependencyInjection.Modules`

### Install

```bash
dotnet add package ForEvolve.DependencyInjection
```

## ForEvolve.DependencyInjection.AspNetCore

Bundles containing the following packages:

-   `ForEvolve.DependencyInjection`
-   `ForEvolve.DependencyInjection.ContextualBindings.AspNetCore`

### Install

```bash
dotnet add package ForEvolve.DependencyInjection.AspNetCore
```

# Individual packages

## ForEvolve.DependencyInjection.ContextualBindings

The goal of the `ForEvolve.DependencyInjection.ContextualBindings` library is to add support for contextual dependency injection to any application.

What is currently working:

-   Transient contextual injection
-   Scoped contextual injection
-   Singleton contextual injection
-   Contextual injection in MVC controllers
-   Contextual object trees building

### Install

```bash
dotnet add package ForEvolve.DependencyInjection.ContextualBindings
# OR
dotnet add package ForEvolve.DependencyInjection.ContextualBindings.AspNetCore
```

### How to use

You can create simple contextual bindings like:

```csharp
// Inject a LastNameGenerator instance into the INameGenerator parameter of FirstNameService
services
    .AddContextualBinding<IFirstNameService, FirstNameService>(d => d
        .WithConstructorArgument<INameGenerator, FirstNameGenerator>())
;
// Inject a LastNameGenerator instance into the INameGenerator parameter of LastNameService
services
    .AddContextualBinding<ILastNameService, LastNameService>(d => d
        .WithConstructorArgument<INameGenerator, LastNameGenerator>())
;
// Inject a FirstNameGenerator in the first INameGenerator parameter and
// inject a LastNameGenerator in the second INameGenerator parameter of FullNameGenerator.
services
    .AddContextualBinding<INameGenerator, FullNameGenerator>(d => d
        .WithConstructorArgument<INameGenerator, FirstNameGenerator>()
        .WithConstructorArgument<INameGenerator, LastNameGenerator>())
;
```

Or go for more complex object trees like this:

```csharp
services
    .AddContextualBinding<IComplexObjectTreeService, ComplexObjectTreeService>(d =>
    {
        d.WithConstructorArgument<IDirectDependency, DirectDependency1>(d => d
            .WithConstructorArgument<ISubDependency1, SubDependency1_1>()
            .WithConstructorArgument<ISubDependency2, SubDependency2_1>()
            .WithConstructorArgument<ISubDependency3, SubDependency3_1>()
        );
        d.WithConstructorArgument<IDirectDependency, DirectDependency2>(d => d
            .WithConstructorArgument<ISubDependency1, SubDependency1_2>()
            .WithConstructorArgument<ISubDependency2, SubDependency2_2>()
            .WithConstructorArgument<ISubDependency3, SubDependency3_2>()
        );
    })
;
```

## ForEvolve.DependencyInjection.ContextualBindings.AspNetCore

The `ForEvolve.DependencyInjection.ContextualBindings.AspNetCore` library adds support for contextual dependency injection into controller's constructors. It extend the capabilities of `ForEvolve.DependencyInjection.ContextualBindings`. Under the hood, it decorates the default `IControllerActivator` by a custom implementation, loading conditional bindings when they exist and falling back to the default implementation when none does.

> This project uses `Scrutor` to decorate the `IControllerActivator`.

### Install

```bash
dotnet add package ForEvolve.DependencyInjection.ContextualBindings.AspNetCore
```

### How to use

To enable controller injection, you must register the `IControllerActivator` decorator by calling the `WithContextualBindings()` extension method on an `IMvcBuilder`, like this:

```csharp
services
    .AddControllers()
    .WithContextualBindings();
```

Then you can use the capabilities of `ForEvolve.DependencyInjection.ContextualBindings`, but for controllers, like this:

```csharp
services
    .AddContextualBinding<FirstController>(d => d
        .WithConstructorArgument<IService, Implementation1>())
    .AddContextualBinding<SecondController>(d => d
        .WithConstructorArgument<IService, Implementation2>())
;
```

## ForEvolve.DependencyInjection.Modules

The goal of the `ForEvolve.DependencyInjection.Modules` library is to allow splitting DI bindings into modules and enabling auto-discovery of those modules.

### Install

```bash
dotnet add package ForEvolve.DependencyInjection.Modules
```

### How to use

To enable and scan for modules, use the following code:

```csharp
services
    .AddDependencyInjectionModules()
    .ScanAssemblies(typeof(Program).Assembly)
    .Initialize()
;
// OR
services
    .AddDependencyInjectionModules()
    .ScanAssemblies(typeof(AnyClassThatIsPartOfTheAssemblyThatYouWantToScan).Assembly, typeof(ClassFromAnotherAssembly).Assembly)
    .Initialize()
;
```

To create a module you can implement `IDependencyInjectionModule` or inherit from `DependencyInjectionModule`.
You can inject any dependencies that you want in your constructor, as long as you defined them (see bellow).
Once you have a class, register your dependencies, like this:

```csharp
public class SomeImportantDIModule : DependencyInjectionModule
{
    public ContextualServiceInjectionModule(IServiceCollection services)
        : base(services)
    {
        services.AddSingleton<ISomeService, SomeImplementation>();
        services
            .AddContextualBinding<FirstController>(d => d
                .WithConstructorArgument<IService, Implementation1>())
        ;
        // ...
    }
}
```

You can also register custom dependencies that can be injected in your modules like this:

```csharp
services
    .AddDependencyInjectionModules()
    .UseConfiguration(configuration)
    .ConfigureServices(services => services.TryAddSingleton<ISomeInterface, SomeImplementation>())
    .ScanAssemblies(typeof(Program).Assembly)
    .Initialize()
;
// ...
public class SomeOtherModule : DependencyInjectionModule
{
    public ContextualServiceInjectionModule(IServiceCollection services, ISomeInterface someInterface)
        : base(services)
    {
        // You can now use `someInterface` to do something...
    }
}

```

The dependencies are only used during the registration process, are only added to a private `IServiceCollection`, and are not added to the application `IServiceCollection`. Use these only to initialize modules.

As long as the assembly containing the modules are scanned, you can split your bindings as you want and you don't need to do anything else.

# Future

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

# Found a bug or have a feature request?

Please open an issue and be as clear as possible; see _How to contribute?_ for more information.

# How to contribute?

If you would like to contribute to the project, first, thank you for your interest, and please read [Contributing to ForEvolve open source projects](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CONTRIBUTING.md) for more information.

## Contributor Covenant Code of Conduct

Also, please read the [Contributor Covenant Code of Conduct](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CODE_OF_CONDUCT.md) that applies to all ForEvolve repositories.
