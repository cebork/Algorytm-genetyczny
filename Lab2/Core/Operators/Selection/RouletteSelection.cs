using Lab2.Core.Domain;
using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Selection
{
    public class RouletteSelection : ISelectionStrategy
    {
        private readonly IRandomProvider _random;

        public RouletteSelection(IRandomProvider random)
        {
            _random = random;
        }

        public IReadOnlyList<Individual> Select(IReadOnlyList<Individual> population)
        {
            int n = population.Count;

            // Build prefix sum once — O(n)
            decimal[] prefixSum = new decimal[n];
            prefixSum[0] = population[0].Fitness;
            for (int i = 1; i < n; i++)
                prefixSum[i] = prefixSum[i - 1] + population[i].Fitness;

            decimal total = prefixSum[n - 1];
            var selected = new List<Individual>(n);

            for (int i = 0; i < n; i++)
            {
                decimal r = (decimal)_random.NextDouble() * total;

                // Binary search — O(log n)
                int lo = 0, hi = n - 1;
                while (lo < hi)
                {
                    int mid = (lo + hi) / 2;
                    if (prefixSum[mid] < r)
                        lo = mid + 1;
                    else
                        hi = mid;
                }

                selected.Add(population[lo]);
            }

            return selected;
        }
    }
}
