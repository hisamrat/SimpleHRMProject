using AutoMapper;
using SimpleHRM.Models;
using SimpleHRM.Models.Dto;
using SimpleHRM.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.Utility
{
   public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, EmployeeCreateDto>().ReverseMap();
            CreateMap<EmployeesLeave, EmployeesLeaveDto>().ReverseMap();
            CreateMap<EmployeesLeave, EmployeesLeaveCreateDto>().ReverseMap();
        }
    }
}
