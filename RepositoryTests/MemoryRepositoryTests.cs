using Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Repository.Tests
{
    [TestClass()]
    public class MemoryRepositoryTests
    {
        [TestMethod()]
        public void InsertTest()
        {
            MemoryRepository repository = new MemoryRepository();

            Assert.AreEqual(repository.GetAll().Count(), 0);

            repository.Insert(new Models.Employee { Id = 1, FirstName = "Employee 1", LastName = "Employee 1", Email = "Email 1" });
            Assert.AreEqual(repository.GetAll().Count(), 1);

            repository.Insert(new Models.Employee { Id = 2, FirstName = "Employee 2", LastName = "Employee 2", Email = "Email 2" });
            Assert.AreEqual(repository.GetAll().Count(), 2);
        }
    }
}