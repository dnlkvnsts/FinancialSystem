using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class EnterpriseWithEmployeesDto
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
        public string LegalAddress { get; set; } = null!;
        public List<EmployeeDto> Employees { get; set; } = new();
    }
}
