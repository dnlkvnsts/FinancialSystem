using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Behaviors
{
    // Реализуем интерфейс IPipelineBehavior от MediatR
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        // Сюда автоматически инжектятся все валидаторы для конкретного TRequest
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Если для данного запроса есть валидаторы
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // Запускаем все валидаторы параллельно
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // Собираем все ошибки в один список
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                // Если есть хотя бы одна ошибка, выбрасываем исключение
                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            // Если ошибок нет (или валидаторов нет), передаем запрос дальше по конвейеру (в Handler)
            return await next();
        }
    }
}
