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
    public class GeneticAlgorithmBuilder
    {
        private List<Individual> _initialPopulation;
        private IFitnessEvaluator _fitness;
        private ISelectionStrategy _selection;
        private ICrossoverOperator _crossover;
        private IMutationOperator _mutation;
        private ITerminationCondition _termination;
        private PopulationStatisticsCollector _statistics;

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

            if (_termination == null)
                throw new InvalidOperationException("Termination condition is not defined.");

            return new GeneticAlgorithm(
                _initialPopulation,
                _fitness,
                _selection,
                _crossover,
                _mutation,
                _termination,
                _statistics ?? new PopulationStatisticsCollector()
            );
        }
    }
}
