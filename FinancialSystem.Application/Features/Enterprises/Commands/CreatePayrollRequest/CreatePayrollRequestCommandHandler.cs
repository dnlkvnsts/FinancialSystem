using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Aggregates;
using FinancialSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.CreatePayrollRequest
{
    public class CreatePayrollRequestCommandHandler : IRequestHandler<CreatePayrollRequestCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePayrollRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePayrollRequestCommand request, CancellationToken cancellationToken)
        {
            // 1. Загружаем объект предприятия (а не просто проверяем AnyAsync)
            // Это нужно, чтобы EF Core начал отслеживать изменения в этом объекте
            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(e => e.Id == request.EnterpriseId, cancellationToken);

            if (enterprise == null)
            {
                throw new Exception($"Предприятие с ID {request.EnterpriseId} не найдено.");
            }

            // 2. Проверяем, нет ли уже ожидающей или одобренной заявки
            var existingRequest = await _context.PayrollRequests
                .AnyAsync(pr => pr.ClientId == request.ClientId
                             && pr.EnterpriseId == request.EnterpriseId
                             && pr.Status != PayrollRequestStatus.Rejected, cancellationToken);

            if (existingRequest)
            {
                throw new Exception("Заявка на зарплатный проект в это предприятие уже существует или одобрена.");
            }

            // --- ЛОГИКА ПРИСОЕДИНЕНИЯ ---
            // 3. Добавляем клиента в список сотрудников предприятия
            enterprise.AddEmployee(request.ClientId);

            // 4. Создаем сущность заявки
            var entity = new PayrollRequest(request.ClientId, request.EnterpriseId);

            // 5. Сохраняем всё в БД
            // EF Core увидит, что мы изменили enterprise и добавили новую заявку,
            // и выполнит оба действия в одной транзакции.
            _context.PayrollRequests.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
