﻿//-----------------------------------------------------------------------
// <copyright file="AkkaDiFixture.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2021 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2021 .NET Foundation <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Akka.DependencyInjection.Tests
{
    public class AkkaDiFixture : IDisposable
    {
        public interface IDependency : IDisposable
        {
            string Name { get; }

            bool Disposed { get; }
        }

        public interface ITransientDependency : IDependency
        {
        }

        public class Transient : ITransientDependency
        {
            public Transient() : this("t" + Guid.NewGuid().ToString())
            {
            }

            public Transient(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }

        public interface IScopedDependency : IDependency
        {
        }

        public class Scoped : IScopedDependency
        {
            public Scoped() : this("s" + Guid.NewGuid().ToString())
            {
            }

            public Scoped(string name)
            {
                Name = name;
            }

            public void Dispose()
            {
                Disposed = true;
            }

            public bool Disposed { get; private set; }
            public string Name { get; }
        }

        public interface ISingletonDependency : IDependency
        {
        }

        public class Singleton : ISingletonDependency
        {
            public Singleton() : this("singleton")
            {
            }

            public Singleton(string name)
            {
                Name = name;
            }

            public void Dispose()
            {
                Disposed = true;
            }

            public bool Disposed { get; private set; }
            public string Name { get; }
        }

        public AkkaDiFixture()
        {
            Services = new ServiceCollection();

            // <DiFixture>
            // register some default services
            Services.AddSingleton<ISingletonDependency, Singleton>()
                .AddScoped<IScopedDependency, Scoped>()
                .AddTransient<ITransientDependency, Transient>();
            // </DiFixture>
            Provider = Services.BuildServiceProvider();
        }

        public IServiceCollection Services { get; private set; }

        public IServiceProvider Provider { get; private set; }

        public void Dispose()
        {
            Services = null;
            Provider = null;
        }
    }
}
