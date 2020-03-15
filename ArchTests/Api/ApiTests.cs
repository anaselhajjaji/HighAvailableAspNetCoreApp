using NetArchTest.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Healthcheck.Apis;

namespace Healthcheck.ArchTests
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
                "Healthcheck.Repository"
            };
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("Healthcheck.Apis.Controllers")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            Assert.That(result.FailingTypes, Is.Null.Or.Empty);
        }
    }
}