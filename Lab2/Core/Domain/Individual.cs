using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Enums;
using System.Globalization;
using MathNet.Numerics.LinearAlgebra;

namespace Lab2.Core.Domain
{
    public class Individual
    {
        private readonly int precisionDigits;
        public AlgorithmType AlgorithmType { get; set; }
        public int MatrixSize { get; set; }
        private double precision;
        private double crossProbability;
        private double mutationPorbability;

        public Matrix<double> IndividualMatrix { get; set; }

        public decimal OrderNumber { get; set; }
        public double Mark;
        public double FitValue;
        public double Probability;
        public double Distribuator;
        public double RandomValueToCheck;


        public Matrix<double> MatrixAfterSelection { get; set; }

        public Matrix<double> MatrixParents { get; set; }

        public Matrix<double> MatrixChild { get; set; }

        public Matrix<double> MatrixAfterCross { get; set; }

        public Matrix<double> MatrixAfterMutation { get; set; }
        public Matrix<double> ReferenceMatrix { get; set; }
        public Matrix<double>[] PatternMatrixes { get; set; }
        public string xBinParents { get; set; }
        public string xBinChild { get; set; }
        public decimal CutPoint { get; set; }

        public string MutationPosition { get; set; }

        public double MarkAfterMutation { get; set; }
        public double NotNormalizedMarkAfterMutation { get; set; }
        public Individual(decimal orderNumber, decimal matrixSize, double precision, double crossProbability, double mutationProbability, Matrix<double> referenceMatrix, AlgorithmType algorithmType, Matrix<double>[] patternMatrixes)
        {

            AlgorithmType = algorithmType;
            OrderNumber = orderNumber;
            MatrixSize = (int)matrixSize;
            this.precision = precision;
            this.crossProbability = crossProbability;
            mutationPorbability = mutationProbability;
            ReferenceMatrix = referenceMatrix;
            PatternMatrixes = patternMatrixes;
            precisionDigits = GetPrecisionDigits(precision);
            InitOsobnikMatrix();
            SetOcena();

        }

        public Individual(decimal orderNumber, decimal matrixSize, double precision, double crossProbability, double mutationProbability, Matrix<double> nextMatrix, Matrix<double> referenceMatrix, AlgorithmType algorithmType, Matrix<double>[] patternMatrixes)
        {
            AlgorithmType = algorithmType;
            OrderNumber = orderNumber;
            MatrixSize = (int)matrixSize;
            this.precision = precision;
            this.crossProbability = crossProbability;
            mutationPorbability = mutationProbability;
            IndividualMatrix = nextMatrix;
            ReferenceMatrix = referenceMatrix;
            PatternMatrixes = patternMatrixes;
            precisionDigits = GetPrecisionDigits(precision);
            SetOcena();
        }
        private int GetPrecisionDigits(double number)
        {
            var s = number.ToString("0.#############################", CultureInfo.InvariantCulture).TrimEnd('0');
            var parts = s.Split('.');
            return parts.Length == 2 ? parts[1].Length : 0;
        }

        private void InitOsobnikMatrix()
        {
            IndividualMatrix = Matrix<double>.Build.Dense(MatrixSize, MatrixSize, 0);
            Random random = RandomSingleton.Instance;
            for (int i = 1; i < MatrixSize - 1; i++)
            {
                for (int j = 1; j < MatrixSize - 1; j++)
                {
                    IndividualMatrix[i, j] = random.Next(2);
                }
            }

        }

        //private int getPrecision(decimal number)
        //{
        //    string numberAsString = number.ToString("0.#############################", System.Globalization.CultureInfo.InvariantCulture);
        //    string trimmedString = numberAsString.TrimEnd('0');
        //    var numberSplited = trimmedString.Split(".");
        //    if (numberSplited != null && numberSplited.Length > 0) return numberSplited[1].Length;
        //    return 0;
        //}

        public int getL()
        {
            //return (int) Math.Ceiling(Math.Log2(((MatrixWidth/2) / d) + 1));
            return (int)MatrixSize;
        }


        private void SetOcena()
        {
            switch (AlgorithmType)
            {
                case AlgorithmType.SUPERVISED:
                {
                    var m_ref = ReferenceMatrix.SubMatrix(1, MatrixSize - 2, 1, MatrixSize - 2);
                    var m_res = IndividualMatrix.SubMatrix(1, MatrixSize - 2, 1, MatrixSize - 2);

                    // Zastosowanie logicznego AND przez PointwiseMultiply
                    var intersection = m_ref.PointwiseMultiply(m_res);
                    double truePositive = intersection.Enumerate().Sum();

                    // Liczba pozytywnych przypadków w referencyjnej macierzy
                    double totalReferencePositive = m_ref.Enumerate().Sum();

                    // Wynik
                    Mark = totalReferencePositive == 0 ? 0 : Math.Round(truePositive / totalReferencePositive, precisionDigits);

                    break;
                }
                case AlgorithmType.UNSUPERVISED:
                {
                    Mark = CalculateMarkWithPatterns(null, PatternMatrixes, precision);
                    break;
                }

            }

        }

