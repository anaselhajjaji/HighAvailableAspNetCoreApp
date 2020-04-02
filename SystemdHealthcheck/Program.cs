using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace Healthcheck.Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
        {
            var port = Environment.GetEnvironmentVariable("PORT");

            return Host.CreateDefaultBuilder(args)
                .UseSerilog() // To use Serilog
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseUrls("http://*:"+port);
                        webBuilder.UseStartup<Startup>();
                    });
        }
    }
}
