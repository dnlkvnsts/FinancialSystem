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
            // 1. Проверяем, существует ли предприятие
            var enterpriseExists = await _context.Enterprises
                .AnyAsync(e => e.Id == request.EnterpriseId, cancellationToken);

            if (!enterpriseExists)
            {
                throw new Exception($"Предприятие с ID {request.EnterpriseId} не найдено."); // Можно заменить на кастомный NotFoundException
            }

            // 2. Проверяем, нет ли уже ожидающей или одобренной заявки (бизнес-правило)
            var existingRequest = await _context.PayrollRequests
                .AnyAsync(pr => pr.ClientId == request.ClientId
                             && pr.EnterpriseId == request.EnterpriseId
                             && pr.Status != PayrollRequestStatus.Rejected, cancellationToken);

            if (existingRequest)
            {
                throw new Exception("Заявка на зарплатный проект в это предприятие уже существует или одобрена.");
            }

            // 3. Создаем сущность
            var entity = new PayrollRequest(request.ClientId, request.EnterpriseId);

            // 4. Сохраняем в БД
            _context.PayrollRequests.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
