using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHRM.DataAccess.Data;
using SimpleHRM.DataAccess.Repositories.IRepositories;
using SimpleHRM.Models;
using SimpleHRM.Models.DTO;

namespace SimpleHRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(ApplicationDbContext context,IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _context = context;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Employee>>   CreateEmployee([FromQuery] EmployeeCreateDto employeeCreateDto)
        {
            try
            {
                if (employeeCreateDto == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                var employee = _mapper.Map<Employee>(employeeCreateDto);
               
                if (!_employeeRepository.CreateEmployee(employee))
                {
                    ModelState.AddModelError("", $"sometion went wrong saving record{employeeCreateDto.FirstName}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);

            }
            catch (Exception)
            {

                throw;
            }
           
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
