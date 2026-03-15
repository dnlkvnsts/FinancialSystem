using FinancialSystem.Application.Features.Users.Commands.RegisterClient;
using FluentValidation;


namespace FinancialSystem.Application.Validators
{
    public class RegisterClientCommandValidator : AbstractValidator<RegisterClientCommand>
    {
        public RegisterClientCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Имя обязательно для заполнения.");

           
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен.")
                .EmailAddress().WithMessage("Неверный формат Email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен.")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов.");

          
        }
    }
}
