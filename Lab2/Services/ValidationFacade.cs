using Lab2.Core.Domain;
using Lab2.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Services
{
    public sealed class ValidationFacade
    {
        private readonly InitialDataValidator _initialDataValidator = new();

        public void ValidateOrThrow(InitialData data)
        {
            var result = _initialDataValidator.Validate(data);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }
}
