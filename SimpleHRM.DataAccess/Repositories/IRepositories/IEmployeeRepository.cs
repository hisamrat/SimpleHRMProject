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
        ICollection<Employee> GetEmployees();
        Employee GetEmployee(int employeeId);
        bool EmployeeExists(string name);
        bool EmployeeExists(int id);
        bool CreateEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        bool Save();
    }
}
