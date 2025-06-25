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
        public decimal ProbGen1 { get; set; }
        public Individual(decimal orderNumber, decimal matrixSize, decimal precision, decimal crossProbability, decimal mutationProbability, bool[,] referenceMatrix, AlgorithmType algorithmType, bool[][,] patternMatrixes, decimal probGen1)
        {

            AlgorithmType = algorithmType;
            OrderNumber = orderNumber;
            MatrixSize = (int)matrixSize;
            this.precision = precision;
            this.crossProbability = crossProbability;
            mutationPorbability = mutationProbability;
            ReferenceMatrix = referenceMatrix;
            PatternMatrixes = patternMatrixes;
            ProbGen1 = probGen1;
            precisionDigits = GetPrecisionDigits(precision);
            InitOsobnikMatrix();
            SetOcena();
            ProbGen1 = probGen1;
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
                    // If random.NextDouble() < probgen1 → set true
                    IndividualMatrix[i, j] = random.NextDouble() < (double)ProbGen1;
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
            if (MatrixAfterCross == null)
            {
                MatrixAfterMutation = null;
                return;
            }
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

            for (int i = 1; i < MatrixSize - 1; i++)
            {
                for (int j = 1; j < MatrixSize - 1; j++)
                {
                    foreach (var pattern in referencePatterns)
                    {
                        bool[,] sourceMatrix = matrixAfterMutation != null ? matrixAfterMutation : IndividualMatrix;

                        if (PatternMatchesAt(sourceMatrix, pattern, i, j))
                        {
                            matchCount++;
                            break;
                        }
                    }

                    totalPositions++;
                }
            }



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

        internal void BitSwapMutation()
        {
            if (MatrixAfterCross == null)
            {
                MatrixAfterMutation = null;
                return;
            }

            double chance = RandomSingleton.Instance.NextDouble();
            if ((decimal)chance > mutationPorbability)
            {
                MatrixAfterMutation = MatrixAfterCross;
                return;
            }

            int rows = MatrixAfterCross.GetLength(0);
            int cols = MatrixAfterCross.GetLength(1);

            if (rows <= 2 || cols <= 2)
            {
                MatrixAfterMutation = MatrixAfterCross;
                return;
            }

            int innerRows = rows - 2;
            int innerCols = cols - 2;
            int totalInnerBits = innerRows * innerCols;

            int maxLength = Math.Min(6, totalInnerBits / 4);
            int segmentLength = RandomSingleton.Instance.Next(2, maxLength + 1);

            int maxStart = totalInnerBits - 2 * segmentLength;
            if (maxStart <= 0)
            {
                MatrixAfterMutation = MatrixAfterCross;
                return;
            }

            int firstStart = RandomSingleton.Instance.Next(0, maxStart);
            int secondStart = RandomSingleton.Instance.Next(firstStart + segmentLength, totalInnerBits - segmentLength);

            bool[] flatInner = new bool[totalInnerBits];
            for (int i = 0; i < innerRows; i++)
            {
                for (int j = 0; j < innerCols; j++)
                {
                    flatInner[i * innerCols + j] = MatrixAfterCross[i + 1, j + 1];
                }
            }

            for (int k = 0; k < segmentLength; k++)
            {
                bool temp = flatInner[firstStart + k];
                flatInner[firstStart + k] = flatInner[secondStart + k];
                flatInner[secondStart + k] = temp;
            }

            bool[,] afterMutation = (bool[,])MatrixAfterCross.Clone();
            for (int i = 0; i < totalInnerBits; i++)
            {
                int r = i / innerCols;
                int c = i % innerCols;
                afterMutation[r + 1, c + 1] = flatInner[i];
            }

            MatrixAfterMutation = afterMutation;
        }

        public void MutateUniformBlock()
        {
            if (MatrixAfterCross == null)
            {
                MatrixAfterMutation = null;
                return;
            }

            MutationPosition = "";
            int rows = MatrixAfterCross.GetLength(0);
            int cols = MatrixAfterCross.GetLength(1);

            bool[,] afterMutation = (bool[,])MatrixAfterCross.Clone();

            for (int i = 2; i < rows - 2; i++)
            {
                for (int j = 2; j < cols - 2; j++)
                {
                    bool centerValue = afterMutation[i, j];
                    bool isUniform = true;

                    for (int di = -1; di <= 1 && isUniform; di++)
                    {
                        for (int dj = -1; dj <= 1 && isUniform; dj++)
                        {
                            if (afterMutation[i + di, j + dj] != centerValue)
                                isUniform = false;
                        }
                    }

                    if (isUniform)
                    {
                        afterMutation[i, j] = !centerValue;
                        MutationPosition += $"[{i}, {j}]b,";
                    }
                }
            }

            MatrixAfterMutation = afterMutation ?? MatrixAfterCross;
        }



        public void MutateByNarrowing(int iterationCount, int maxIterations)
        {
            if (MatrixAfterCross == null)
            {
                MatrixAfterMutation = null;
                return;
            }

            MutationPosition = "";
            int rows = MatrixAfterCross.GetLength(0);
            int cols = MatrixAfterCross.GetLength(1);

            int validHeight = rows - 2;
            int validWidth = cols - 2;

            decimal multiplier = Math.Max(0.0m, (maxIterations - iterationCount) / (decimal)maxIterations);
            int mutationTargetCount = (int)Math.Round(
                mutationPorbability * validHeight * validWidth * 2 * multiplier
            );

            if (mutationTargetCount <= 0)
            {
                MatrixAfterMutation = MatrixAfterCross;
                return;
            }

            bool[,] afterMutation = (bool[,])MatrixAfterCross.Clone();
            var usedCoords = new HashSet<(int, int)>();
            var rand = RandomSingleton.Instance;

            int attempts = 0;
            while (usedCoords.Count < mutationTargetCount && attempts < mutationTargetCount * 10)
            {
                int i = rand.Next(1, rows - 1);
                int j = rand.Next(1, cols - 1);

                if (usedCoords.Add((i, j)))
                {
                    afterMutation[i, j] = !MatrixAfterCross[i, j];
                    MutationPosition += $"[{i}, {j}],";
                }

                attempts++;
            }

            MatrixAfterMutation = afterMutation;
        }



    }
}
