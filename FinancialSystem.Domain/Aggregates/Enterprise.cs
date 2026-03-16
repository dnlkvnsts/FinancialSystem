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

        // ЗАМЕНИ Guid на int ТУТ
        private readonly List<int> _employeeIds = new();
        public IReadOnlyCollection<int> EmployeeIds => _employeeIds.AsReadOnly();

        public Enterprise(string name, string legalAddress)
        {
            Name = name;
            LegalAddress = legalAddress;
        }

        // И ТУТ ЗАМЕНИ Guid на int
        public void AddEmployee(int clientId)
        {
            if (!_employeeIds.Contains(clientId))
            {
                _employeeIds.Add(clientId);
            }
        }

      
    }
}
