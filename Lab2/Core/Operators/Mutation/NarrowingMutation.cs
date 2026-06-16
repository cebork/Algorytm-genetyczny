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
        private readonly decimal _startMultiplier;
        private readonly int _narrowingStep;
        private readonly IRandomProvider _random;

        public NarrowingMutation(
            decimal baseProbability,
            decimal startMultiplier,
            int narrowingStep,
            IRandomProvider random)
        {
            _baseProbability = baseProbability;
            _startMultiplier = startMultiplier;
            _narrowingStep = narrowingStep;
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            int step = Math.Max(1, _narrowingStep);
            int totalSteps = Math.Max(1, (int)Math.Ceiling(maxIterations / (decimal)step));
            int currentStep = Math.Min(totalSteps, iteration / step);

            decimal progress = currentStep / (decimal)totalSteps;
            decimal multiplier = Math.Max(0m, _startMultiplier * (1m - progress));
            decimal effectiveProb = Math.Min(1m, _baseProbability * multiplier);
            if (effectiveProb <= 0)
                return genotype;

            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (_random.NextDouble() <= (double)effectiveProb)
                    {
                        genotype[i, j] = !genotype[i, j];
                    }
                }
            }

            return genotype;
        }
    }
}
