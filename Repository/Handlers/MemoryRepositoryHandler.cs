using MediatR;
using Healthcheck.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Healthcheck.Model.Events;
using Healthcheck.Model.Dtos;

namespace Healthcheck.Repository.Handlers
{
    public class MemoryRepositoryHandler : IRequestHandler<CreateEvent<Employee>, Employee>, IRequestHandler<DeleteAllEvent, bool>, IRequestHandler<GetAllEvent<Employee>, IEnumerable<Employee>>
    {
        private readonly IRepository<Employee> _repository;

        public MemoryRepositoryHandler(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        public async Task<Employee> Handle(CreateEvent<Employee> notification, CancellationToken cancellationToken)
        {
            await _repository.Insert(notification.Item);
            return notification.Item;
        }

        public async Task<bool> Handle(DeleteAllEvent notification, CancellationToken cancellationToken)
        {
            await _repository.DeleteAll();
            return true;
        }

        public async Task<IEnumerable<Employee>> Handle(GetAllEvent<Employee> notification, CancellationToken cancellationToken)
        {
            return _repository.GetAll();
        }
    }
}
