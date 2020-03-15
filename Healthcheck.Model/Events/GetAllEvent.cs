using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcheck.Model.Events
{
    public class GetAllEvent<T> : IRequest<IEnumerable<T>>
    {
    }
}
