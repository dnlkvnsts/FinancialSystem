using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; } // Было Guid, стало int
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
