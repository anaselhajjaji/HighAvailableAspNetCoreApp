using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Healthcheck.Apis.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using MediatR;
using Healthcheck.Repository.Interfaces;
using Healthcheck.Repository;
using Healthcheck.Apis.Services;
using Healthcheck.Model.Dtos;

namespace Healthcheck.Apis
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
            // Add application dependencies
            services.AddSingleton<IRepository<Employee>, MemoryRepository>();

            // Add controllers
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SystemdHealthcheck API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Anas EL HAJJAJI"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://github.com/anaselhajjaji/systemdhealthcheck/blob/master/LICENSE"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Add hosted services
            services.AddHostedService<WorkerService>();
            services.AddSingleton<WorkerServiceHealthCheck>();
            services.AddHostedService<SecondWorkerService>();
            services.AddSingleton<SecondWorkerServiceHealthCheck>();

            // Add health checks UI
            var port = Environment.GetEnvironmentVariable("PORT");
            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("Application HealthCheck", "http://localhost:"+ port +"/health");
            });

            // Add Health Check publisher
            services.AddHealthChecks()
                .AddCheck<WorkerServiceHealthCheck>(
                    "worker_running_check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "ready" })
                .AddCheck<SecondWorkerServiceHealthCheck>(
                    "second_worker_running_check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "ready" });

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(5);
                options.Predicate = (check) => check.Tags.Contains("ready");
                options.Period = TimeSpan.FromSeconds(30);
            });

            services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();

            // Add MediatR
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly, typeof(IRepository<>).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            // Add health check UI
            app.UseHealthChecksUI(config => config.UIPath = "/health-ui");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    // Wtite the health in a format understandable by the HealthCheckUI
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
