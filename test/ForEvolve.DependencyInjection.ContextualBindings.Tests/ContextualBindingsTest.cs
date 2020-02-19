using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace ForEvolve.DependencyInjection.ContextualBindings
{
    public class ContextualBindingsTest
    {
        [Fact]
        public void Test_with_concrete_consumers()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<Consumer1>(setup: ctx => ctx.WithConstructorArgument<IService, Service1>());
            services
                .AddContextualBinding<Consumer2>(setup: ctx => ctx.WithConstructorArgument<IService, Service2>());
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var consumer1 = serviceProvider.GetService<Consumer1>();
            var consumer2 = serviceProvider.GetService<Consumer2>();

            // Assert
            Assert.IsType<Service1>(consumer1.Service);
            Assert.IsType<Service2>(consumer2.Service);
        }

        [Fact]
        public void Test_with_abstract_consumers()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<IConsumer, Consumer1>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>());
            services
                .AddContextualBinding<ISomeOtherInterface, Consumer2>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service2>());
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var consumer1 = serviceProvider.GetService<IConsumer>();
            var consumer2 = serviceProvider.GetService<ISomeOtherInterface>();

            // Assert
            Assert.IsType<Service1>(consumer1.Service);
            Assert.IsType<Service2>(consumer2.Service);
        }

        [Fact]
        public void Test_singleton_lifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<IConsumer, Consumer1>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Singleton));
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var consumer1 = serviceProvider.GetService<IConsumer>();
            var consumer2 = serviceProvider.GetService<IConsumer>();

            // Assert
            Assert.Same(consumer1.Service, consumer2.Service);
        }

        [Fact]
        public void Test_scoped_lifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<IConsumer, Consumer1>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Scoped));
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var consumer1 = serviceProvider.GetService<IConsumer>();
            var consumer2 = serviceProvider.GetService<IConsumer>();

            // Assert
            Assert.Same(consumer1.Service, consumer2.Service);
        }

        [Fact]
        public void Test_multiple_scoped_lifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<IConsumer, Consumer1>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Scoped));
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var scope1 = serviceProvider.CreateScope();
            var consumer1 = scope1.ServiceProvider.GetService<IConsumer>();
            var consumer2 = scope1.ServiceProvider.GetService<IConsumer>();

            var scope2 = serviceProvider.CreateScope();
            var consumer3 = scope2.ServiceProvider.GetService<IConsumer>();
            var consumer4 = scope2.ServiceProvider.GetService<IConsumer>();

            // Assert
            Assert.Same(consumer1.Service, consumer2.Service);
            Assert.Same(consumer3.Service, consumer4.Service);
            Assert.NotSame(consumer1.Service, consumer3.Service);
        }

        [Fact]
        public void Test_transient_lifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<IConsumer, Consumer1>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Transient));
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var consumer1 = serviceProvider.GetService<IConsumer>();
            var consumer2 = serviceProvider.GetService<IConsumer>();

            // Assert
            Assert.NotSame(consumer1.Service, consumer2.Service);
        }

        [Fact]
        public void Test_consumer_with_multiple_of_the_same_argument()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<DualConsumer>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Transient)
                .WithConstructorArgument<IService, Service2>(lifetime: ServiceLifetime.Transient))
                ;
            services
                .AddContextualBinding<IDualConsumer, DualConsumer>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service2>(lifetime: ServiceLifetime.Transient)
                .WithConstructorArgument<IService, Service2>(lifetime: ServiceLifetime.Transient))
                ;
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var dualConsumer1 = serviceProvider.GetService<DualConsumer>();
            var dualConsumer2 = serviceProvider.GetService<IDualConsumer>();

            // Assert
            Assert.IsType<Service1>(dualConsumer1.ServiceA);
            Assert.IsType<Service2>(dualConsumer1.ServiceB);
            Assert.IsType<Service2>(dualConsumer2.ServiceA);
            Assert.IsType<Service2>(dualConsumer2.ServiceB);
        }

        [Fact]
        public void Test_mixed_cases()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<INonContextual, NonContextual>();
            services
                .AddContextualBinding<MixedClass1>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Transient))
                ;
            services
                .AddContextualBinding<MixedClass2>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service2>(lifetime: ServiceLifetime.Transient)
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Transient))
                ;
            services
                .AddContextualBinding<MixedClass3>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(lifetime: ServiceLifetime.Transient)
                .WithConstructorArgument<IService, Service2>(lifetime: ServiceLifetime.Transient))
                ;
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var mixedClass1 = serviceProvider.GetService<MixedClass1>();
            var mixedClass2 = serviceProvider.GetService<MixedClass2>();
            var mixedClass3 = serviceProvider.GetService<MixedClass3>();

            // Assert
            Assert.IsType<Service1>(mixedClass1.Service);
            Assert.IsType<Service2>(mixedClass2.ServiceA);
            Assert.IsType<Service1>(mixedClass2.ServiceB);
            Assert.IsType<Service1>(mixedClass3.ServiceA);
            Assert.IsType<Service2>(mixedClass3.ServiceB);
            Assert.Same(mixedClass1.NonContextual, mixedClass2.NonContextual);
            Assert.Same(mixedClass1.NonContextual, mixedClass3.NonContextual);
        }

        [Fact]
        public void Should_build_complex_object_tree()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<INonContextual, NonContextual>();
            services
                .AddContextualBinding<MixedClass3>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>()
                .WithConstructorArgument<IService, Service2>(lifetime: ServiceLifetime.Singleton))
                ;
            services
                .AddContextualBinding<ComplexObject>(setup: ctx => ctx
                .WithConstructorArgument<IConsumer, Consumer2>(lifetime: ServiceLifetime.Singleton)
                .WithConstructorArgument<IService, Service2>(lifetime: ServiceLifetime.Singleton))
                ;
            services.AddSingleton<IService, Service1>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var complexObject1 = serviceProvider.GetService<ComplexObject>();
            var complexObject2 = serviceProvider.GetService<ComplexObject>();
            var service = serviceProvider.GetService<IService>();

            // Assert
            Assert.IsType<Service1>(service);
            Assert.Same(complexObject1.Consumer, complexObject2.Consumer);
            Assert.IsType<Service2>(complexObject1.Service);
            Assert.IsType<Service2>(complexObject2.Service);
            Assert.IsType<NonContextual>(complexObject2.NonContextual);
            Assert.NotSame(complexObject1.MixedClass, complexObject2.MixedClass);
            Assert.IsType<Service1>(complexObject1.MixedClass.ServiceA);
            Assert.IsType<Service1>(complexObject2.MixedClass.ServiceA);
            Assert.IsType<Service2>(complexObject1.MixedClass.ServiceB);
            Assert.IsType<Service2>(complexObject2.MixedClass.ServiceB);
            Assert.Same(complexObject1.MixedClass.ServiceB, complexObject2.MixedClass.ServiceB);
        }

        [Fact]
        public void Should_allow_top_level_singleton()
        {
            // Arrange
            var services = new ServiceCollection();
            services
                .AddContextualBinding<IConsumer, Consumer1>(setup: ctx => ctx
                .WithConstructorArgument<IService, Service1>(), lifetime: ServiceLifetime.Singleton)
                ;
            var serviceProvider = services.BuildServiceProvider();


            // Act
            var consumer1 = serviceProvider.GetService<IConsumer>();
            var consumer2 = serviceProvider.GetService<IConsumer>();

            // Assert
            Assert.Same(consumer1, consumer2);
        }
    }

    public interface IService { }
    public class Service1 : IService { }
    public class Service2 : IService { }

    public interface IConsumer
    {
        IService Service { get; }
    }

    public interface ISomeOtherInterface
    {
        IService Service { get; }
    }

    public class Consumer1 : IConsumer
    {
        public Consumer1(IService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public IService Service { get; }
    }

    public class Consumer2 : IConsumer, ISomeOtherInterface
    {
        public Consumer2(IService service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public IService Service { get; }
    }

    public interface IDualConsumer
    {
        IService ServiceA { get; }
        IService ServiceB { get; }
    }

    public class DualConsumer : IDualConsumer
    {
        public DualConsumer(IService serviceA, IService serviceB)
        {
            ServiceA = serviceA ?? throw new ArgumentNullException(nameof(serviceA));
            ServiceB = serviceB ?? throw new ArgumentNullException(nameof(serviceB));
        }

        public IService ServiceA { get; }
        public IService ServiceB { get; }
    }

    public interface INonContextual { }
    public class NonContextual : INonContextual { }

    public class MixedClass1
    {
        public MixedClass1(IService service, INonContextual nonContextual)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
            NonContextual = nonContextual ?? throw new ArgumentNullException(nameof(nonContextual));
        }

        public IService Service { get; }
        public INonContextual NonContextual { get; }
    }

    public class MixedClass2
    {
        public MixedClass2(IService serviceA, IService serviceB, INonContextual nonContextual)
        {
            ServiceA = serviceA ?? throw new ArgumentNullException(nameof(serviceA));
            ServiceB = serviceB ?? throw new ArgumentNullException(nameof(serviceB));
            NonContextual = nonContextual ?? throw new ArgumentNullException(nameof(nonContextual));
        }

        public IService ServiceA { get; }
        public IService ServiceB { get; }
        public INonContextual NonContextual { get; }
    }

    public class MixedClass3
    {
        public MixedClass3(IService serviceA, INonContextual nonContextual, IService serviceB)
        {
            ServiceA = serviceA ?? throw new ArgumentNullException(nameof(serviceA));
            ServiceB = serviceB ?? throw new ArgumentNullException(nameof(serviceB));
            NonContextual = nonContextual ?? throw new ArgumentNullException(nameof(nonContextual));
        }

        public IService ServiceA { get; }
        public IService ServiceB { get; }
        public INonContextual NonContextual { get; }
    }

    public class ComplexObject
    {
        public ComplexObject(IConsumer consumer, IService service, INonContextual nonContextual, MixedClass3 mixedClass)
        {
            Consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            Service = service ?? throw new ArgumentNullException(nameof(service));
            NonContextual = nonContextual ?? throw new ArgumentNullException(nameof(nonContextual));
            MixedClass = mixedClass ?? throw new ArgumentNullException(nameof(mixedClass));
        }

        public IConsumer Consumer { get; }
        public IService Service { get; }
        public INonContextual NonContextual { get; }
        public MixedClass3 MixedClass { get; }
    }
}
