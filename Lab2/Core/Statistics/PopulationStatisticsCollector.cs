using Lab2.Core.Domain;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Statistics
{
    public sealed class PopulationStatisticsCollector
        : IPopulationStatisticsProvider
    {
        public PopulationStatistics Current { get; private set; }

        public void Update(
            IReadOnlyList<Individual> population,
            int generation
        )
        {
            var fitness = population
            .Select(i => (double)i.Fitness)
            .ToArray();

            Current = new PopulationStatistics
            {
                Generation = generation,
                BestFitness = (decimal)fitness.Max(),
                WorstFitness = (decimal)fitness.Min(),
                AverageFitness = (decimal)fitness.Average(),
                FitnessStdDev = (decimal)fitness.StandardDeviation(),
                Diversity = CalculateDiversity(population)
            };
        }

        private static double CalculateDiversity(
            IReadOnlyList<Individual> population
        )
        {
            int size = population.Count;
            if (size < 2) return 0;

            int totalGenes = 0;
            int differentGenes = 0;

            var reference = population[0].Genotype;

            for (int i = 1; i < size; i++)
            {
                var matrix = population[i].Genotype;

                for (int r = 0; r < reference.GetLength(0); r++)
                {
                    for (int c = 0; c < reference.GetLength(1); c++)
                    {
                        totalGenes++;
                        if (matrix[r, c] != reference[r, c])
                            differentGenes++;
                    }
                }
            }

            return totalGenes == 0
                ? 0
                : (double)differentGenes / totalGenes;
        }
    }
}
