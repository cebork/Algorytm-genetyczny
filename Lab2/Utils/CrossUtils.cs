using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Domain;

namespace Lab2.Utils
{
    internal static class CrossUtils
    {
        public static void SetCutPoint(List<Individual> individuals)
        {
            int newCutPoint = RandomSingleton.Instance.Next(1, individuals[0].getL() - 2);
            int iter = 0;
            foreach (var individual in individuals)
            {
                if (individual.MatrixParents != null)
                {
                    if (iter == 2)
                    {
                        newCutPoint = RandomSingleton.Instance.Next(1, individuals[0].getL() - 2);
                        iter = 0;
                    }
                    individual.CutPoint = newCutPoint;
                    iter++;
                }
                else
                {
                    individual.CutPoint = -1;
                }
            }
        }

        public static void CrossOsobniks(List<Individual> individuals)
        {
            Individual fParent = null;

            foreach (var individual in individuals)
            {
                if (individual.MatrixParents == null)
                {
                    individual.xBinChild = "-";
                    continue;
                }

                if (fParent == null)
                {
                    fParent = individual;
                    continue;
                }

                Individual sParent = individual;

                if (IsValidCutPoint(fParent) && IsValidCutPoint(sParent))
                {
                    PerformCrossover(fParent, sParent);
                }
                else
                {
                    fParent.xBinChild = "-";
                    sParent.xBinChild = "-";
                }

                fParent = null;
            }

            if (fParent != null)
            {
                fParent.xBinChild = "-";
            }
        }

        private static bool IsValidCutPoint(Individual individual)
        {
            int cutPoint = (int)individual.CutPoint;
            return cutPoint >= 0 && cutPoint <= individual.MatrixSize;
        }

        private static void PerformCrossover(Individual fParent, Individual sParent)
        {
            int cut1 = (int)fParent.CutPoint;
            int cut2 = (int)sParent.CutPoint;

            int width1 = fParent.MatrixSize - cut1;
            int width2 = sParent.MatrixSize - cut2;
            int cutWidth = Math.Min(width1, width2);

            if (cutWidth <= 0)
                return;

            bool[,] fPart1 = GetMatrixSlice(fParent.MatrixAfterSelection, 0, cut1);
            bool[,] fPart2 = GetMatrixSlice(sParent.MatrixAfterSelection, cut1, cutWidth);

            bool[,] sPart1 = GetMatrixSlice(sParent.MatrixAfterSelection, 0, cut2);
            bool[,] sPart2 = GetMatrixSlice(fParent.MatrixAfterSelection, cut2, cutWidth);

            fParent.MatrixChild = MergeMatricesHorizontally(fPart1, fPart2);
            sParent.MatrixChild = MergeMatricesHorizontally(sPart1, sPart2);
        }



        public static void CreatePopulationAfterCrossing(List<Individual> individuals)
        {
            foreach (var individual in individuals)
            {
                if (individual.MatrixChild != null)
                {
                    individual.MatrixAfterCross = individual.MatrixChild;
                }
                else
                {
                    individual.MatrixAfterCross = individual.MatrixAfterSelection;
                }
            }
        }


        static bool[,] GetMatrixSlice(bool[,] matrix, int startCol, int width)
        {
            int rows = matrix.GetLength(0);
            int cols = Math.Min(width, matrix.GetLength(1) - startCol);

            bool[,] slice = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    slice[i, j] = matrix[i, startCol + j];
                }
            }

            return slice;
        }



        static bool[,] MergeMatricesHorizontally(bool[,] leftMatrix, bool[,] rightMatrix)
        {
            int rows = leftMatrix.GetLength(0);
            int leftCols = leftMatrix.GetLength(1);
            int rightCols = rightMatrix.GetLength(1);

            bool[,] mergedMatrix = new bool[rows, leftCols + rightCols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < leftCols; j++)
                {
                    mergedMatrix[i, j] = leftMatrix[i, j];
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rightCols; j++)
                {
                    mergedMatrix[i, leftCols + j] = rightMatrix[i, j];
                }
            }

            return mergedMatrix;
        }

        internal static void CreatePopulationAfterCrossingNPoints(List<Individual> individuals, decimal crossCount)
        {
            int numPoints = (int)crossCount;
            var random = RandomSingleton.Instance;
            Individual fParent = null;

            foreach (var individual in individuals)
            {
                if (individual.MatrixParents == null)
                {
                    individual.xBinChild = "-";
                    individual.MatrixAfterCross = individual.MatrixAfterSelection;
                    continue;
                }

                if (fParent == null)
                {
                    fParent = individual;
                    continue;
                }

                Individual sParent = individual;

                if (fParent.MatrixAfterSelection != null && sParent.MatrixAfterSelection != null)
                {
                    int width = Math.Min(fParent.MatrixSize, sParent.MatrixSize);

                    if (width < 3)
                    {
                        fParent.MatrixAfterCross = fParent.MatrixAfterSelection;
                        sParent.MatrixAfterCross = sParent.MatrixAfterSelection;
                        fParent.xBinChild = "-";
                        sParent.xBinChild = "-";
                        fParent = null;
                        continue;
                    }

                    var cutPoints = new HashSet<int>();
                    while (cutPoints.Count < numPoints)
                    {
                        int point = random.Next(1, width - 1);
                        cutPoints.Add(point);
                    }

                    var sortedCuts = cutPoints.OrderBy(p => p).ToList();

                    fParent.MatrixChild = MultiPointCrossover(fParent.MatrixAfterSelection, sParent.MatrixAfterSelection, sortedCuts);
                    sParent.MatrixChild = MultiPointCrossover(sParent.MatrixAfterSelection, fParent.MatrixAfterSelection, sortedCuts);

                    fParent.MatrixAfterCross = fParent.MatrixChild ?? fParent.MatrixAfterSelection;
                    sParent.MatrixAfterCross = sParent.MatrixChild ?? sParent.MatrixAfterSelection;
                }
                else
                {
                    fParent.xBinChild = "-";
                    sParent.xBinChild = "-";
                    fParent.MatrixAfterCross = fParent.MatrixAfterSelection;
                    sParent.MatrixAfterCross = sParent.MatrixAfterSelection;
                }

                fParent = null;
            }

            if (fParent != null)
            {
                fParent.xBinChild = "-";
                fParent.MatrixAfterCross = fParent.MatrixAfterSelection;
            }
        }


        private static bool[,] MultiPointCrossover(bool[,] m1, bool[,] m2, List<int> cutPoints)
        {
            int rows = m1.GetLength(0);
            int cols = Math.Min(m1.GetLength(1), m2.GetLength(1));

            var segments = new List<(int start, int length)>();
            int prev = 0;
            foreach (int cut in cutPoints)
            {
                segments.Add((prev, cut - prev));
                prev = cut;
            }
            segments.Add((prev, cols - prev));

            bool useFirst = true;
            var result = new bool[rows, cols];
            int colOffset = 0;

            foreach (var (start, length) in segments)
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < length; c++)
                    {
                        result[r, colOffset + c] = useFirst ? m1[r, start + c] : m2[r, start + c];
                    }
                }
                colOffset += length;
                useFirst = !useFirst;
            }

            return result;
        }



    }
}

