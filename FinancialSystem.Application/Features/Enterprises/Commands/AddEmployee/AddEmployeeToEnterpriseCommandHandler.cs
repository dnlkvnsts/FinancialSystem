using FinancialSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinancialSystem.Application.Features.Enterprises.Commands.AddEmployee
{
    public class AddEmployeeToEnterpriseCommandHandler : IRequestHandler<AddEmployeeToEnterpriseCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddEmployeeToEnterpriseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddEmployeeToEnterpriseCommand request, CancellationToken cancellationToken)
        {
            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(e => e.Id == request.EnterpriseId, cancellationToken);

            if (enterprise == null)
            {
                throw new Exception($"Предприятие с ID {request.EnterpriseId} не найдено");
            }

            // ПРОВЕРКА: если сотрудник уже есть в списке
            if (enterprise.EmployeeIds.Contains(request.ClientId))
            {
                // Выбрасываем исключение с текстом, который хотим увидеть
                throw new Exception($"Клиент с ID {request.ClientId} уже является сотрудником этого предприятия!");
            }

            enterprise.AddEmployee(request.ClientId);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
