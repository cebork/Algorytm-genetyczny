using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Mutation
{
    public class BitSwapMutation : IMutationOperator
    {
        private readonly decimal _mutationProbability;
        private readonly IRandomProvider _random;

        public BitSwapMutation(decimal mutationProbability, IRandomProvider random)
        {
            _mutationProbability = mutationProbability;
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            if (_random.NextDouble() > (double)_mutationProbability)
                return genotype;

            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);

            int innerRows = rows - 2;
            int innerCols = cols - 2;
            int totalInnerBits = innerRows * innerCols;

            if (totalInnerBits < 4)
                return genotype;

            int maxLength = Math.Min(6, totalInnerBits / 4);
            int segmentLength = _random.Next(2, maxLength + 1);

            int maxStart = totalInnerBits - 2 * segmentLength;
            if (maxStart <= 0)
                return genotype;

            int firstStart = _random.Next(0, maxStart);
            int secondStart = _random.Next(firstStart + segmentLength, totalInnerBits - segmentLength);

            bool[] flat = new bool[totalInnerBits];
            for (int i = 0; i < innerRows; i++)
            {
                for (int j = 0; j < innerCols; j++)
                {
                    flat[i * innerCols + j] = genotype[i + 1, j + 1];
                }
            }

            for (int k = 0; k < segmentLength; k++)
            {
                (flat[firstStart + k], flat[secondStart + k]) =
                    (flat[secondStart + k], flat[firstStart + k]);
            }

            for (int i = 0; i < totalInnerBits; i++)
            {
                int r = i / innerCols;
                int c = i % innerCols;
                genotype[r + 1, c + 1] = flat[i];
            }

            return genotype;
        }
    }
}
