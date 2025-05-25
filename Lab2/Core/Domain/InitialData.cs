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
        public InitialData() { }

        public InitialData(
            decimal matrixSize,
            decimal precision,
            decimal numberOfIndividuals,
            decimal crossProbability,
            decimal mutationProbability,
            decimal numberOfIterations,
            bool[,] supervisedReferenceMatrix
        )
        {
            MatrixSize = matrixSize;
            Precision = precision;
            NumberOfIndividuals = numberOfIndividuals;
            CrossProbability = crossProbability;
            MutationProbability = mutationProbability;
            NumberOfIterations = numberOfIterations;
            SupervisedReferenceMatrix = supervisedReferenceMatrix;
        }
    }
}
