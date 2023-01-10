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
        public bool CreateEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            return Save();
        }

        public bool DeleteEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public bool EmployeeExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool EmployeeExists(int id)
        {
            throw new NotImplementedException();
        }

        public Employee GetEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Employee> GetEmployees()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
