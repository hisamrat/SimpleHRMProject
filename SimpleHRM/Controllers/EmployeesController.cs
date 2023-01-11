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

        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
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
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                if (!_employeeRepository.EmployeeExists(id))
                {
       
                    return NotFound();
                }

                var empobj = await _employeeRepository.GetEmployee(id);

                if (!await _employeeRepository.DeleteEmployee(empobj))
                {
                   
                    return StatusCode(StatusCodes.Status500InternalServerError);
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
