using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleHRM.DataAccess.Data;
using SimpleHRM.DataAccess.Repositories.IRepositories;
using SimpleHRM.Models;
using SimpleHRM.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleHRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageLeaveController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeesLeaveRepository _employeesLeave;

        public ManageLeaveController(IMapper mapper, IEmployeesLeaveRepository employeesLeave)
        {
           _mapper = mapper;
            _employeesLeave = employeesLeave;
        }
        /// <summary>
        /// Get a list of employees leave information
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            try
            {
                var objlist = await _employeesLeave.GetLeaves();
                var empleave = _mapper.Map<List<EmployeesLeaveDto>>(objlist);
                return Ok(empleave);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Get individual employee leave information with leaveId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeesLeaveDto>> GetLeave(int id)
        {
            try
            {
                var obj = await _employeesLeave.GetLeave(id);


                if (obj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                var employeeleave = _mapper.Map<EmployeesLeaveDto>(obj);
                return Ok(employeeleave);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Create individual employee leave request
        /// </summary>
        /// <param name="employeesLeaveCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLeave([FromQuery] EmployeesLeaveCreateDto employeesLeaveCreateDto)
        {
            try
            {
                if (employeesLeaveCreateDto == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                if (!_employeesLeave.EmployeeExists(employeesLeaveCreateDto.EmployeeId))
                {

                    return NotFound();
                }
                var employeeleave = _mapper.Map<EmployeesLeave>(employeesLeaveCreateDto);

                if (!await _employeesLeave.CreateEmployeeLeave(employeeleave))
                {

                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                var leave = _mapper.Map<EmployeesLeaveDto>(employeeleave);
                return CreatedAtAction("GetLeave", new { id = employeeleave.Id }, leave);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Update individual employee leave request
        /// </summary>
        /// <param name="employeesLeaveUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateLeave([FromQuery] EmployeesLeaveUpdateDto employeesLeaveUpdate)
        {
            try
            {
                if (employeesLeaveUpdate == null)
                {
                    return BadRequest(ModelState);
                }

                var empleaveobj = _mapper.Map<EmployeesLeave>(employeesLeaveUpdate);

                if (!await _employeesLeave.UpdateEmployeeLeave(empleaveobj))
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
        /// Delete individual employee leave request with leaveId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            try
            {
                if (!_employeesLeave.LeaveExists(id))
                {

                    return NotFound();
                }

                var empleaveobj = await _employeesLeave.GetLeave(id);

                if (!await _employeesLeave.DeleteEmployeeLeave(empleaveobj))
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
