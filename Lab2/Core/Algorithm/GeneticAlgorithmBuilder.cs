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

namespace Lab2.Core.Algorithm
{
    public class GeneticAlgorithmBuilder
    {
        private List<Individual> _initialPopulation;
        private IFitnessEvaluator _fitness;
        private ISelectionStrategy _selection;
        private ICrossoverOperator _crossover;
        private decimal _crossProbability = 1m;
        private IRandomProvider _crossoverRandom;
        private bool _eliteEnabled;
        private decimal _eliteFraction;
        private bool _stopAtFirstCorrect;
        private IMutationOperator _mutation;
        private ITerminationCondition _termination;
        private PopulationStatisticsCollector _statistics;

        // Stagnation diversification (optional)
        private bool _stagnationEnabled;
        private int _stagnationWindow;
        private decimal _stagnationResetFraction;
        private IRandomProvider _stagnationRandom;
        private decimal _stagnationDiversityThreshold;

        private GeneticAlgorithmBuilder() { }

        public static GeneticAlgorithmBuilder Create()
        {
            return new GeneticAlgorithmBuilder();
        }


        public GeneticAlgorithmBuilder WithInitialPopulation(List<Individual> population)
        {
            _initialPopulation = population ?? throw new ArgumentNullException(nameof(population));
            return this;
        }


        public GeneticAlgorithmBuilder WithFitness(IFitnessEvaluator fitness)
        {
            _fitness = fitness ?? throw new ArgumentNullException(nameof(fitness));
            return this;
        }


        public GeneticAlgorithmBuilder WithSelection(ISelectionStrategy selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            return this;
        }


        public GeneticAlgorithmBuilder WithCrossover(ICrossoverOperator crossover)
        {
            _crossover = crossover ?? throw new ArgumentNullException(nameof(crossover));
            return this;
        }


        public GeneticAlgorithmBuilder WithCrossoverProbability(decimal crossProbability, IRandomProvider random)
        {
            if (crossProbability < 0 || crossProbability > 1)
                throw new ArgumentOutOfRangeException(nameof(crossProbability), "Crossover probability must be in [0,1].");

            _crossProbability = crossProbability;
            _crossoverRandom = random ?? throw new ArgumentNullException(nameof(random));
            return this;
        }


        public GeneticAlgorithmBuilder WithElitism(bool enabled, decimal eliteFraction)
        {
            if (eliteFraction < 0m || eliteFraction > 1m)
                throw new ArgumentOutOfRangeException(nameof(eliteFraction), "Elite fraction must be in [0,1].");

            _eliteEnabled = enabled;
            _eliteFraction = eliteFraction;
            return this;
        }


        public GeneticAlgorithmBuilder StopAtFirstCorrect(bool enabled)
        {
            _stopAtFirstCorrect = enabled;
            return this;
        }


        public GeneticAlgorithmBuilder WithMutation(IMutationOperator mutation)
        {
            _mutation = mutation ?? throw new ArgumentNullException(nameof(mutation));
            return this;
        }


        public GeneticAlgorithmBuilder WithTermination(ITerminationCondition termination)
        {
            _termination = termination ?? throw new ArgumentNullException(nameof(termination));
            return this;
        }
        public GeneticAlgorithmBuilder WithStatistics(PopulationStatisticsCollector statistics)
        {
            _statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
            return this;
        }

        public GeneticAlgorithmBuilder WithStagnationReset(
            int stagnationWindow,
            decimal resetFraction,
            IRandomProvider random,
            decimal diversityThreshold)
        {
            _stagnationEnabled = true;
            _stagnationWindow = stagnationWindow;
            _stagnationResetFraction = resetFraction;
            _stagnationRandom = random ?? throw new ArgumentNullException(nameof(random));
            _stagnationDiversityThreshold = diversityThreshold;
            return this;
        }

        public GeneticAlgorithm Build()
        {
            if (_initialPopulation == null || _initialPopulation.Count == 0)
                throw new InvalidOperationException("Initial population is not defined.");

            if (_fitness == null)
                throw new InvalidOperationException("Fitness evaluator is not defined.");

            if (_selection == null)
                throw new InvalidOperationException("Selection strategy is not defined.");

            if (_crossover == null)
                throw new InvalidOperationException("Crossover operator is not defined.");

            if (_mutation == null)
                throw new InvalidOperationException("Mutation operator is not defined.");

            if (_crossoverRandom == null)
                throw new InvalidOperationException("Crossover random provider is not defined.");

            if (_termination == null)
                throw new InvalidOperationException("Termination condition is not defined.");

            var ga = new GeneticAlgorithm(
                _initialPopulation,
                _fitness,
                _selection,
                _crossover,
                _crossProbability,
                _crossoverRandom,
                _eliteEnabled,
                _eliteFraction,
                _stopAtFirstCorrect,
                _mutation,
                _termination,
                _statistics ?? new PopulationStatisticsCollector()
            );

            if (_stagnationEnabled)
                ga.ConfigureStagnationReset(
                    _stagnationWindow,
                    _stagnationResetFraction,
                    _stagnationRandom,
                    _stagnationDiversityThreshold
                );

            return ga;
        }
    }
}
