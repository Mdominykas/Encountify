// TODO: Use Entity Framework Functions instead of SQL Commands

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using EncountifyAPI.Data;
using System.IO;
using Serilog;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using EncountifyAPI.Middleware;
using EncountifyAPI.Controllers;
using EncountifyAPI.Aspects;
using EncountifyAPI.Interfaces;


namespace EncountifyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
              .CreateLogger();

            Log.Information("-------------------------------------------------------------------");
            Log.Information("NEW LOGGING SESSION");
            Log.Information("-------------------------------------------------------------------");

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EncountifyAPI",
                    Description = "An ASP.NET Core Web API for the Encountify Xamarin.Forms App",
                    TermsOfService = new Uri("https://github.com/Mdominykas/Encountify"),
                    Contact = new OpenApiContact
                    {
                        Name = "Contact",
                        Url = new Uri("https://github.com/Mdominykas/Encountify")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://github.com/Mdominykas/Encountify/blob/main/LICENSE.md")
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddDbContext<EncountifyAPIContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EncountifyAPIContext")));

            services.AddSingleton(x => Log.Logger);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<LocationControllerExecutables>().As<ILocationExecutables>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LogAspect))
                .InstancePerDependency();

            builder.RegisterType<UserControllerExecutables>().As<IUserHandlerExecutables>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LogAspect))
                .InstancePerDependency();

            builder.RegisterType<VisitedControllerExecutables>().As<IVisitedExecutables>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LogAspect))
                .InstancePerDependency();

            builder.RegisterType<AchievmentControllerExecutables>().As<IAchievmentExecutables>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LogAspect))
                .InstancePerDependency();

            builder.RegisterType<AssignedAchievmentControllerExecutables>().As<IAssignedAchievmentExecutables>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LogAspect))
                .InstancePerDependency();

            builder.Register(x => Log.Logger).SingleInstance();
            builder.RegisterType<LogAspect>().SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EncountifyAPI v1"));

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseMiddleware<StatisticsMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
