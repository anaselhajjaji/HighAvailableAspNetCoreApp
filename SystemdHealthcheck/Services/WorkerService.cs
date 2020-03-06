using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SystemdHealthcheck.Services
{
    public class WorkerService : BackgroundService
    {
        private ILogger<WorkerService> _logger;
        private readonly WorkerServiceHealthCheck _workerServiceHealthCheck;
        private int stopWorkerAfterSeconds = 30;
        private readonly IMediator _mediator;

        public WorkerService(ILogger<WorkerService> logger,
            WorkerServiceHealthCheck workerServiceHealthCheck,
            IMediator mediator)
        {
            this._logger = logger;
            this._workerServiceHealthCheck = workerServiceHealthCheck;
            this._mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && stopWorkerAfterSeconds > 0)
            {
                _logger.LogInformation("Worker service running at { time }", DateTimeOffset.Now);
                _workerServiceHealthCheck.WorkerRunning = true;

                await Task.Delay(1000, stoppingToken);
                stopWorkerAfterSeconds--;
            }

            _workerServiceHealthCheck.WorkerRunning = false;
            await this._mediator.Publish(new ServiceEvent("Worker Service stopped working..."));
        }
    }
}
