using FinancialSystem.Application.Features.Accounts.Commands.CreateAccount;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Validators
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            // Правило для OwnerId: должен быть больше нуля
            RuleFor(x => x.OwnerId)
                .GreaterThan(0).WithMessage("ID владельца счета должен быть больше 0.");

            // Правило для валюты: не пустая и ровно 3 символа (например, USD, RUB, EUR)
            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Валюта обязательна для заполнения.")
                .Length(3).WithMessage("Код валюты должен состоять ровно из 3 символов.");

            // Правило для номера счета: не пустой и, например, от 10 до 20 символов
            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Номер счета обязателен.")
                .MinimumLength(10).WithMessage("Номер счета слишком короткий.")
                .MaximumLength(20).WithMessage("Номер счета слишком длинный.");
        }
    }
}
