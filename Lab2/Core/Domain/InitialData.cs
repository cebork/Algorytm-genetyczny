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
        public decimal Precision { get; set; }
        public decimal NumberOfIndividuals { get; set; }
        public decimal CrossProbability { get; set; }
        public decimal MutationProbability { get; set; }
        public decimal NumberOfIterations { get; set; }
        public bool[,] SupervisedReferenceMatrix { get; set; }
        public bool[][,] UnsupervisedPatternMatrixes { get; set; }
        public decimal NumberOfExperiments { get; set; }
        public AlgorithmOption AlgorithmOption { get; set; }
        public SelectionType SelectionType { get; set; }
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
        public decimal EliteToMove { get; set; }
        public InitialData() { }

        public InitialData(
            decimal matrixSize,
            decimal precision,
            decimal numberOfIndividuals,
            decimal crossProbability,
            decimal mutationProbability,
            decimal numberOfIterations,
            bool[,] supervisedReferenceMatrix,
            decimal numberOfExperiments
        )
        {
            MatrixSize = matrixSize;
            Precision = precision;
            NumberOfIndividuals = numberOfIndividuals;
            CrossProbability = crossProbability;
            MutationProbability = mutationProbability;
            NumberOfIterations = numberOfIterations;
            SupervisedReferenceMatrix = supervisedReferenceMatrix;
            NumberOfExperiments = numberOfExperiments;
        }
    }
}
