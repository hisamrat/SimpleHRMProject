using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private const string LeaveListCacheKey = "leaveList";
        private readonly IMapper _mapper;
        private readonly IEmployeesLeaveRepository _employeesLeave;
        private IMemoryCache _cache;
        public ManageLeaveController(IMemoryCache cache, IMapper mapper, IEmployeesLeaveRepository employeesLeave)
        {
            _cache = cache;
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
                if (!_cache.TryGetValue(LeaveListCacheKey, out List<EmployeesLeaveDto> leaveList))
                {
                    var objlist = await _employeesLeave.GetLeaves();
                    leaveList = _mapper.Map<List<EmployeesLeaveDto>>(objlist);
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                       
                    };
                    _cache.Set(LeaveListCacheKey, leaveList, cacheExpiryOptions);
                }
                return Ok(leaveList);
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
                _cache.Remove(LeaveListCacheKey);
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
                if (!_employeesLeave.LeaveExists(employeesLeaveUpdate.Id))
                {
                    return NotFound();
                }
                var empleaveobj = _mapper.Map<EmployeesLeave>(employeesLeaveUpdate);

                if (!await _employeesLeave.UpdateEmployeeLeave(empleaveobj))
                {

                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                _cache.Remove(LeaveListCacheKey);
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
                _cache.Remove(LeaveListCacheKey);
                return NoContent();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
    }
}
