using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Healthcheck.Apis.Services
{
    public class SecondWorkerService : BackgroundService, INotificationHandler<ServiceEvent>
    {
        private ILogger<SecondWorkerService> _logger;
        private readonly SecondWorkerServiceHealthCheck _workerServiceHealthCheck;
        
        public SecondWorkerService(ILogger<SecondWorkerService> logger,
            SecondWorkerServiceHealthCheck workerServiceHealthCheck)
        {
            this._logger = logger;
            this._workerServiceHealthCheck = workerServiceHealthCheck;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker service running at { time }", DateTimeOffset.Now);
                _workerServiceHealthCheck.WorkerRunning = true;

                await Task.Delay(1000, stoppingToken);
            }

            _workerServiceHealthCheck.WorkerRunning = false;
        }

        public async Task Handle(ServiceEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Received service notification: { notification.Message }");
        }
    }
}
