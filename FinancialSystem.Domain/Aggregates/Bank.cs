using FinancialSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Aggregates
{
    public class Bank : AggregateRoot
    {
        public string Name { get; private set; }
        public string Bik { get; private set; } // Добавляем БИК, он важен для банков

        // 1. Пустой конструктор нужен для Entity Framework Core
        private Bank() { }

        // 2. Обновляем конструктор для создания банка
        public Bank(string name, string bik)
        {
            Name = name;
            Bik = bik;
        }

        private readonly List<int> _accountIds = new();

        // Используем .AsReadOnly() для безопасности
        public IReadOnlyCollection<int> AccountIds => _accountIds.AsReadOnly();

        public void RegisterAccount(int accountId) => _accountIds.Add(accountId);

    }
}
