using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Mutation
{
    public interface IMutationOperator
    {
        bool[,] Mutate(
            bool[,] genotype,
            int iteration,
            int maxIterations
        );
    }
}
