using Microsoft.EntityFrameworkCore;
using SimpleHRM.DataAccess.Data;
using SimpleHRM.DataAccess.Repositories.IRepositories;
using SimpleHRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.DataAccess.Repositories
{
    public class EmployeesLeaveRepository : IEmployeesLeaveRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeesLeaveRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateEmployeeLeave(EmployeesLeave employeesLeave)
        {
            await _dbContext.EmployeesLeaves.AddAsync(employeesLeave);
            return await Save();
        }

        public async Task<bool> DeleteEmployeeLeave(EmployeesLeave employeesLeave)
        {
            _dbContext.EmployeesLeaves.Remove(employeesLeave);
            return await Save();
        }

        public bool EmployeeExists(int id)
        {
            return _dbContext.Employees.Any(a => a.Id == id);
        }
        public bool LeaveExists(int id)
        {
            return _dbContext.EmployeesLeaves.Any(a => a.Id == id);
        }
        public async Task<EmployeesLeave> GetLeave(int leaveid)
        {
            return await _dbContext.EmployeesLeaves.AsNoTracking().Include(c => c.Employee).FirstOrDefaultAsync(c=>c.Id== leaveid);
        }

        public async Task<ICollection<EmployeesLeave>> GetLeaves()
        {
            return _dbContext.EmployeesLeaves.AsNoTracking().Include(c=>c.Employee).OrderBy(p => p.Id).ToList();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> UpdateEmployeeLeave(EmployeesLeave employeesLeave)
        {
            _dbContext.EmployeesLeaves.Update(employeesLeave);
            return await Save();
        }
    }
}
