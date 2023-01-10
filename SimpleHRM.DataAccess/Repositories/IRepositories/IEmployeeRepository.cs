using SimpleHRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.DataAccess.Repositories.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetEmployees();
        Task<Employee> GetEmployee(int employeeId);
        bool EmployeeExists(string name);
        bool EmployeeExists(int id);

        Task<bool> CreateEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        Task<bool> DeleteEmployee(Employee employee);
        Task<bool> Save();
    }
}
