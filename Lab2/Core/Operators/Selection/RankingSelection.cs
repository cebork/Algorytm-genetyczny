using Lab2.Core.Domain;
using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Core.Operators.Selection
{
    public class RankingSelection : ISelectionStrategy
    {
        private readonly IRandomProvider _random;

        public RankingSelection(IRandomProvider random)
        {
            _random = random;
        }

        public IReadOnlyList<Individual> Select(IReadOnlyList<Individual> population)
        {
            if (population == null || population.Count == 0)
                throw new ArgumentException("Population is empty.");

            int n = population.Count;
            var ranked = population
                .OrderBy(individual => individual.Fitness)
                .ToArray();

            decimal[] prefixSum = new decimal[n];
            prefixSum[0] = 1;
            for (int i = 1; i < n; i++)
                prefixSum[i] = prefixSum[i - 1] + i + 1;

            decimal total = prefixSum[n - 1];
            var selected = new List<Individual>(n);

            for (int i = 0; i < n; i++)
            {
                decimal r = (decimal)_random.NextDouble() * total;

                int lo = 0;
                int hi = n - 1;
                while (lo < hi)
                {
                    int mid = (lo + hi) / 2;
                    if (prefixSum[mid] < r)
                        lo = mid + 1;
                    else
                        hi = mid;
                }

                selected.Add(ranked[lo]);
            }

            return selected;
        }
    }
}