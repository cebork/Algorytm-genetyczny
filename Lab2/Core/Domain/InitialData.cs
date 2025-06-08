using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Enums;
using MathNet.Numerics.LinearAlgebra;

namespace Lab2.Core.Domain
{
    public class InitialData
    {

        public AlgorithmType AlgorithmType { get; set; }
        public double MatrixSize { get; set; }
        public double Precision { get; set; }
        public double NumberOfIndividuals { get; set; }
        public double CrossProbability { get; set; }
        public double MutationProbability { get; set; }
        public double NumberOfIterations { get; set; }
        public Matrix<double> SupervisedReferenceMatrix { get; set; }
        public Matrix<double>[] UnsupervisedPatternMatrixes { get; set; }
        public InitialData() { }

        public InitialData(
            double matrixSize,
            double precision,
            double numberOfIndividuals,
            double crossProbability,
            double mutationProbability,
            double numberOfIterations,
            Matrix<double> supervisedReferenceMatrix
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
