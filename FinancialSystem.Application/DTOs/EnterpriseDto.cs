using AutoMapper;
using FinancialSystem.Application.Common.Mappers;
using FinancialSystem.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class EnterpriseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LegalAddress { get; set; } = string.Empty;
    }
}
