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
        [Test]
        public void EmployeesController_DoesNotHaveDependency_ToRepository()
        {
            Assembly ApiAssembly = typeof(Startup).Assembly;

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

            Assert.That(result.FailingTypes, Is.Null.Or.Empty);
        }
    }
}