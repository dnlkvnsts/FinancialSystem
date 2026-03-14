using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("Произошла одна или несколько ошибок валидации.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("Произошла одна или несколько ошибок валидации.")
        {
            Errors = errors;
        }

        public IReadOnlyDictionary<string, string[]> Errors { get; }
    }
}
