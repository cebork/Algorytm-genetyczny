using Lab2.Core.Random;
using System;
using System.Collections.Generic;

namespace Lab2.Core.Operators.Crossover
{
    public class MultiPointCrossover : ICrossoverOperator
    {
        private readonly int _points;
        private readonly IRandomProvider _random;

        public MultiPointCrossover(int points, IRandomProvider random)
        {
            _points = points;
            _random = random;
        }

        public (bool[,] Child1, bool[,] Child2) Cross(bool[,] p1, bool[,] p2)
        {
            int rows = p1.GetLength(0);
            int cols = Math.Min(p1.GetLength(1), p2.GetLength(1));

            if (cols < 3 || _points <= 0)
                return (p1, p2);

            int effectivePoints = Math.Min(_points, cols - 2);
            int[] cuts = new int[effectivePoints];
            int cutCount = 0;
            while (cutCount < effectivePoints)
            {
                int candidate = _random.Next(1, cols - 1);
                bool duplicate = false;
                for (int k = 0; k < cutCount; k++)
                {
                    if (cuts[k] == candidate) { duplicate = true; break; }
                }
                if (!duplicate)
                    cuts[cutCount++] = candidate;
            }
            Array.Sort(cuts);

            bool[,] c1 = new bool[rows, cols];
            bool[,] c2 = new bool[rows, cols];

            bool takeFirst = true;
            int start = 0;

            for (int ci = 0; ci <= cuts.Length; ci++)
            {
                int cut = ci < cuts.Length ? cuts[ci] : cols;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = start; c < cut; c++)
                    {
                        c1[r, c] = takeFirst ? p1[r, c] : p2[r, c];
                        c2[r, c] = takeFirst ? p2[r, c] : p1[r, c];
                    }
                }
                takeFirst = !takeFirst;
                start = cut;
            }

            return (c1, c2);
        }
    }
}
