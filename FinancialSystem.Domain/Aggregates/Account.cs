using FinancialSystem.Domain.Common;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Events;
using FinancialSystem.Domain.Exceptions;
using FinancialSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinancialSystem.Domain.Aggregates
{
    public class Account : AggregateRoot
    {
        public string Number { get; private set; }
        public Money Balance { get; private set; }
        public AccountType Type { get; private set; } // ДОБАВИЛИ: Debit, Credit, Savings
        public bool IsBlocked { get; private set; }
        public bool IsClosed { get; private set; }

        public int OwnerId { get; private set; }
        public int BankId { get; private set; }

        private Account() { }

        // ОБНОВЛЕННЫЙ КОНСТРУКТОР (Пункт "Открытие")
        public Account(string number, Money initialBalance, int ownerId, int bankId, AccountType type)
        {
            Number = number;
            Balance = initialBalance;
            OwnerId = ownerId;
            BankId = bankId;
            Type = type; // Указываем, Счет это или Вклад
            IsClosed = false;
            IsBlocked = false;
        }

        // Пункт "Перевод": Метод для зачисления
        public void Deposit(Money amount)
        {
            if (IsClosed) throw new DomainException("Счет закрыт");
            if (IsBlocked) throw new DomainException("Счет заблокирован");

            // Логика "На вклад": Если это вклад (Savings), можно добавить лог или бонус
            if (Type == AccountType.Savings)
            {
                // Тут может быть специфичная логика для вкладов
            }

            Balance += amount;
        }

        // Пункт "Перевод": Метод для списания
        public void Withdraw(Money amount)
        {
            if (IsClosed) throw new DomainException("Счет закрыт");
            if (IsBlocked) throw new DomainException("Счет заблокирован");

            // Если это вклад (Savings), часто банки запрещают частичное снятие
            if (Type == AccountType.Savings && Balance.Amount < amount.Amount)
                throw new DomainException("Нельзя снимать средства с вклада, если их недостаточно (вклады часто не подразумевают овердрафт)");

            if (Balance.Amount < amount.Amount)
                throw new DomainException("Недостаточно средств на счету");

            Balance -= amount;
        }

        // Пункт "Закрытие"
        public void Close()
        {
            if (IsClosed) throw new DomainException("Этот счет уже закрыт.");
            if (Balance.Amount > 0) throw new DomainException("Сначала выведите остаток средств.");

            IsClosed = true;
        }
    }

}
