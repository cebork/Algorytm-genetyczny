using Lab2.Core.Domain;
using Lab2.Core.Fitness;
using Lab2.Core.Operators.Crossover;
using Lab2.Core.Operators.Mutation;
using Lab2.Core.Operators.Selection;
using Lab2.Core.Random;
using Lab2.Core.Statistics;
using Lab2.Core.Termination;
using System;
using System.Collections.Generic;
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

        // Stagnation reset (disabled by default)
        private bool _stagnationEnabled;
        private int _stagnationWindow;
        private decimal _resetFraction;
        private IRandomProvider _stagnationRandom;
        private decimal _probGen1;

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

        /// <summary>
        /// Enables the stagnation-reset mechanism. When best fitness does not improve
        /// for <paramref name="stagnationWindow"/> consecutive generations, the bottom
        /// <paramref name="resetFraction"/> of the population is replaced with fresh
        /// random individuals, keeping the elite at the top.
        /// </summary>
        public void ConfigureStagnationReset(
            int stagnationWindow,
            decimal resetFraction,
            IRandomProvider random,
            decimal probGen1)
        {
            _stagnationWindow = stagnationWindow;
            _resetFraction = resetFraction;
            _stagnationRandom = random;
            _probGen1 = probGen1;
            _stagnationEnabled = stagnationWindow > 0;
        }

        public void Run(IProgress<int> progress = null)
        {
            int iteration = 0;
            EvaluatePopulation();
            _statistics.Update(_population, iteration);
            _statisticsHistory.Add(_statistics.Current);
            progress?.Report(iteration);

            int maxIterations = _termination is MaxIterationCondition m ? m.MaxIterations : iteration;

            decimal bestFitness = _statistics.Current.BestFitness;
            int stagnationCounter = 0;

            while (!_termination.ShouldStop(iteration))
            {
                var parents = _selection.Select(_population);
                var offspring = new List<Individual>(_populationSize + 1);

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

                if (offspring.Count > _populationSize)
                    offspring.RemoveAt(offspring.Count - 1);

                _population = offspring;

                EvaluatePopulation();

                if (_stagnationEnabled)
                {
                    decimal currentBest = GetBestFitness();
                    if (currentBest > bestFitness)
                    {
                        bestFitness = currentBest;
                        stagnationCounter = 0;
                    }
                    else
                    {
                        stagnationCounter++;
                        if (stagnationCounter >= _stagnationWindow)
                        {
                            ApplyStagnationReset();
                            EvaluatePopulation();
                            stagnationCounter = 0;
                            bestFitness = GetBestFitness();
                        }
                    }
                }

                _statistics.Update(_population, iteration);
                _statisticsHistory.Add(_statistics.Current);
                _history.Add(_population);
                iteration++;

                progress?.Report(iteration);
            }
        }

        private decimal GetBestFitness()
        {
            decimal best = decimal.MinValue;
            for (int i = 0; i < _population.Count; i++)
            {
                if (_population[i].Fitness > best)
                    best = _population[i].Fitness;
            }
            return best;
        }

        private void ApplyStagnationReset()
        {
            // Sort descending by fitness — keeps elite at the front
            _population.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

            int eliteCount = Math.Max(1, (int)(_populationSize * (1m - _resetFraction)));
            int innerSize = _population[0].Genotype.GetLength(0) - 2;

            for (int i = eliteCount; i < _populationSize; i++)
            {
                _population[i] = new Individual(
                    InitialGenotypeFactory.Create(innerSize, _probGen1, _stagnationRandom)
                );
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
