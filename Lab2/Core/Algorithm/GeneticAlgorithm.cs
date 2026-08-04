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
        private readonly decimal _crossProbability;
        private readonly IRandomProvider _random;
        private readonly bool _eliteEnabled;
        private readonly decimal _eliteFraction;
        private readonly bool _stopAtFirstCorrect;
        private readonly IMutationOperator _mutation;
        private readonly ITerminationCondition _termination;
        private readonly PopulationStatisticsCollector _statistics;
        private readonly int _populationSize;

        private const decimal StagnationMinImprovement = 0.0001m;
        private const decimal StagnationParentPoolFraction = 0.2m;
        private const decimal StagnationEliteFraction = 0.1m;
        private const decimal StagnationDiversificationMutationProbability = 0.1m;
        private const decimal PerfectFitnessThreshold = 1m;

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
        public Individual? BestSolution { get; private set; }
        public int? BestSolutionGeneration { get; private set; }
        public Individual? PerfectSolution { get; private set; }
        public int? PerfectSolutionGeneration { get; private set; }
        public bool StoreHistory { get; set; }

        public GeneticAlgorithm(
            List<Individual> initialPopulation,
            IFitnessEvaluator fitness,
            ISelectionStrategy selection,
            ICrossoverOperator crossover,
            decimal crossProbability,
            IRandomProvider random,
            bool eliteEnabled,
            decimal eliteFraction,
            bool stopAtFirstCorrect,
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
            _crossProbability = crossProbability;
            _random = random;
            _eliteEnabled = eliteEnabled;
            _eliteFraction = Math.Clamp(eliteFraction, 0m, 1m);
            _stopAtFirstCorrect = stopAtFirstCorrect;
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

        public void Run(IProgress<int>? progress = null)
        {
            int iteration = 0;
            EvaluatePopulation();
            _statistics.Update(_population, iteration);
            _statisticsHistory.Add(_statistics.Current);
            CaptureBestSolution(iteration);
            CapturePerfectSolution(iteration);
            StorePopulationSnapshot();
            progress?.Report(iteration);

            if (_stopAtFirstCorrect && PerfectSolution != null)
                return;

            int maxIterations = _termination is MaxIterationCondition m ? m.MaxIterations : iteration;

            decimal bestFitness = _statistics.Current.BestFitness;
            int stagnationCounter = 0;

            while (!_termination.ShouldStop(iteration))
            {
                if (_selection is IGenerationAwareSelectionStrategy generationAwareSelection)
                    generationAwareSelection.SetGenerationContext(iteration, maxIterations);

                var parents = new List<Individual>(_selection.Select(_population));
                ShuffleParents(parents);
                var offspring = GetEliteClones();

                for (int i = 0; i < parents.Count - 1 && offspring.Count < _populationSize; i += 2)
                {
                    bool[,] c1;
                    bool[,] c2;

                    if (_random.NextDouble() <= (double)_crossProbability)
                    {
                        (c1, c2) = _crossover.Cross(
                            parents[i].Genotype,
                            parents[i + 1].Genotype
                        );
                    }
                    else
                    {
                        c1 = (bool[,])parents[i].Genotype.Clone();
                        c2 = (bool[,])parents[i + 1].Genotype.Clone();
                    }

                    c1 = _mutation.Mutate(c1, iteration, maxIterations);
                    c2 = _mutation.Mutate(c2, iteration, maxIterations);

                    if (offspring.Count < _populationSize)
                        offspring.Add(new Individual(c1));

                    if (offspring.Count < _populationSize)
                        offspring.Add(new Individual(c2));
                }

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
                CaptureBestSolution(iteration + 1);
                CapturePerfectSolution(iteration + 1);
                StorePopulationSnapshot();
                iteration++;

                progress?.Report(iteration);

                if (_stopAtFirstCorrect && PerfectSolution != null)
                    break;
            }
        }

        private List<Individual> GetEliteClones()
        {
            var elites = new List<Individual>();
            if (!_eliteEnabled || _eliteFraction <= 0m)
                return elites;

            var sortedPopulation = new List<Individual>(_population);
            sortedPopulation.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

            int count = Math.Min((int)Math.Ceiling(_populationSize * _eliteFraction), _populationSize);
            for (int i = 0; i < count; i++)
                elites.Add(sortedPopulation[i].Clone());

            return elites;
        }

        private void ShuffleParents(List<Individual> parents)
        {
            for (int i = parents.Count - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1);
                (parents[i], parents[j]) = (parents[j], parents[i]);
            }
        }

        private void CaptureBestSolution(int generation)
        {
            Individual? best = null;
            for (int i = 0; i < _population.Count; i++)
            {
                if (best == null || _population[i].Fitness > best.Fitness)
                    best = _population[i];
            }

            if (best == null)
                return;

            if (BestSolution != null && best.Fitness <= BestSolution.Fitness)
                return;

            BestSolution = best.Clone();
            BestSolutionGeneration = generation;
        }
        private void CapturePerfectSolution(int generation)
        {
            if (PerfectSolution != null)
                return;

            Individual? best = null;
            for (int i = 0; i < _population.Count; i++)
            {
                if (_population[i].Fitness < PerfectFitnessThreshold)
                    continue;

                if (best == null || _population[i].Fitness > best.Fitness)
                    best = _population[i];
            }

            if (best == null)
                return;

            PerfectSolution = best.Clone();
            PerfectSolutionGeneration = generation;
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

        private void StorePopulationSnapshot()
        {
            if (!StoreHistory)
                return;

            _history.Add(_population.Select(individual => individual.Clone()).ToList());
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

