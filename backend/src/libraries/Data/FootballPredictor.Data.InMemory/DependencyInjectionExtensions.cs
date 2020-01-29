using Autofac;
using FootballPredictor.Data.Abstractions;
using System;
using System.Reflection;

namespace FootballPredictor.Data.InMemory
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInMemoryDatabase(this ContainerBuilder builder, Action<DbOptions> configure)
        {
            var options = new DbOptions();
            configure(options);

            // Repositories
            builder.RegisterAssemblyTypes(options.Assemblies?.ToArray() ?? new[] { Assembly.GetExecutingAssembly()} )
                .AsClosedTypesOf(typeof(IGenericDbRepository<,>))
                .AsImplementedInterfaces();

            // Seeders
            builder.RegisterAssemblyTypes(options.Assemblies?.ToArray() ?? new []{ Assembly.GetExecutingAssembly() })
                .AsClosedTypesOf(typeof(DbSeeder<>));
        }
    }
}
