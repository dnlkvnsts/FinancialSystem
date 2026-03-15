using FinancialSystem.Domain.Common;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Aggregates
{
    public class User : AggregateRoot
    {
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public RoleType Role { get; private set; }
        public bool IsConfirmed { get; private set; }

        // Пустой конструктор для EF Core
        private User() { }

        // Конструктор для создания нового пользователя (Регистрация)
        public User(string fullName, string email, string passwordHash, RoleType role)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            IsConfirmed = false;

            // Добавляем событие: пользователь зарегистрирован
            AddDomainEvent(new UserRegisteredEvent(this.Id, this.Email));
        }

        // Метод для подтверждения пользователя (Менеджером)
        public void Confirm()
        {
            if (!IsConfirmed)
            {
                IsConfirmed = true;
                // Добавляем событие: пользователь подтвержден
                AddDomainEvent(new UserConfirmedEvent(this.Id));
            }
        }
    }
}
