using System.Reflection;
using Autofac;
using AutoWrapper;
using FootballPredictor.Api.Authentication;
using FootballPredictor.Data.Abstractions;
using FootballPredictor.Data.EFCore.PostgreSQL;
using FootballPredictor.Data.InMemory;
using FootballPredictor.Domain.Services;
using FootballPredictor.FootballDataProvider.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballPredictor.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

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
            // https://elanderson.net/2019/10/swagger-openapi-with-nswag-and-asp-net-core-3/
            AddOpenApiDocument(services); 
            services.AddAuthentication();
            services.AddHttpClient();

            // Dependencies
            services.AddTransient<IMatchDataProvider, FootballDataMatchDataProvider>();
            services.AddPostgresDbContext(options =>
            {
                options.ConnectionString = @"Host=rogue.db.elephantsql.com;Database=hhgfafoj;Username=hhgfafoj;Password=bxK--h2kwjxoblpg2JsN1AYIHLef0012";
            });

            #region Local Development Dependencies
            if (Environment.IsDevelopment())
            {
                services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
            } 
            #endregion
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
            app.UseAuthentication();
            app.UseApiResponseAndExceptionWrapper(); // Must be "before" the UseRouting()
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Swagger
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        // Register your own things directly with Autofac, like:
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Command / Query Handlers
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsClosedTypesOf(typeof(IQueryHandler<,>));

            // >> InMemory Database Repositories
            builder.AddInMemoryDatabase(options =>
            {
                options.Assemblies = new AssemblyList(typeof(TeamsInMemoryDbRepository).Assembly);
            });

            // >> PostgreSQL Repositories
            builder.AddPostgresDbRepositories(options =>
            {
                options.Assemblies = new AssemblyList(typeof(MatchPostgresRepository).Assembly);
            });
        }

        private static void AddOpenApiDocument(IServiceCollection services)
        {
            services.AddOpenApiDocument(document =>
            {
                document.PostProcess = d =>
                {
                    d.Info.Version = "v1";
                    d.Info.Title = typeof(Startup).Namespace;
                    d.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "CodeBoss",
                        Email = "connect@codeboss.co.za",
                        Url = "http://codeboss.co.za"
                    };
                };
            });
        }
    }
}
