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
        public AccountType Type { get; private set; }
        public bool IsBlocked { get; private set; }
        public bool IsClosed { get; private set; }

        // НОВОЕ ПОЛЕ: Процентная ставка (например, 0.10 для 10%)
        public decimal InterestRate { get; private set; }

        public int OwnerId { get; private set; }
        public int BankId { get; private set; }

        private Account() { }

        // ОБНОВЛЕННЫЙ КОНСТРУКТОР: Добавляем interestRate
        public Account(string number, Money initialBalance, int ownerId, int bankId, AccountType type, decimal interestRate = 0)
        {
            Number = number;
            Balance = initialBalance;
            OwnerId = ownerId;
            BankId = bankId;
            Type = type;
            InterestRate = interestRate; // Сохраняем ставку
            IsClosed = false;
            IsBlocked = false;
        }

        // РЕАЛИЗАЦИЯ ПУНКТА "НАКОПЛЕНИЕ": Метод для начисления процентов
        public void AccrueInterest()
        {
            if (Type != AccountType.Savings)
                throw new DomainException("Накопление процентов доступно только для вкладов.");

            if (IsClosed)
                throw new DomainException("Нельзя начислить проценты на закрытый вклад.");

            // Пример: ежемесячное начисление (годовая ставка / 12 месяцев)
            decimal monthlyRate = InterestRate / 12 / 100;
            decimal interestAmount = Balance.Amount * monthlyRate;

            if (interestAmount > 0)
            {
                var interestMoney = new Money(Balance.Currency, interestAmount);
                // Используем уже созданный метод Deposit для пополнения
                Deposit(interestMoney);
            }
        }

        // Метод для блокировки (нужен для ТЗ менеджера)
        public void Block() => IsBlocked = true;
        public void Unblock() => IsBlocked = false;

        // Пункт "Перевод": Метод для зачисления
        public void Deposit(Money amount)
        {
            if (IsClosed) throw new DomainException("Счет закрыт");
            if (IsBlocked) throw new DomainException("Счет заблокирован");

            Balance += amount;
        }

        // Пункт "Перевод": Метод для списания
        public void Withdraw(Money amount)
        {
            if (IsClosed) throw new DomainException("Счет закрыт");
            if (IsBlocked) throw new DomainException("Счет заблокирован");

            if (Balance.Amount < amount.Amount)
                throw new DomainException("Недостаточно средств на счету");

            Balance -= amount;
        }

        // Пункт "Закрытие"
        public void Close()
        {
            if (IsClosed) throw new DomainException("Этот счет уже закрыт.");
            // По ТЗ обычно нельзя закрыть счет, если там остались деньги
            if (Balance.Amount > 0) throw new DomainException("Сначала выведите остаток средств (баланс должен быть 0).");

            IsClosed = true;
        }

        
    }


}
