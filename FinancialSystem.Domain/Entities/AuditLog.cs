using FinancialSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Entities
{
    public class AuditLog : Entity
    {
        public int UserId { get; set; }

        public string Action { get; set; }

        public string Details { get; set; }

        // ДОБАВЬ ЭТО:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsReverted { get; set; } = false;
    }
}
