using FinancialSystem.Application.Features.Enterprises.Commands.CreatePayrollRequest;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Validators
{
    public class CreatePayrollRequestCommandValidator : AbstractValidator<CreatePayrollRequestCommand>
    {
        public CreatePayrollRequestCommandValidator()
        {
            RuleFor(v => v.ClientId)
                .GreaterThan(0).WithMessage("Некорректный ID клиента.");

            RuleFor(v => v.EnterpriseId)
                .GreaterThan(0).WithMessage("Необходимо выбрать предприятие.");
        }
    }
}
