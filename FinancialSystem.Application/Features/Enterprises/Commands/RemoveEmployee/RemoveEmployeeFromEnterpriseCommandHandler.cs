using FinancialSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.RemoveEmployee
{
    public class RemoveEmployeeFromEnterpriseCommandHandler : IRequestHandler<RemoveEmployeeFromEnterpriseCommand>
    {
        private readonly IApplicationDbContext _context;

        public RemoveEmployeeFromEnterpriseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveEmployeeFromEnterpriseCommand request, CancellationToken cancellationToken)
        {
            // 1. Ищем предприятие
            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(e => e.Id == request.EnterpriseId, cancellationToken);

            if (enterprise == null)
            {
                throw new Exception($"Предприятие с ID {request.EnterpriseId} не найдено");
            }

            // 2. Проверка: а является ли он вообще сотрудником?
            if (!enterprise.EmployeeIds.Contains(request.ClientId))
            {
                throw new Exception($"Клиент с ID {request.ClientId} не работает в этом предприятии.");
            }

            // 3. Вызываем доменный метод удаления
            enterprise.RemoveEmployee(request.ClientId);

            // 4. Сохраняем изменения
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
