using AutoMapper;
using FinancialSystem.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 1. Регистрируем MediatR (если ты этого еще не сделал)
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // 2. Автоматически находим и регистрируем ВСЕ валидаторы (например, твой CreateAccountCommandValidator)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. Добавляем наш "перехватчик" (ValidationBehavior) в конвейер MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
