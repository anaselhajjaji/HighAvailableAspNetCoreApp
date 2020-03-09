using MediatR;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Events
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
