using FinancialSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Aggregates
{
    public class Enterprise : AggregateRoot
    {
        public string Name { get; private set; }
        public string LegalAddress { get; private set; }

        // Список ID пользователей-сотрудников
        private readonly List<Guid> _employeeIds = new();
        public IReadOnlyCollection<Guid> EmployeeIds => _employeeIds.AsReadOnly();

        // Пустой конструктор для EF Core (обязательно, если используешь базу данных)
        private Enterprise() { }

        public Enterprise(string name, string legalAddress)
        {
            
            Name = name;
            LegalAddress = legalAddress;
        }

        public void AddEmployee(Guid clientId)
        {
            if (!_employeeIds.Contains(clientId))
            {
                _employeeIds.Add(clientId);

                // Если захочешь, здесь можно добавить событие:
                // AddDomainEvent(new EmployeeAddedEvent(this.Id, clientId));
            }
        }
    }
}
