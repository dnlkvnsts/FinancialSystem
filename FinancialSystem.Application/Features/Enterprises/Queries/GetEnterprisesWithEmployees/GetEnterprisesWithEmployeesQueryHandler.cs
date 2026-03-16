using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Enterprises.Queries.GetEnterprisesWithEmployee;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinancialSystem.Application.Features.Enterprises.Queries.GetEnterprisesWithEmployees
{
    public class GetEnterprisesWithEmployeesHandler : IRequestHandler<GetEnterprisesWithEmployeesQuery, List<EnterpriseWithEmployeesDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetEnterprisesWithEmployeesHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<EnterpriseWithEmployeesDto>> Handle(GetEnterprisesWithEmployeesQuery request, CancellationToken cancellationToken)
        {
            // 1. Получаем все предприятия из БД
            var enterprises = await _context.Enterprises.ToListAsync(cancellationToken);

            // 2. Собираем все уникальные Guid сотрудников
            var allEmployeeIds = enterprises
                .SelectMany(e => e.EmployeeIds)
                .Distinct()
                .ToList();

            // 3. Загружаем данные пользователей (проверь, как называется таблица: Users или Clients)
            var users = await _context.Users
                .Where(u => allEmployeeIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u, cancellationToken);

            // 4. Маппим в DTO
            return enterprises.Select(e => new EnterpriseWithEmployeesDto
            {
                Id = e.Id, // int
                Name = e.Name,
                LegalAddress = e.LegalAddress,
                Employees = e.EmployeeIds
                    .Where(id => users.ContainsKey(id))
                    .Select(id => new EmployeeDto
                    {
                        Id = id, // Guid
                        FullName = users[id].FullName,
                        Email = users[id].Email
                    }).ToList()
            }).ToList();
        }
    }
}
