namespace ForEvolve.DependencyInjection
{
    /// <summary>
    /// Marker interface that represents a DI module.
    /// Classes marked with this interface are instanciated during an assembly scan.
    /// Implement this to create DI module and register dependencies in the constructor of
    /// your class or inherit from <see cref="DependencyInjectionModule"/>.
    /// </summary>
    /// <remarks>
    /// When implementing this interface, make sure to register all dependencies with the
    /// <see cref="IScanningContext"/> by calling the 
    /// <see cref="IScanningContext.WithDependencies(System.Action{IServiceCollection})"/>
    /// method or any available extension methods.
    /// </remarks>
    public interface IDependencyInjectionModule { }
}
