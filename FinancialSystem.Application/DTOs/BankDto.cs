using FinancialSystem.Application.Common.Mappers;
using FinancialSystem.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class BankDto : IMapFrom<Bank> // Указываем связь
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bik { get; set; } = string.Empty;
    }
}
