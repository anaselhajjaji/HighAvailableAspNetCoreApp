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
using SystemdHealthcheck.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SystemdHealthcheck.Services;

namespace SystemdHealthcheck
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
            services.AddControllers();

            services.AddHostedService<WorkerService>();
            services.AddSingleton<WorkerServiceHealthCheck>();

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
                    tags: new[] { "ready" }); ;

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(5);
                options.Predicate = (check) => check.Tags.Contains("ready");
                options.Period = TimeSpan.FromSeconds(30);
            });

            services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
