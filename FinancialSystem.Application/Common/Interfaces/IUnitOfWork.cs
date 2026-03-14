using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        // Сохраняет все изменения в рамках одной транзакции
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // Управление транзакциями (атомарность)
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
