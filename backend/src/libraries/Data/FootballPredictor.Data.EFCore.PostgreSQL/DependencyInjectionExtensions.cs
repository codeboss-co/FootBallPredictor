using FootballPredictor.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Autofac;

namespace FootballPredictor.Data.EFCore.PostgreSQL
{
    public static class DependencyInjectionExtensions
    {
        public static void AddPostgresDbContext(this IServiceCollection services, Action<DbOptions> configure)
        {
            // Register Custom Options
            var options = new DbOptions();
            configure(options);

            services.AddDbContext<FootballDbContext>(x => x.UseNpgsql(options.ConnectionString));
        }

        public static void AddPostgresDbRepositories(this ContainerBuilder builder, Action<DbOptions> configure)
        {
            var options = new DbOptions();
            configure(options);

            // Repositories
            builder.RegisterAssemblyTypes(options.Assemblies?.ToArray() ?? new[] {Assembly.GetExecutingAssembly()})
                .AsClosedTypesOf(typeof(IGenericDbRepository<,>))
                .AsImplementedInterfaces();
        }
    }
}
