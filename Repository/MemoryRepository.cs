using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemdHealthcheck.Models;

namespace SystemdHealthcheck.Repository
{
    public class MemoryRepository : IRepository<Employee>
    {
        private readonly List<Employee> employees = new List<Employee>();
        private int generatedId = 0;

        public IEnumerable<Employee> GetAll()
        {
            return employees;
        }

        public async Task Insert(Employee element)
        {
            element.Id = ++generatedId;
            employees.Add(element);
        }
    }
}
