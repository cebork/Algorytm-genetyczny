using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Mutation
{
    public class UniformBlockMutation : IMutationOperator
    {
        private readonly decimal _probWhite;
        private readonly decimal _probRed;
        private readonly IRandomProvider _random;

        public UniformBlockMutation(
            decimal probWhite,
            decimal probRed,
            IRandomProvider random
        )
        {
            _probWhite = probWhite;
            _probRed = probRed;
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);
            var cellsToFlip = new List<(int Row, int Col)>();

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    bool center = genotype[i, j];
                    bool uniform = true;

                    for (int di = -1; di <= 1 && uniform; di++)
                    {
                        for (int dj = -1; dj <= 1 && uniform; dj++)
                        {
                            if (genotype[i + di, j + dj] != center)
                                uniform = false;
                        }
                    }

                    if (!uniform)
                        continue;

                    decimal prob = center ? _probRed : _probWhite;
                    if (_random.NextDouble() <= (double)prob)
                    {
                        cellsToFlip.Add((i, j));
                    }
                }
            }

            foreach (var cell in cellsToFlip)
            {
                genotype[cell.Row, cell.Col] = !genotype[cell.Row, cell.Col];
            }

            return genotype;
        }
    }
}
