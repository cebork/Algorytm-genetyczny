using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Mutation
{
    public class CompositeMutation : IMutationOperator
    {
        private readonly IReadOnlyList<IMutationOperator> _mutations;

        public CompositeMutation(params IMutationOperator[] mutations)
        {
            _mutations = mutations;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            foreach (var mutation in _mutations)
            {
                genotype = mutation.Mutate(genotype, iteration, maxIterations);
            }

            return genotype;
        }
    }
}
