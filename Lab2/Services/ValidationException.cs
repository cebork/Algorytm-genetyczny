using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Services
{
    public sealed class ValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base(string.Join(Environment.NewLine, errors))
        {
            Errors = errors.ToList();
        }
    }
}
