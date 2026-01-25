using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Validation
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T model);
    }
}
