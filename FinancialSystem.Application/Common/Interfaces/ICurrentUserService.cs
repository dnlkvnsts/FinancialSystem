using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        // ID текущего пользователя (может быть null, если не вошел)
        string? UserId { get; }

        // Роль: "Client", "Manager", "Admin"
        string? Role { get; }

        bool IsAuthenticated { get; }
    }
}
