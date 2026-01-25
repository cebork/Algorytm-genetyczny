using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Crossover
{
    public class OnePointCrossover : ICrossoverOperator
    {
        private readonly IRandomProvider _random;

        public OnePointCrossover(IRandomProvider random)
        {
            _random = random;
        }

        public (bool[,] Child1, bool[,] Child2) Cross(bool[,] p1, bool[,] p2)
        {
            int rows = p1.GetLength(0);
            int cols = Math.Min(p1.GetLength(1), p2.GetLength(1));

            if (cols < 3)
                return (p1, p2);

            int cut = _random.Next(1, cols - 1);

            bool[,] c1 = new bool[rows, cols];
            bool[,] c2 = new bool[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (c < cut)
                    {
                        c1[r, c] = p1[r, c];
                        c2[r, c] = p2[r, c];
                    }
                    else
                    {
                        c1[r, c] = p2[r, c];
                        c2[r, c] = p1[r, c];
                    }
                }
            }

            return (c1, c2);
        }
    }
}
