using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Mutation
{
    public class BitFlipMutation : IMutationOperator
    {
        private readonly decimal _mutationProbability;
        private readonly IRandomProvider _random;

        public BitFlipMutation(decimal mutationProbability, IRandomProvider random)
        {
            _mutationProbability = mutationProbability;
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (_random.NextDouble() <= (double)_mutationProbability)
                    {
                        genotype[i, j] = !genotype[i, j];
                    }
                }
            }

            return genotype;
        }
    }
}
