﻿using System;
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
using SimpleHRM.Utility;

namespace SimpleHRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(ApplicationDbContext dbContext,IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Search employee information here
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paginationDto"></param>
        /// <returns></returns>

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchEmployee(string Searchvalue, [FromQuery] PaginationDto paginationDto)
        {
            try
            {              
                DateTime chaeckdate;
                var check = DateTime.TryParse(Searchvalue, out chaeckdate);            
                var queryable = _dbContext.Employees.AsQueryable();
                if (!string.IsNullOrEmpty(Searchvalue))
                {                 
                    queryable = queryable.Where(a =>  a.Id.ToString()==Searchvalue.Trim() || a.JoiningDate.Date == (check == true ? Convert.ToDateTime(Searchvalue).Date : null) || a.DateOfBirth.Date ==  (check==true?Convert.ToDateTime(Searchvalue).Date:null) || a.FirstName.Trim().StartsWith(Searchvalue.Trim()) || a.MiddleName.Trim().StartsWith(Searchvalue.Trim()) || a.LastName.Trim().StartsWith(Searchvalue.Trim()) || a.Designation.Trim().StartsWith(Searchvalue.Trim()) || a.Department.Trim().StartsWith(Searchvalue.Trim()));
                }
                await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDto.RecordsPerPage);
                var objlist = queryable.Paginate(paginationDto).AsNoTracking().OrderBy(p => p.Id).ToList();            
                var empmodel = _mapper.Map<List<EmployeeDto>>(objlist);
                return Ok(empmodel);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Get a list of employee information
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get individual employee information with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

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
        /// <summary>
        /// Update individual employee information
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Create individual employee information
        /// </summary>
        /// <param name="employeeCreateDto"></param>
        /// <returns></returns>

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
                var emp = _mapper.Map<EmployeeDto>(employee);
                return CreatedAtAction("GetEmployee", new { id = employee.Id }, emp);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Delete individual employee information with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
