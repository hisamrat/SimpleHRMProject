using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.Models.Dto
{
   public class EmployeesLeaveCreateDto
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string LeaveType { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
