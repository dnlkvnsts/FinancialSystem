using FinancialSystem.Domain.Aggregates;
using FinancialSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Bank> Banks { get; }
        DbSet<Account> Accounts { get; }
       
        DbSet<Transaction> Transactions { get; }
      
        DbSet<AuditLog> AuditLogs { get; } // Для требований Админа

        DbSet<Enterprise> Enterprises { get; }

        public DbSet<PayrollRequest> PayrollRequests { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
