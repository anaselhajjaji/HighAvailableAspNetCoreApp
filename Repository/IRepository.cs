using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemdHealthcheck.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        Task Insert(T element);
    }
}
