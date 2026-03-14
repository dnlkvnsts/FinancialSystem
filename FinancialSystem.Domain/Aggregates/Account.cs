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
    public  class Account : AggregateRoot
    {
        public string Number { get; private set; }
        public Money Balance { get; private set; }
        public bool IsBlocked { get; private set; }
        public int OwnerId { get; private set; }


        private Account() { }
        public Account(string number, Money initialBalance, int ownerId)
        {
            Number = number;
            Balance = initialBalance;
            OwnerId = ownerId;
        }   


        public void Withdraw(Money amount)
        {
            if (IsBlocked) throw new DomainException("Счет заблокирован");
            if (Balance.Amount < amount.Amount) throw new DomainException("Недостаточно средств на счету для снятия денег");

            Balance -= amount;

            AddDomainEvent(new MoneyTransferredEvent(Id, amount.Amount));
        }

        public void Block(string reason)
        {
            IsBlocked = true;
            AddDomainEvent(new AccountBlockedEvent(Id, reason));
        }


    }
}
