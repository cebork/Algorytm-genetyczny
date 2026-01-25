using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Crossover
{
    public interface ICrossoverOperator
    {
        (bool[,] Child1, bool[,] Child2) Cross(
            bool[,] parent1,
            bool[,] parent2
        );
    }
}
