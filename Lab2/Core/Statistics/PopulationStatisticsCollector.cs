using Lab2.Core.Domain;
using System;
using System.Collections.Generic;

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
            int n = population.Count;
            double min = double.MaxValue;
            double max = double.MinValue;
            double mean = 0.0;
            double m2 = 0.0;

            for (int i = 0; i < n; i++)
            {
                double f = (double)population[i].Fitness;
                if (f < min) min = f;
                if (f > max) max = f;
                double delta = f - mean;
                mean += delta / (i + 1);
                m2 += delta * (f - mean);
            }

            double stdDev = n > 1 ? Math.Sqrt(m2 / (n - 1)) : 0.0;

            Current = new PopulationStatistics
            {
                Generation = generation,
                BestFitness = (decimal)max,
                WorstFitness = (decimal)min,
                AverageFitness = (decimal)mean,
                FitnessStdDev = (decimal)stdDev,
                Diversity = CalculateDiversity(population)
            };
        }

        private static double CalculateDiversity(
            IReadOnlyList<Individual> population
        )
        {
            int size = population.Count;
            if (size < 2) return 0;

            const int MaxSampleSize = 10;
            int sampleSize = Math.Min(size - 1, MaxSampleSize);
            int step = Math.Max(1, (size - 1) / sampleSize);

            var reference = population[0].Genotype;
            int rows = reference.GetLength(0);
            int cols = reference.GetLength(1);

            int totalGenes = 0;
            int differentGenes = 0;

            for (int idx = 1; idx < size; idx += step)
            {
                var matrix = population[idx].Genotype;

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
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
