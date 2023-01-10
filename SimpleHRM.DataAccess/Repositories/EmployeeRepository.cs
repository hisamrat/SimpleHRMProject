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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateEmployee(Employee employee)
        {
          await  _dbContext.Employees.AddAsync(employee);
            return await Save();
        }

        public async Task<bool> DeleteEmployee(Employee employee)
        {
             _dbContext.Employees.Remove(employee);
            return await Save();
        }

        public bool EmployeeExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool EmployeeExists(int id)
        {
            return _dbContext.Employees.Any(a => a.Id == id);
        }

        public async Task<Employee>  GetEmployee(int employeeId)
        {
          return  await _dbContext.Employees.FindAsync(employeeId);
        }

        public async Task<ICollection<Employee>> GetEmployees()
        {
            return  _dbContext.Employees.AsNoTracking().OrderBy(p => p.Id).ToList();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
             _dbContext.Employees.Update(employee);
            return await Save();
        }
    }
}
