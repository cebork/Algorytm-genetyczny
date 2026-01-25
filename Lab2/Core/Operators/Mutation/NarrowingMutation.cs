using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Mutation
{
    public class NarrowingMutation : IMutationOperator
    {
        private readonly decimal _baseProbability;
        private readonly IRandomProvider _random;

        public NarrowingMutation(decimal baseProbability, IRandomProvider random)
        {
            _baseProbability = baseProbability;
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            decimal multiplier = Math.Max(
                0.000m,
                (maxIterations - iteration) / (decimal)maxIterations
            );

            decimal effectiveProb = _baseProbability * multiplier;
            if (effectiveProb <= 0)
                return genotype;

            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);

            bool[,] result = (bool[,])genotype.Clone();

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (_random.NextDouble() <= (double)effectiveProb)
                    {
                        result[i, j] = !result[i, j];
                    }
                }
            }

            return result;
        }
    }
}
