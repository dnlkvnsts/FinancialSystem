using FinancialSystem.Application.Features.Accounts.Commands.OpenAccount;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Validators
{
    public class OpenAccountCommandValidator : AbstractValidator<OpenAccountCommand>
    {
        public OpenAccountCommandValidator()
        {
            RuleFor(v => v.BankId).GreaterThan(0).WithMessage("Нужно выбрать банк");
            RuleFor(v => v.Currency).NotEmpty().MaximumLength(3);
            RuleFor(v => v.OwnerId).GreaterThan(0);
        }
    }
}
