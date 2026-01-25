using System;
using System.Collections.Generic;
using System.Linq;
using Lab2.Core.Domain;

namespace Lab2.Core.Statistics
{
    public sealed class PopulationStatistics
    {
        public int Generation { get; init; }

        public decimal BestFitness { get; init; }
        public decimal WorstFitness { get; init; }
        public decimal AverageFitness { get; init; }
        public decimal FitnessStdDev { get; init; }

        public double Diversity { get; init; } // 0–1

        public override string ToString()
            => $"Gen {Generation}: best={BestFitness:F4}, avg={AverageFitness:F4}";
    }
}
