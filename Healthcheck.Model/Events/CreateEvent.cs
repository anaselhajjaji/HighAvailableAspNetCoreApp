using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcheck.Model.Events
{
    public class CreateEvent<T> : IRequest<T>
    {
        public T Item { get; }

        public CreateEvent(T item)
        {
            Item = item;
        }
    }
}
