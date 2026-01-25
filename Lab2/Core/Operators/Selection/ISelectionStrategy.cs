using Lab2.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Selection
{
    public interface ISelectionStrategy
    {
        IReadOnlyList<Individual> Select(IReadOnlyList<Individual> population);
    }
}
