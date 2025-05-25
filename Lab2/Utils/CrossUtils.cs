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


    }
}

