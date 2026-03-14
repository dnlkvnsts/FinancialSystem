using FinancialSystem.Domain.Common;
using FinancialSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Aggregates
{
    public class User : AggregateRoot
    {
        public string FullName { get; private set; }
        public RoleType Role { get; private set; }
        public bool IsConfirmed {  get; private set; }


        public User(string fullName, RoleType role)
        {
            FullName = fullName;
            Role = role;
            IsConfirmed = false;
        }

        public void Confirm() => IsConfirmed = true;

    }
}
