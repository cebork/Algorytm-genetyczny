using System;

namespace Lab2.Core.Domain
{
    public static class MatrixPaddingMapper
    {
        public static bool[,] ExtractActiveMatrix(bool[,] genotype)
        {
            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);

            if (rows < 3 || cols < 3)
                throw new ArgumentException("Genotype must contain an active matrix and a dead border.");

            var active = new bool[rows - 2, cols - 2];

            for (int row = 1; row < rows - 1; row++)
            {
                for (int col = 1; col < cols - 1; col++)
                {
                    active[row - 1, col - 1] = genotype[row, col];
                }
            }

            return active;
        }

        public static bool[,] AddEmptyRowAndColumn(
            bool[,] source,
            int targetSize,
            int emptyRowIndex,
            int emptyColumnIndex)
        {
            int sourceRows = source.GetLength(0);
            int sourceCols = source.GetLength(1);

            if (sourceRows != targetSize - 1 || sourceCols != targetSize - 1)
                throw new ArgumentException("Source matrix must be exactly one row and one column smaller than target size.");

            ValidatePaddingIndexes(targetSize, emptyRowIndex, emptyColumnIndex);

            var result = new bool[targetSize, targetSize];
            int emptyRow = emptyRowIndex - 1;
            int emptyCol = emptyColumnIndex - 1;

            for (int row = 0; row < targetSize; row++)
            {
                if (row == emptyRow)
                    continue;

                int sourceRow = row < emptyRow ? row : row - 1;

                for (int col = 0; col < targetSize; col++)
                {
                    if (col == emptyCol)
                        continue;

                    int sourceCol = col < emptyCol ? col : col - 1;
                    result[row, col] = source[sourceRow, sourceCol];
                }
            }

            return result;
        }

        public static bool[,] AddEmptyActiveRowAndColumnToGenotype(
            bool[,] genotype,
            int targetActiveSize,
            int emptyRowIndex,
            int emptyColumnIndex)
        {
            int sourceActiveRows = genotype.GetLength(0) - 2;
            int sourceActiveCols = genotype.GetLength(1) - 2;

            if (sourceActiveRows != targetActiveSize - 1 ||
                sourceActiveCols != targetActiveSize - 1)
            {
                throw new ArgumentException("Genotype active matrix must be exactly one row and one column smaller than target active size.");
            }

            ValidatePaddingIndexes(targetActiveSize, emptyRowIndex, emptyColumnIndex);

            var result = new bool[targetActiveSize + 2, targetActiveSize + 2];
            int emptyActiveRow = emptyRowIndex;
            int emptyActiveCol = emptyColumnIndex;

            for (int row = 1; row <= targetActiveSize; row++)
            {
                if (row == emptyActiveRow)
                    continue;

                int sourceRow = row < emptyActiveRow ? row : row - 1;

                for (int col = 1; col <= targetActiveSize; col++)
                {
                    if (col == emptyActiveCol)
                        continue;

                    int sourceCol = col < emptyActiveCol ? col : col - 1;
                    result[row, col] = genotype[sourceRow, sourceCol];
                }
            }

            return result;
        }

        public static bool[,] RemoveRowAndColumn(
            bool[,] source,
            int emptyRowIndex,
            int emptyColumnIndex)
        {
            int rows = source.GetLength(0);
            int cols = source.GetLength(1);

            if (rows != cols)
                throw new ArgumentException("Source matrix must be square.");

            ValidatePaddingIndexes(rows, emptyRowIndex, emptyColumnIndex);

            var result = new bool[rows - 1, cols - 1];
            int emptyRow = emptyRowIndex - 1;
            int emptyCol = emptyColumnIndex - 1;

            for (int row = 0; row < rows; row++)
            {
                if (row == emptyRow)
                    continue;

                int targetRow = row < emptyRow ? row : row - 1;

                for (int col = 0; col < cols; col++)
                {
                    if (col == emptyCol)
                        continue;

                    int targetCol = col < emptyCol ? col : col - 1;
                    result[targetRow, targetCol] = source[row, col];
                }
            }

            return result;
        }

        private static void ValidatePaddingIndexes(
            int matrixSize,
            int emptyRowIndex,
            int emptyColumnIndex)
        {
            if (emptyRowIndex < 1 || emptyRowIndex > matrixSize)
                throw new ArgumentOutOfRangeException(nameof(emptyRowIndex));

            if (emptyColumnIndex < 1 || emptyColumnIndex > matrixSize)
                throw new ArgumentOutOfRangeException(nameof(emptyColumnIndex));
        }
    }
}
