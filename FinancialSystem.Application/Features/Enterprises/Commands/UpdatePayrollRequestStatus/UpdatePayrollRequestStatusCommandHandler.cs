using FinancialSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.UpdatePayrollRequestStatus
{
    public class UpdatePayrollRequestStatusCommandHandler : IRequestHandler<UpdatePayrollRequestStatusCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePayrollRequestStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePayrollRequestStatusCommand request, CancellationToken cancellationToken)
        {
            // 1. Ищем заявку в базе
            var entity = await _context.PayrollRequests
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new Exception($"Заявка с ID {request.Id} не найдена");
            }

            // 2. Обновляем статус
            entity.UpdateStatus(request.Status);

            // 3. Сохраняем изменения
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
