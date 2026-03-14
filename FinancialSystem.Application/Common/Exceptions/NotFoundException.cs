using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        // Самый удобный конструктор: передаем имя сущности и её ID
        public NotFoundException(string name, object key)
            : base($"Сущность \"{name}\" ({key}) не найдена.") { }
    }
}
