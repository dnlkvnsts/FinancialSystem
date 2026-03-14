using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Поиск по ID
        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // 2. Поиск всех счетов пользователя
        public async Task<IEnumerable<Account>> GetByUserIdAsync(int userId)
        {
            return await _context.Accounts
                .Where(x => x.OwnerId == userId)
                .ToListAsync();
        }

        // 3. Добавление нового счета
        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        // 4. Обновление (в EF Core достаточно просто пометить сущность как измененную)
        public void Update(Account account)
        {
            _context.Accounts.Update(account);
        }
    }
}
