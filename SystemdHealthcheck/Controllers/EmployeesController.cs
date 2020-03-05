using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Models;
using Repository;

namespace SystemdHealthcheck.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger, IRepository<Employee> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Returns all the registered employees.
        /// </summary>
        /// <returns>All the registered employees</returns>
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            _logger.LogInformation("Returning employees.");
            return _repository?.GetAll();
        }

        /// <summary>
        /// Creates an employee.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "firstName": "First Name",
        ///        "lastName": "Last Name",
        ///        "email": "email@email.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="employee">the employee to create.</param>
        /// <returns>A newly created employee.</returns>
        /// <response code="201">Returns the newly created employee</response>
        /// <response code="400">If the item is null</response>     
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> Create(Employee employee)
        {
            _logger.LogInformation("Inserting new employee.");
            await _repository?.Insert(employee);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Deletes all the employees
        /// </summary>
        /// <returns>Nothing</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await _repository.DeleteAll();
            return NoContent();
        }
    }
}
