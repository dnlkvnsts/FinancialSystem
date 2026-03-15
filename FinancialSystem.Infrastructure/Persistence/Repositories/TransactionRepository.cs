using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task<List<Transaction>> GetByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                .Include(t => t.FromAccount) // Загружаем объект отправителя
                .Include(t => t.ToAccount)   // Загружаем объект получателя
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .ToListAsync();
        }
    }
}