        public void SetFitValue(double minValue)
        {
            FitValue = Mark - minValue + precision;
        }

        public void SetProbability(double sumValue)
        {
            Probability = FitValue / sumValue;
        }




        public void SetCutPoint()
        {
            if (xBinParents != "-")
            {
                CutPoint = RandomSingleton.Instance.Next(0, getL() - 2);
            }
            else
            {
                CutPoint = -1;
            }

        }


        public void SetParent()
        {
            double random = RandomSingleton.Instance.NextDouble();
            if ((double)random <= crossProbability)
            {
                MatrixParents = MatrixAfterSelection;
            }
            else
            {
                MatrixParents = null;
            }
        }

        public void Mutate()
        {
            var random = RandomSingleton.Instance;
            MutationPosition = "";

            var rows = MatrixAfterCross.RowCount;
            var cols = MatrixAfterCross.ColumnCount;

            // Domyślnie: brak mutacji
            var afterMutation = MatrixAfterCross.Clone();

            // Tworzymy maskę mutacji z losowych wartości (1 = mutuj, 0 = nie)
            var mutationMask = Matrix<double>.Build.Dense(rows, cols, (i, j) =>
                (i > 0 && i < rows - 1 && j > 0 && j < cols - 1) && ((double)random.NextDouble() <= mutationPorbability)
                    ? 1.0 : 0.0);

            // Zapisz pozycje mutacji
            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (mutationMask[i, j] == 1.0)
                        MutationPosition += $"[{i}, {j}],";
                }
            }

            // Odwracamy wartości tam, gdzie mutacja == 1
            var inverted = MatrixAfterCross.Map(x => x == 1 ? 0.0 : 1.0);
            MatrixAfterMutation = MatrixAfterCross.PointwiseMultiply(mutationMask.Map(x => 0.0)) // gdzie mutacja = 0 → zachowaj starą
                .Add(inverted.PointwiseMultiply(mutationMask.Map(x => (double)x))); // gdzie mutacja = 1 → użyj odwróconej
        }


    public double SetOcena(Matrix<double> matrixAfterMutation)
        {
            switch (AlgorithmType)
            {
                case AlgorithmType.SUPERVISED:
                    {
                        var m_ref = ReferenceMatrix.SubMatrix(1, MatrixSize - 2, 1, MatrixSize - 2);
                        var m_res = matrixAfterMutation.SubMatrix(1, MatrixSize - 2, 1, MatrixSize - 2);

                        var intersection = m_ref.PointwiseMultiply(m_res);
                        double truePositive = intersection.Enumerate().Sum();

                        double totalReferencePositive = m_ref.Enumerate().Sum();
                        NotNormalizedMarkAfterMutation = truePositive;
                        return totalReferencePositive == 0 ? 0 : Math.Round(truePositive / totalReferencePositive, precisionDigits);
                    }
                case AlgorithmType.UNSUPERVISED:
                    {
                        return CalculateMarkWithPatterns(matrixAfterMutation, PatternMatrixes, precision);
                    }

                default:
                    throw new InvalidOperationException("Unknown algorithm type");
            }
        }


        private double CalculateMarkWithPatterns(Matrix<double> matrixAfterMutation, Matrix<double>[] referencePatterns, double precision)
        {
            return 0;
            //int size = MatrixSize;
            //int totalPositions = 0;
            //int matchCount = 0;

            //Parallel.For(1, MatrixSize - 1, i =>
            //{
            //    for (int j = 1; j < MatrixSize - 1; j++)
            //    {
            //        foreach (var pattern in referencePatterns)
            //        {
            //            if (PatternMatchesAt(matrixAfterMutation != null ? matrixAfterMutation : IndividualMatrix, pattern, i, j))
            //            {
            //                Interlocked.Increment(ref matchCount);
            //                break;
            //            }
            //        }

            //        Interlocked.Increment(ref totalPositions);
            //    }
            //});


            //if (totalPositions == 0)
            //    return 0;

            //return Math.Round((decimal)matchCount / totalPositions, precisionDigits);
        }


        private bool PatternMatchesAt(bool[,] matrix, bool[,] pattern, int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (matrix[row + i, col + j] != pattern[i + 1, j + 1])
                        return false;
                }
            }
            return true;
        }


    }
}
