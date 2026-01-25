using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Validation
{
    public sealed class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; } = new();

        public static ValidationResult Success()
            => new ValidationResult();

        public static ValidationResult Failure(params string[] errors)
        {
            var result = new ValidationResult();
            result.Errors.AddRange(errors);
            return result;
        }
    }
}
