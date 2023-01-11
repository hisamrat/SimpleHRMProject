using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHRM.Models.DTO
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        public int recordsPerPage = 10;
        public readonly int maxRecordsPerPage = 50;
        public int RecordsPerPage
        {

            get { return recordsPerPage; }

            set
            {
                recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }

    }
}
