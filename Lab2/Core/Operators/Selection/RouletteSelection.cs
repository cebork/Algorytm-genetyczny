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
            decimal sum = population.Sum(i => i.Fitness);
            var selected = new List<Individual>();

            for (int i = 0; i < population.Count; i++)
            {
                decimal r = (decimal)_random.NextDouble() * sum;
                decimal acc = 0;

                foreach (var ind in population)
                {
                    acc += ind.Fitness;
                    if (acc >= r)
                    {
                        selected.Add(ind.Clone());
                        break;
                    }
                }
            }

            return selected;
        }
    }
}
