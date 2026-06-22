using Lab2.Core.Domain;
using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2.Core.Operators.Selection
{
    public class SoftTournamentSelection : ISelectionStrategy
    {
        private readonly int _tournamentSize;
        private readonly decimal _bestSelectionProbability;
        private readonly IRandomProvider _random;
        private readonly Individual[] _contestantBuffer;

        public SoftTournamentSelection(int tournamentSize, decimal bestSelectionProbability, IRandomProvider random)
        {
            if (tournamentSize < 2)
                throw new ArgumentException("Tournament size must be at least 2.");

            if (bestSelectionProbability is < 0 or > 1)
                throw new ArgumentException("Selection threshold must be in [0,1].");

            _tournamentSize = tournamentSize;
            _bestSelectionProbability = bestSelectionProbability;
            _random = random;
            _contestantBuffer = new Individual[tournamentSize];
        }

        public IReadOnlyList<Individual> Select(IReadOnlyList<Individual> population)
        {
            if (population == null || population.Count == 0)
                throw new ArgumentException("Population is empty.");

            var selected = new List<Individual>(population.Count);

            for (int i = 0; i < population.Count; i++)
            {
                selected.Add(RunTournament(population));
            }

            return selected;
        }

        private Individual RunTournament(IReadOnlyList<Individual> population)
        {
            for (int i = 0; i < _tournamentSize; i++)
            {
                _contestantBuffer[i] = population[_random.Next(0, population.Count)];
            }

            Array.Sort(
                _contestantBuffer,
                0,
                _tournamentSize,
                Comparer<Individual>.Create((a, b) => b.Fitness.CompareTo(a.Fitness))
            );

            if (_random.NextDouble() <= (double)_bestSelectionProbability)
                return _contestantBuffer[0];

            return _contestantBuffer[_random.Next(1, _tournamentSize)];
        }
    }
}
