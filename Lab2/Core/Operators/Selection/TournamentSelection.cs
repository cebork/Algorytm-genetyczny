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

        public TournamentSelection(int tournamentSize, IRandomProvider random)
        {
            if (tournamentSize < 2)
                throw new ArgumentException("Tournament size must be at least 2.");

            _tournamentSize = tournamentSize;
            _random = random;
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
            var contestants = new List<Individual>(_tournamentSize);

            for (int i = 0; i < _tournamentSize; i++)
            {
                int index = _random.Next(0, population.Count);
                contestants.Add(population[index]);
            }

            // Zwycięzca = najlepszy fitness
            return contestants
                .OrderByDescending(i => i.Fitness)
                .First()
                .Clone();
        }
    }
}
