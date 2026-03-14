using FinancialSystem.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{
     public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id);
        
        Task<IEnumerable<Account>> GetByUserIdAsync(int userId);
        
        Task AddAsync(Account account);
        
        void Update(Account account);

       
    }
}
