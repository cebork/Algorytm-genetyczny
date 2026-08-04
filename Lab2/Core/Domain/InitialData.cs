using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Enums;
namespace Lab2.Core.Domain
{
    public class InitialData
    {

        public AlgorithmType AlgorithmType { get; set; }
        public decimal MatrixSize { get; set; }
        public decimal RequestedMatrixSize { get; set; }
        public bool UseEvenMatrixPadding { get; set; }
        public int EmptyRowIndex { get; set; } = 1;
        public int EmptyColumnIndex { get; set; } = 1;
        public decimal NumberOfIndividuals { get; set; }
        public decimal CrossProbability { get; set; }
        public decimal MutationProbability { get; set; }
        public decimal BitFlipEarlyExplorationMultiplier { get; set; } = 1.6m;
        public decimal BitFlipLateExplorationMultiplier { get; set; } = 0.7m;
        public decimal NarrowingMutationMultiplier { get; set; } = 1m;
        public int NarrowingMutationStep { get; set; } = 1;
        public decimal NarrowingMutationExponent { get; set; } = 1m;
        public bool LimitNarrowingMutation { get; set; }
        public decimal NumberOfIterations { get; set; }
        public bool[,] SupervisedReferenceMatrix { get; set; }
        public bool[][,] UnsupervisedPatternMatrixes { get; set; }
        public decimal NumberOfExperiments { get; set; }
        public AlgorithmOption AlgorithmOption { get; set; }
        public SelectionType SelectionType { get; set; }
        public bool AdvancedSelectionEnabled { get; set; }
        public List<SelectionStage> SelectionStages { get; set; } = new();
        public decimal TournamentSelectionSize { get; set; }
        public decimal TournamentSoftSelectionTreshold { get; set; }
        public CrossType CrossType { get; set; }
        public decimal CrossCount { get; set; }
        public MutationType MutationType { get; set; }
        public bool UniformBlock { get; set; }
        public decimal ProbGen1 { get; set; }
        public decimal UniformBlockMutationProbWhite { get; set; }
        public decimal UniformBlockMutationProbRed { get; set; }
        public bool EliteOn { get; set; }
        public decimal EliteFraction { get; set; }
        public bool StopAtFirstCorrect { get; set; }
        public bool UseSeed { get; set; }
        public int RandomSeed { get; set; }
        public bool StagnationEnabled { get; set; }
        public int StagnationWindow { get; set; } = 50;
        public decimal StagnationResetFraction { get; set; } = 0.1m;
        public decimal StagnationDiversityThreshold { get; set; } = 0.05m;
        public InitialData() { }

        public InitialData(
            decimal matrixSize,
            decimal numberOfIndividuals,
            decimal crossProbability,
            decimal mutationProbability,
            decimal numberOfIterations,
            bool[,] supervisedReferenceMatrix,
            decimal numberOfExperiments
        )
        {
            MatrixSize = matrixSize;
            NumberOfIndividuals = numberOfIndividuals;
            CrossProbability = crossProbability;
            MutationProbability = mutationProbability;
            NumberOfIterations = numberOfIterations;
            SupervisedReferenceMatrix = supervisedReferenceMatrix;
            NumberOfExperiments = numberOfExperiments;
        }
    }
}
