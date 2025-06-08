using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Domain;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

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

            Matrix<double> fPart1 = GetMatrixSlice(fParent.MatrixAfterSelection, 0, cut1);
            Matrix<double> fPart2 = GetMatrixSlice(sParent.MatrixAfterSelection, cut1, cutWidth);

            Matrix<double> sPart1 = GetMatrixSlice(sParent.MatrixAfterSelection, 0, cut2);
            Matrix<double> sPart2 = GetMatrixSlice(fParent.MatrixAfterSelection, cut2, cutWidth);

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


        static Matrix<double> GetMatrixSlice(Matrix<double> matrix, int startCol, int width)
        {
            int rows = matrix.RowCount;
            int cols = Math.Min(width, matrix.ColumnCount - startCol); // zabezpieczenie końca

            return matrix.SubMatrix(0, rows, startCol, cols);
        }




        static Matrix<double> MergeMatricesHorizontally(Matrix<double> leftMatrix, Matrix<double> rightMatrix)
        {
            int rows = leftMatrix.RowCount;
            int leftCols = leftMatrix.ColumnCount;
            int rightCols = rightMatrix.ColumnCount;

            var merged = Matrix<double>.Build.Dense(rows, leftCols + rightCols);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < leftCols; j++)
                    merged[i, j] = leftMatrix[i, j];

                for (int j = 0; j < rightCols; j++)
                    merged[i, leftCols + j] = rightMatrix[i, j];
            }

            return merged;
        }



    }
}

