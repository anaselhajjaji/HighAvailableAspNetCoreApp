using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Healthcheck.Apis.HealthChecks
{
    public class HealthCheckPublisher : IHealthCheckPublisher
    {
        private readonly ILogger _logger;

        public HealthCheckPublisher(ILogger<HealthCheckPublisher> logger)
        {
            _logger = logger;
        }

        // The following example is for demonstration purposes only. Health Checks 
        // Middleware already logs health checks results. A real-world readiness 
        // check in a production app might perform a set of more expensive or 
        // time-consuming checks to determine if other resources are responding 
        // properly.
        public Task PublishAsync(HealthReport report, 
            CancellationToken cancellationToken)
        {
            if (report.Status == HealthStatus.Healthy)
            {
                _logger.LogInformation("{Timestamp} Readiness Probe Status: {Result}", 
                    DateTime.UtcNow, report.Status);

                // Call systemd-notify on linux
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    StartSystemdNotify();
                }    
            }
            else
            {
                _logger.LogError("{Timestamp} Readiness Probe Status: {Result}", 
                    DateTime.UtcNow, report.Status);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }

        private void StartSystemdNotify() 
        {
            _logger.LogInformation("{Timestamp} Notify systemd...", DateTime.UtcNow);
            try 
            {
                var process = new Process();
                process.StartInfo.FileName = "/bin/systemd-notify";
                process.StartInfo.Arguments = "--ready";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, data) => {
                    _logger.LogInformation("{Timestamp} systemd { Result }...", 
                        DateTime.UtcNow,
                        data.Data);
                };
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += (sender, data) => {
                    _logger.LogInformation("{Timestamp} systemd { Result }...", 
                        DateTime.UtcNow,
                        data.Data);
                };
                process.Start();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Timestamp} systemd-notify could not be run.", DateTime.UtcNow);
                
            }
        }
    }
}