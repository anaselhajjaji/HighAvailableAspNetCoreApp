using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        Task Insert(T element);
    }
}
