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
        private readonly double _earlyExplorationMultiplier;
        private readonly double _lateExplorationMultiplier;

        public BitFlipMutation(
            decimal mutationProbability,
            decimal earlyExplorationMultiplier,
            decimal lateExplorationMultiplier,
            IRandomProvider random)
        {
            _mutationProbability = mutationProbability;
            _earlyExplorationMultiplier = Math.Max(0.0, (double)earlyExplorationMultiplier);
            _lateExplorationMultiplier = Math.Max(0.0, (double)lateExplorationMultiplier);
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            if (_mutationProbability <= 0)
                return genotype;

            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);
            double effectiveProbability = CalculateEffectiveProbability(iteration, maxIterations);

            for (int row = 1; row < rows - 1; row++)
            {
                for (int col = 1; col < cols - 1; col++)
                {
                    if (_random.NextDouble() <= effectiveProbability)
                        genotype[row, col] = !genotype[row, col];
                }
            }

            return genotype;
        }

        private double CalculateEffectiveProbability(int iteration, int maxIterations)
        {
            double progress = maxIterations <= 0
                ? 0.0
                : Math.Clamp(iteration / (double)maxIterations, 0.0, 1.0);

            double multiplier = _earlyExplorationMultiplier -
                (_earlyExplorationMultiplier - _lateExplorationMultiplier) * progress;

            double probability = (double)_mutationProbability * multiplier;
            return Math.Clamp(probability, 0.0, 1.0);
        }
    }
}
