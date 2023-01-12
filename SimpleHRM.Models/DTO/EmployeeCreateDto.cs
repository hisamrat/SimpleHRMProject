using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.Models.DTO
{
   public class EmployeeCreateDto
    {
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime JoiningDate { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public string Department { get; set; }
    }
}
