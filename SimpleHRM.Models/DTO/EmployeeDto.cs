using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.Models.DTO
{
   public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }
}
