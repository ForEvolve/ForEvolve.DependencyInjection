﻿using ForEvolve.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ForEvolve.DependencyInjection
{
    public class ScanningContext : IScanningContext
    {
        protected IServiceCollection LocalServices { get; } = new ServiceCollection();
        private readonly List<TypeInfo> _modulesTypeInfo = new List<TypeInfo>();
        private bool _initialized = false;

        public ScanningContext(IServiceCollection services)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
            LocalServices.TryAddSingleton(services);
        }

        public IScanningContext ConfigureServices(Action<IServiceCollection> setup)
        {
            if (setup == null) { throw new ArgumentNullException(nameof(setup)); }
            setup(LocalServices);
            return this;
        }

        public IScanningContext Register(IEnumerable<TypeInfo> modulesTypeInfo)
        {
            if (modulesTypeInfo == null) { throw new ArgumentNullException(nameof(modulesTypeInfo)); }
            _modulesTypeInfo.AddRange(modulesTypeInfo);
            return this;
        }

        public void Initialize()
        {
            if (_initialized)
            {
                throw new ScanningContextInitializedException();
            }
            _initialized = true;
            using var serviceProvider = LocalServices.BuildServiceProvider();
            var modules = _modulesTypeInfo
                .KeepOnlyDependencyInjectionModules()
                .Distinct()
            ;
            foreach (var module in modules)
            {
                ActivatorUtilities.CreateInstance(serviceProvider, module.AsType());
            }
        }
    }
}
