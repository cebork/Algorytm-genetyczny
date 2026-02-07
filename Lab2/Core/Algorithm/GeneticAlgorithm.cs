using Lab2.Core.Domain;
using Lab2.Core.Fitness;
using Lab2.Core.Operators.Crossover;
using Lab2.Core.Operators.Mutation;
using Lab2.Core.Operators.Selection;
using Lab2.Core.Statistics;
using Lab2.Core.Termination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Algorithm
{
    public class GeneticAlgorithm
    {
        private List<Individual> _population;
        private readonly IFitnessEvaluator _fitness;
        private readonly ISelectionStrategy _selection;
        private readonly ICrossoverOperator _crossover;
        private readonly IMutationOperator _mutation;
        private readonly ITerminationCondition _termination;
        private readonly PopulationStatisticsCollector _statistics;


        private readonly int _populationSize;

        private readonly List<PopulationStatistics> _statisticsHistory = new();
        public IReadOnlyList<PopulationStatistics> StatisticsHistory => _statisticsHistory;

        public IReadOnlyList<IReadOnlyList<Individual>> History => _history;
        private readonly List<IReadOnlyList<Individual>> _history = new();

        public GeneticAlgorithm(
            List<Individual> initialPopulation,
            IFitnessEvaluator fitness,
            ISelectionStrategy selection,
            ICrossoverOperator crossover,
            IMutationOperator mutation,
            ITerminationCondition termination,
            PopulationStatisticsCollector statistics
        )
        {
            _population = initialPopulation;
            _populationSize = initialPopulation.Count;
            _fitness = fitness;
            _selection = selection;
            _crossover = crossover;
            _mutation = mutation;
            _termination = termination;
            _statistics = statistics;
        }

        public void Run(IProgress<int> progress = null)
        {
            int iteration = 0;
            EvaluatePopulation();
            _statistics.Update(_population, iteration);
            _statisticsHistory.Add(_statistics.Current);

            progress?.Report(iteration);

            int maxIterations = _termination is MaxIterationCondition m ? m.MaxIterations : iteration;

            while (!_termination.ShouldStop(iteration))
            {
                var parents = _selection.Select(_population);

                var offspring = new List<Individual>();

                for (int i = 0; i < parents.Count - 1; i += 2)
                {
                    var (c1, c2) = _crossover.Cross(
                        parents[i].Genotype,
                        parents[i + 1].Genotype
                    );



                    c1 = _mutation.Mutate(c1, iteration, maxIterations);
                    c2 = _mutation.Mutate(c2, iteration, maxIterations);

                    offspring.Add(new Individual(c1));
                    offspring.Add(new Individual(c2));
                }

                _population = offspring.Take(_populationSize).ToList();
                EvaluatePopulation();

                _statistics.Update(_population, iteration);
                _statisticsHistory.Add(_statistics.Current);

                _history.Add(_population.Select(i => i.Clone()).ToList());
                iteration++;

                progress?.Report(iteration);

            }
        }

        private void EvaluatePopulation()
        {
            Parallel.ForEach(
                _population,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                individual =>
                {
                    individual.Fitness = _fitness.Evaluate(individual.Genotype);
                }
            );
        }


    }
}
