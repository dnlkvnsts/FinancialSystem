
using FinancialSystem.Domain.Common;
using FinancialSystem.Domain.Exceptions;

namespace FinancialSystem.Domain.ValueObjects
{
    public  class Money : ValueObject
    {
        public  string Currency {  get; }
        public decimal Amount { get; }

        public Money(string currency, decimal amount)
        {
            if (amount < 0) throw new DomainException("Сумма не может быть отрицательной");
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;
        }

        public static Money operator +(Money left, Money right)
        {
            if(left.Currency != right.Currency) throw new DomainException("Валюты не совпадают");

            return new Money(left.Currency, left.Amount + right.Amount);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency) throw new DomainException("Валюты не совпадают");

            return new Money(left.Currency, left.Amount - right.Amount);
        }


    }
}
