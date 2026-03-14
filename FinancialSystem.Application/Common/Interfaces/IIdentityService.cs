using FinancialSystem.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{

    public interface IIdentityService
    {
        // Проверка логина/пароля
        Task<(bool Result, string userId, string role)> AuthenticateAsync(string username, string password);

        // Регистрация нового клиента (со статусом IsConfirmed = false)
        Task<(bool Result, string userId)> RegisterAsync(string username, string password, string fullName);

        // Проверка, разрешен ли вход (подтвержден ли менеджер)
        Task<bool> IsConfirmedAsync(string userId);
    }
}
