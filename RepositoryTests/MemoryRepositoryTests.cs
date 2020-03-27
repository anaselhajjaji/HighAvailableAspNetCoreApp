using Healthcheck.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Healthcheck.Model.Dtos;
using System.Threading.Tasks;

namespace Healthcheck.Repository.Tests
{
    [TestClass()]
    public class MemoryRepositoryTests
    {
        [TestMethod()]
        public async Task InsertTest()
        {
            MemoryRepository repository = new MemoryRepository();
            IEnumerable<Employee> employeeList = await repository.GetAll();

            Assert.AreEqual(employeeList.Count(), 0);

            await repository.Insert(new Employee { Id = 1, FirstName = "Employee 1", LastName = "Employee 1", Email = "Email 1" });
            employeeList = await repository.GetAll();
            Assert.AreEqual(employeeList.Count(), 1);

            await repository.Insert(new Employee { Id = 2, FirstName = "Employee 2", LastName = "Employee 2", Email = "Email 2" });
            employeeList = await repository.GetAll();
            Assert.AreEqual(employeeList.Count(), 2);

            await repository.DeleteAll();
            employeeList = await repository.GetAll();
            Assert.AreEqual(employeeList.Count(), 0);
        }
    }
}