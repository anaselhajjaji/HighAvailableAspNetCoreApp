using NetArchTest.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using SystemdHealthcheck;

namespace ArchTests.Api
{
    [TestFixture]
    public class ApiTests
    {
        protected static Assembly ApiAssembly = typeof(Startup).Assembly;

        [Test]
        public void EmployeesController_DoesNotHaveDependency_ToRepository()
        {
            var otherModules = new List<string>
            {
                "Repository"
            };
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("SystemdHealthcheck.Controllers")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            Assert.That(types, Is.Null.Or.Empty);
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}