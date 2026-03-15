using FinancialSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);

        Task<List<Transaction>> GetByAccountIdAsync(int accountId);
    }
}
