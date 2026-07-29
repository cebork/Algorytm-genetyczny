using System;

namespace Lab2.Core.Domain
{
    public readonly struct BoolMatrix
    {
        private readonly bool[] _cells;

        public int Rows { get; }
        public int Cols { get; }

        public BoolMatrix(int rows, int cols)
        {
            if (rows < 0)
                throw new ArgumentOutOfRangeException(nameof(rows));
            if (cols < 0)
                throw new ArgumentOutOfRangeException(nameof(cols));

            Rows = rows;
            Cols = cols;
            _cells = new bool[rows * cols];
        }

        public BoolMatrix(int rows, int cols, bool[] flatCells)
        {
            if (flatCells == null)
                throw new ArgumentNullException(nameof(flatCells));
            if (flatCells.Length != rows * cols)
                throw new ArgumentException("flatCells length must equal rows*cols.", nameof(flatCells));

            Rows = rows;
            Cols = cols;
            _cells = flatCells;
        }

        public bool IsEmpty => _cells == null;

        public bool this[int row, int col]
        {
            get => _cells[row * Cols + col];
            set => _cells[row * Cols + col] = value;
        }

        public BoolMatrix Clone()
        {
            var copy = new bool[_cells.Length];
            Array.Copy(_cells, copy, _cells.Length);
            return new BoolMatrix(Rows, Cols, copy);
        }
    }
}
