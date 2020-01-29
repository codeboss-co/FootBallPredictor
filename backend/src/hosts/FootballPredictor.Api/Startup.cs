using System.Reflection;
using Autofac;
using FootballPredictor.Data.Abstractions;
using FootballPredictor.Data.InMemory;
using FootballPredictor.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballPredictor.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: strict cors policy - Local dev only
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy => policy
                    .WithOrigins(
                        "http://localhost:4200" // Angular App
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                );
            });

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        // Register your own things directly with Autofac, like:
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Command / Query Handlers
            builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly()).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly()).AsClosedTypesOf(typeof(IQueryHandler<,>));

            builder.AddInMemoryDatabase(options =>
            {
                options.Assemblies = new AssemblyList(typeof(TeamsInMemoryDbRepository).Assembly);
            });
        }
    }
}
