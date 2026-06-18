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
        private readonly decimal _asymptoticExponent;
        private readonly bool _useLowerLimit;
        private readonly IRandomProvider _random;

        public NarrowingMutation(
            decimal baseProbability,
            decimal startMultiplier,
            int narrowingStep,
            decimal asymptoticExponent,
            bool useLowerLimit,
            IRandomProvider random)
        {
            _baseProbability = baseProbability;
            _startMultiplier = startMultiplier;
            _narrowingStep = narrowingStep;
            _asymptoticExponent = asymptoticExponent;
            _useLowerLimit = useLowerLimit;
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            decimal effectiveProb = CalculateEffectiveProbability(
                _baseProbability,
                _startMultiplier,
                _narrowingStep,
                _asymptoticExponent,
                _useLowerLimit,
                iteration,
                maxIterations
            );

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

        public static decimal CalculateEffectiveProbability(
            decimal baseProbability,
            decimal startMultiplier,
            int narrowingStep,
            decimal asymptoticExponent,
            bool useLowerLimit,
            int iteration,
            int maxIterations)
        {
            int step = Math.Max(1, narrowingStep);
            int totalIterations = Math.Max(1, maxIterations);
            int currentStep = Math.Min(totalIterations, Math.Max(0, iteration / step * step));
            decimal remainingRatio = Math.Max(0m, (totalIterations - currentStep) / (decimal)totalIterations);
            decimal narrowingFactor = (decimal)Math.Pow((double)remainingRatio, (double)asymptoticExponent);

            decimal effectiveProb = useLowerLimit
                ? baseProbability * (startMultiplier - 1m) * narrowingFactor + baseProbability
                : baseProbability * startMultiplier * narrowingFactor;

            return Math.Min(1m, Math.Max(0m, effectiveProb));
        }
    }
}
