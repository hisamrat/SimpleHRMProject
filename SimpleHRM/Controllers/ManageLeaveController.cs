using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHRM.DataAccess.Data;
using SimpleHRM.DataAccess.Repositories.IRepositories;
using SimpleHRM.Models;
using SimpleHRM.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleHRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageLeaveController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmployeesLeaveRepository _employeesLeave;

        public ManageLeaveController(IMapper mapper, ApplicationDbContext dbContext, IEmployeesLeaveRepository employeesLeave)
        {
           _mapper = mapper;
            _dbContext = dbContext;
            _employeesLeave = employeesLeave;
        }

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

        [HttpGet("{leaveId:int}")]
        public async Task<ActionResult<EmployeesLeaveDto>> GetLeave(int leaveId)
        {
            try
            {
                var obj = await _employeesLeave.GetLeave(leaveId);


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
                return CreatedAtAction("GetLeave", new { leaveId = employeeleave.Id }, employeeleave);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
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
        [HttpDelete("{leaveId:int}")]
        public async Task<IActionResult> DeleteLeave(int leaveId)
        {
            try
            {
                if (!_employeesLeave.LeaveExists(leaveId))
                {

                    return NotFound();
                }

                var empleaveobj = await _employeesLeave.GetLeave(leaveId);

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
