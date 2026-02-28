using Lab2.Core.Domain;
using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Selection
{
    public class TournamentSelection : ISelectionStrategy
    {
        private readonly int _tournamentSize;
        private readonly IRandomProvider _random;
        private readonly Individual[] _contestantBuffer;

        public TournamentSelection(int tournamentSize, IRandomProvider random)
        {
            if (tournamentSize < 2)
                throw new ArgumentException("Tournament size must be at least 2.");

            _tournamentSize = tournamentSize;
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

            // Zwycięzca = najlepszy fitness
            Individual best = _contestantBuffer[0];
            for (int i = 1; i < _tournamentSize; i++)
            {
                if (_contestantBuffer[i].Fitness > best.Fitness)
                    best = _contestantBuffer[i];
            }

            return best;
        }
    }
}
