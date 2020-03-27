using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcheck.Model.Events
{
    public class GetEvent<T> : IRequest<T>
    {
        public int Id { get; }

        public GetEvent(int id)
        {
            Id = id;
        }
    }
}
