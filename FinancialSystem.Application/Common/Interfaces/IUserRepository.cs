using FinancialSystem.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);

        // Для менеджера: список тех, кто ждет одобрения
        Task<IEnumerable<User>> GetPendingUsersAsync();

        void Update(User user);
    }
}
