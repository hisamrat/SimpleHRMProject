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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var objlist =await _employeeRepository.GetEmployees();
                var empmodel = _mapper.Map<List<EmployeeDto>>(objlist);
                return Ok(empmodel);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var obj = await _employeeRepository.GetEmployee(id);
               

                if (obj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var employee = _mapper.Map<EmployeeDto>(obj);
                return Ok(employee);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee( [FromQuery] EmployeeDto employeeDto)
        {
            try
            {
                if (employeeDto == null)
                {
                    return BadRequest(ModelState);
                }

                var empobj = _mapper.Map<Employee>(employeeDto);

                if (!await _employeeRepository.UpdateEmployee(empobj))
                {
                    ModelState.AddModelError("", $"sometion went wrong update record{empobj.FirstName}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return NoContent();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEmployee([FromQuery] EmployeeCreateDto employeeCreateDto)
        {
            try
            {
                if (employeeCreateDto == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                var employee = _mapper.Map<Employee>(employeeCreateDto);
               
                if (!await _employeeRepository.CreateEmployee(employee))
                {
                    ModelState.AddModelError("", $"sometion went wrong saving record{employeeCreateDto.FirstName}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                if (!_employeeRepository.EmployeeExists(id))
                {
                    return NotFound();
                }

                var empobj = _employeeRepository.GetEmployee(id);

                if (!await _employeeRepository.DeleteEmployee(await empobj))
                {

                    return StatusCode(500, ModelState);
                }
                return NoContent();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }      
    }
}
