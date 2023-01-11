using SimpleHRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.DataAccess.Repositories.IRepositories
{
  public  interface IEmployeesLeaveRepository
    {
        Task<ICollection<EmployeesLeave>> GetLeaves();
        Task<EmployeesLeave> GetLeave(int leaveid);
        bool EmployeeExists(int id);
        bool LeaveExists(int id);
        Task<bool> CreateEmployeeLeave(EmployeesLeave employeesLeave);
        Task<bool> UpdateEmployeeLeave(EmployeesLeave employeesLeave);
        Task<bool> DeleteEmployeeLeave(EmployeesLeave employeesLeave);
        Task<bool> Save();
    }
}
