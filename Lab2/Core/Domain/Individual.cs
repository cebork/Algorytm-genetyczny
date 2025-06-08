using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Enums;
using System.Globalization;

namespace Lab2.Core.Domain
{
    public class Individual
    {
        private readonly int precisionDigits;
        public AlgorithmType AlgorithmType { get; set; }
        public int MatrixSize { get; set; }
        private decimal precision;
        private decimal crossProbability;
        private decimal mutationPorbability;

        public bool[,] IndividualMatrix { get; set; }

        public decimal OrderNumber { get; set; }
        public decimal Mark;
        public decimal FitValue;
        public decimal Probability;
        public decimal Distribuator;
        public decimal RandomValueToCheck;


        public bool[,] MatrixAfterSelection { get; set; }

        public bool[,] MatrixParents { get; set; }

        public bool[,] MatrixChild { get; set; }

        public bool[,] MatrixAfterCross { get; set; }

        public bool[,] MatrixAfterMutation { get; set; }
        public bool[,] ReferenceMatrix { get; set; }
        public bool[][,] PatternMatrixes { get; set; }
        public string xBinParents { get; set; }
        public string xBinChild { get; set; }
        public decimal CutPoint { get; set; }

        public string MutationPosition { get; set; }

        public decimal MarkAfterMutation { get; set; }
        public decimal NotNormalizedMarkAfterMutation { get; set; }
        public Individual(decimal orderNumber, decimal matrixSize, decimal precision, decimal crossProbability, decimal mutationProbability, bool[,] referenceMatrix, AlgorithmType algorithmType, bool[][,] patternMatrixes)
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

        public Individual(decimal orderNumber, decimal matrixSize, decimal precision, decimal crossProbability, decimal mutationProbability, bool[,] nextMatrix, bool[,] referenceMatrix, AlgorithmType algorithmType, bool[][,] patternMatrixes)
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
        private int GetPrecisionDigits(decimal number)
        {
            var s = number.ToString("0.#############################", CultureInfo.InvariantCulture).TrimEnd('0');
            var parts = s.Split('.');
            return parts.Length == 2 ? parts[1].Length : 0;
        }

        private void InitOsobnikMatrix()
        {
            IndividualMatrix = new bool[MatrixSize, MatrixSize];
            Random random = RandomSingleton.Instance;
            for (int i = 1; i < MatrixSize - 1; i++)
            {
                for (int j = 1; j < MatrixSize - 1; j++)
                {
                    IndividualMatrix[i, j] = random.Next(2) == 0;
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
                    int meter = 0;
                    int denominator = 0;
                    for (int i = 1; i < MatrixSize - 1; i++)
                    {
                        for (int j = 1; j < MatrixSize - 1; j++)
                        {
                            if (IndividualMatrix[i, j] == ReferenceMatrix[i, j] == true)
                            {
                                meter++;
                                denominator++;
                            }
                            if ((IndividualMatrix[i, j] == true && ReferenceMatrix[i, j] == false) || (IndividualMatrix[i, j] == false && ReferenceMatrix[i, j] == true))
                            {
                                denominator++;
                            }
                        }
                    }

                    Mark = Math.Round((decimal)meter / denominator, precisionDigits);
                    break;
                }
                case AlgorithmType.UNSUPERVISED:
                {
                    Mark = CalculateMarkWithPatterns(null, PatternMatrixes, precision);
                    break;
                }

            }

        }

        public void SetFitValue(decimal minValue)
        {
            FitValue = Mark - minValue + precision;
        }

        public void SetProbability(decimal sumValue)
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
            if ((decimal)random <= crossProbability)
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
            MutationPosition = "";
            int rows = MatrixAfterCross.GetLength(0);
            int cols = MatrixAfterCross.GetLength(1);

            bool[,] afterMutation = null;

            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    double randomDouble = RandomSingleton.Instance.NextDouble();

                    if ((decimal)randomDouble <= mutationPorbability)
                    {
                        if (afterMutation == null)
                            afterMutation = (bool[,])MatrixAfterCross.Clone();

                        afterMutation[i, j] = !MatrixAfterCross[i, j];
                        MutationPosition += $"[{i}, {j}],";


                    }
                }
            }

            MatrixAfterMutation = afterMutation ?? MatrixAfterCross;
        }





        public decimal SetOcena(bool[,] matrixAfterMutation)
        {
            switch (AlgorithmType)
            {
                case AlgorithmType.SUPERVISED:
                    {
                        int meter = 0;
                        int denominator = 0;
                        for (int i = 1; i < MatrixSize - 1; i++)
                        {
                            for (int j = 1; j < MatrixSize - 1; j++)
                            {
                                if (matrixAfterMutation[i, j] == true && ReferenceMatrix[i, j] == true)
                                {
                                    meter++;
                                    denominator++;
                                }
                                if ((matrixAfterMutation[i, j] == true && ReferenceMatrix[i, j] == false) || (matrixAfterMutation[i, j] == false && ReferenceMatrix[i, j] == true))
                                {
                                    denominator++;
                                }
                            }
                        }
                        NotNormalizedMarkAfterMutation = meter;
                        return Math.Round((decimal)meter / denominator, precisionDigits);
                    }
                case AlgorithmType.UNSUPERVISED:
                    {
                        return CalculateMarkWithPatterns(matrixAfterMutation, PatternMatrixes, precision);
                    }

                default:
                    throw new InvalidOperationException("Unknown algorithm type");
            }
        }


        private decimal CalculateMarkWithPatterns(bool[,] matrixAfterMutation, bool[][,] referencePatterns, decimal precision)
        {
            int size = MatrixSize;
            int totalPositions = 0;
            int matchCount = 0;

            Parallel.For(1, MatrixSize - 1, i =>
            {
                for (int j = 1; j < MatrixSize - 1; j++)
                {
                    foreach (var pattern in referencePatterns)
                    {
                        if (PatternMatchesAt(matrixAfterMutation != null ? matrixAfterMutation : IndividualMatrix, pattern, i, j))
                        {
                            Interlocked.Increment(ref matchCount);
                            break;
                        }
                    }

                    Interlocked.Increment(ref totalPositions);
                }
            });


            if (totalPositions == 0)
                return 0;

            return Math.Round((decimal)matchCount / totalPositions, precisionDigits);
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
