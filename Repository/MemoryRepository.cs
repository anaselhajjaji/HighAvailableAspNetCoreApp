using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.Models;

namespace Repository
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

        public async Task DeleteAll()
        {
            generatedId = 0;
            employees.Clear();
        }
    }
}
