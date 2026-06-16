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

        private const decimal StagnationMinImprovement = 0.0001m;
        private const decimal StagnationParentPoolFraction = 0.2m;
        private const decimal StagnationEliteFraction = 0.1m;
        private const decimal StagnationDiversificationMutationProbability = 0.1m;

        // Stagnation diversification (disabled by default)
        private bool _stagnationEnabled;
        private int _stagnationWindow;
        private decimal _resetFraction;
        private decimal _diversityThreshold;
        private IRandomProvider _stagnationRandom;

        private readonly List<PopulationStatistics> _statisticsHistory = new();
        public IReadOnlyList<PopulationStatistics> StatisticsHistory => _statisticsHistory;

        public IReadOnlyList<IReadOnlyList<Individual>> History => _history;
        private readonly List<IReadOnlyList<Individual>> _history = new();
        public IReadOnlyList<Individual> CurrentPopulation => _population;
        public bool StoreHistory { get; set; }

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
        /// Enables stagnation diversification. When best fitness does not improve
        /// for <paramref name="stagnationWindow"/> consecutive generations and diversity
        /// is low, the weakest individuals are replaced by mutated copies of strong ones.
        /// </summary>
        public void ConfigureStagnationReset(
            int stagnationWindow,
            decimal resetFraction,
            IRandomProvider random,
            decimal diversityThreshold)
        {
            _stagnationWindow = stagnationWindow;
            _resetFraction = resetFraction;
            _diversityThreshold = diversityThreshold;
            _stagnationRandom = random;
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
                _statistics.Update(_population, iteration);

                if (_stagnationEnabled)
                {
                    decimal currentBest = _statistics.Current.BestFitness;
                    if (currentBest > bestFitness + StagnationMinImprovement)
                    {
                        bestFitness = currentBest;
                        stagnationCounter = 0;
                    }
                    else
                    {
                        stagnationCounter++;
                        if (stagnationCounter >= _stagnationWindow &&
                            _statistics.Current.Diversity < (double)_diversityThreshold)
                        {
                            ApplyStagnationDiversification();
                            EvaluatePopulation();
                            _statistics.Update(_population, iteration);
                            stagnationCounter = 0;
                            bestFitness = Math.Max(bestFitness, _statistics.Current.BestFitness);
                        }
                    }
                }

                _statisticsHistory.Add(_statistics.Current);
                if (StoreHistory)
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

        private void ApplyStagnationDiversification()
        {
            // Sort descending by fitness, then replace only the weakest tail.
            _population.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

            int eliteCount = Math.Max(1, (int)Math.Ceiling(_populationSize * (double)StagnationEliteFraction));
            int parentPoolSize = Math.Max(eliteCount, (int)Math.Ceiling(_populationSize * (double)StagnationParentPoolFraction));
            int replaceCount = Math.Max(1, (int)Math.Ceiling(_populationSize * (double)_resetFraction));
            replaceCount = Math.Min(replaceCount, _populationSize - eliteCount);

            for (int i = _populationSize - replaceCount; i < _populationSize; i++)
            {
                int parentIndex = _stagnationRandom.Next(0, parentPoolSize);
                bool[,] genotype = (bool[,])_population[parentIndex].Genotype.Clone();
                DiversifyGenotype(genotype);
                _population[i] = new Individual(genotype);
            }
        }

        private void DiversifyGenotype(bool[,] genotype)
        {
            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);

            for (int r = 1; r < rows - 1; r++)
            {
                for (int c = 1; c < cols - 1; c++)
                {
                    if (_stagnationRandom.NextDouble() <= (double)StagnationDiversificationMutationProbability)
                        genotype[r, c] = !genotype[r, c];
                }
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
