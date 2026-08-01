using Lab2.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Core.Operators.Selection
{
    public sealed class ThresholdSelectionStrategy : IGenerationAwareSelectionStrategy
    {
        private readonly IReadOnlyList<ThresholdSelectionStage> _stages;
        private int _currentGeneration;
        private int _maxGenerations = 1;

        public ThresholdSelectionStrategy(IReadOnlyList<ThresholdSelectionStage> stages)
        {
            if (stages == null || stages.Count == 0)
                throw new ArgumentException("At least one selection stage is required.", nameof(stages));

            _stages = stages
                .OrderBy(stage => stage.Threshold)
                .ToList();
        }

        public void SetGenerationContext(int currentGeneration, int maxGenerations)
        {
            _currentGeneration = Math.Max(0, currentGeneration);
            _maxGenerations = Math.Max(1, maxGenerations);
        }

        public IReadOnlyList<Individual> Select(IReadOnlyList<Individual> population)
        {
            decimal progress = Math.Clamp(_currentGeneration / (decimal)_maxGenerations, 0m, 1m);

            foreach (var stage in _stages)
            {
                if (progress <= stage.Threshold)
                    return stage.Selection.Select(population);
            }

            return _stages[^1].Selection.Select(population);
        }
    }
}
