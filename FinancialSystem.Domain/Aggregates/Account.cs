using FinancialSystem.Domain.Common;
using FinancialSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinancialSystem.Domain.Exceptions;
using FinancialSystem.Domain.Events;


namespace FinancialSystem.Domain.Aggregates
{
    public class Account : AggregateRoot
    {
        public string Number { get; private set; }
        public Money Balance { get; private set; }
        public bool IsBlocked { get; private set; }
        public bool IsClosed { get; private set; } // Добавили для реализации "Закрытия"

        public int OwnerId { get; private set; }
        public int BankId { get; private set; }    // Добавили связь с банком

        private Account() { }

        public Account(string number, Money initialBalance, int ownerId, int bankId)
        {
            Number = number;
            Balance = initialBalance;
            OwnerId = ownerId;
            BankId = bankId;
            IsClosed = false;
            IsBlocked = false;
        }

        // Метод для зачисления денег (нужен для переводов и зарплаты)
        public void Deposit(Money amount)
        {
            if (IsClosed) throw new DomainException("Счет закрыт");
            if (IsBlocked) throw new DomainException("Счет заблокирован");

            Balance += amount; // Убедись, что в Money перегружен оператор +
        }

        public void Withdraw(Money amount)
        {
            if (IsClosed) throw new DomainException("Счет закрыт");
            if (IsBlocked) throw new DomainException("Счет заблокирован");
            if (Balance.Amount < amount.Amount)
                throw new DomainException("Недостаточно средств на счету");

            Balance -= amount;

            // Можно создать более общее событие для истории
            AddDomainEvent(new MoneyTransferredEvent(Id, amount.Amount));
        }

        public void Block(string reason)
        {
            IsBlocked = true;
            AddDomainEvent(new AccountBlockedEvent(Id, reason));
        }

        public void Close()
        {
            // 1. Проверяем, не закрыт ли счет уже
            if (IsClosed)
            {
                throw new DomainException("Этот счет уже закрыт. Повторное закрытие невозможно.");
            }

            // 2. Проверяем баланс
            if (Balance.Amount > 0)
            {
                throw new DomainException("Нельзя закрыть счет с положительным балансом. Сначала выведите деньги.");
            }

            // 3. Если проверки прошли, закрываем
            IsClosed = true;
        }
    }

}
