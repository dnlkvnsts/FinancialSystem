using FinancialSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{

    public class PayrollRequestDto
    {
        public int Id { get; set; }
        public int EnterpriseId { get; set; }
       
        public string EnterpriseName { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
