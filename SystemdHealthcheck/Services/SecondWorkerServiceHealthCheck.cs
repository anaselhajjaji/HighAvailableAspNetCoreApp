using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Healthcheck.Apis.Services
{
    public class SecondWorkerServiceHealthCheck : IHealthCheck
    {
        private volatile bool _workerRunning = false;

        public string Name => "second_worker_running_check";

        public bool WorkerRunning
        {
            get => _workerRunning;
            set => _workerRunning = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (WorkerRunning)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("The second worker service is running without problem."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("The second worker running is not running."));
        }
    }
}
