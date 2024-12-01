using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal static class CrossUtils
    {
        public static void SetCutPoint(List<Osobnik> osobniks)
        {
            int newCutPoint = RandomSingleton.Instance.Next(1, osobniks[0].getL() - 2);
            int iter = 0;
            foreach (var osobnik in osobniks)
            {
                if (osobnik.MatrixParents != null)
                {
                    if (iter == 2)
                    {
                        newCutPoint = RandomSingleton.Instance.Next(1, osobniks[0].getL() - 2);
                        iter = 0;
                    }
                    osobnik.CutPoint = newCutPoint;
                    iter++;
                }
                else
                {
                    osobnik.CutPoint = -1;
                }
            }
        }

        public static void CrossOsobniks(List<Osobnik> osobniks)
        {
            Osobnik fParent = null;
            Osobnik sParent = null;
            foreach (var osobnik in osobniks)
            {
                if (osobnik.MatrixParents != null)
                {
                    if (fParent != null)
                    {
                        sParent = osobnik;
                    } else
                    {
                        fParent = osobnik;
                    } 
                    if (fParent != null && sParent != null)
                    {
                        if (fParent.CutPoint >= 0 && fParent.CutPoint <= fParent.MatrixWidth && sParent.CutPoint >= 0 && sParent.CutPoint <= sParent.MatrixWidth)
                        {
                            //string fParentPart1 = fParent.xBinAfterSelection.Substring(0, fParent.CutPoint);
                            //string fParentPart2 = fParent.CutPoint < sParent.xBinAfterSelection.Length ? sParent.xBinAfterSelection.Substring(sParent.CutPoint) : string.Empty;

                            //string sParentPart1 = sParent.xBinAfterSelection.Substring(0, sParent.CutPoint);
                            //string sParentPart2 = sParent.CutPoint < fParent.xBinAfterSelection.Length ? fParent.xBinAfterSelection.Substring(fParent.CutPoint) : string.Empty;

                            //fParent.xBinChild = fParentPart1 + fParentPart2;
                            //sParent.xBinChild = sParentPart1 + sParentPart2;

                            int cutWidth = Math.Min(fParent.MatrixWidth - fParent.CutPoint, sParent.MatrixWidth - sParent.CutPoint);

                            // Cut first parent's matrix into two parts
                            bool[,] fParentPart1 = GetMatrixSlice(fParent.MatrixAfterSelection, 0, fParent.CutPoint);
                            bool[,] fParentPart2 = fParent.CutPoint + cutWidth <= sParent.MatrixAfterSelection.GetLength(1)
                                ? GetMatrixSlice(sParent.MatrixAfterSelection, fParent.CutPoint, cutWidth)
                                : new bool[fParent.MatrixAfterSelection.GetLength(0), 0];

                            // Cut second parent's matrix into two parts
                            bool[,] sParentPart1 = GetMatrixSlice(sParent.MatrixAfterSelection, 0, sParent.CutPoint);
                            bool[,] sParentPart2 = sParent.CutPoint + cutWidth <= fParent.MatrixAfterSelection.GetLength(1)
                                ? GetMatrixSlice(fParent.MatrixAfterSelection, sParent.CutPoint, cutWidth)
                                : new bool[sParent.MatrixAfterSelection.GetLength(0), 0];

                            // Merge the parts into children matrices
                            fParent.MatrixChild = MergeMatricesHorizontally(fParentPart1, fParentPart2);
                            sParent.MatrixChild = MergeMatricesHorizontally(sParentPart1, sParentPart2);

                        }
                        fParent = null;
                        sParent = null;
                    }
                }
                else
                {
                    osobnik.xBinChild = "-";
                }
            }
            if (fParent != null)
            {
                fParent.xBinChild = "-";
            }
            
        }


        public static void CreatePopulationAfterCrossing(List<Osobnik> osobniks)
        {
            foreach (var osobik in osobniks)
            {
                if (osobik.MatrixChild != null)
                {
                    osobik.MatrixAfterCross = osobik.MatrixChild;
                }
                else
                {
                    osobik.MatrixAfterCross = osobik.MatrixAfterSelection;
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

