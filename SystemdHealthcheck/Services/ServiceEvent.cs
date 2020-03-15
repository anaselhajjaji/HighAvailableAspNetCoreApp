using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthcheck.Apis.Services
{
    public class ServiceEvent : INotification
    {
        public string Message { get; }

        public ServiceEvent(string message)
        {
            Message = message;
        }
    }
}
