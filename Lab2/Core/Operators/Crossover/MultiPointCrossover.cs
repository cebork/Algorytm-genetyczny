using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var cutPoints = new HashSet<int>();
            while (cutPoints.Count < _points)
            {
                cutPoints.Add(_random.Next(1, cols - 1));
            }

            var cuts = cutPoints.OrderBy(x => x).ToList();

            bool[,] c1 = new bool[rows, cols];
            bool[,] c2 = new bool[rows, cols];

            bool takeFirst = true;
            int start = 0;

            foreach (int cut in cuts.Append(cols))
            {
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
