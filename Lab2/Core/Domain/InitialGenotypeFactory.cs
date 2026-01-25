using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Domain
{
    public static class InitialGenotypeFactory
    {
        public static bool[,] Create(
            int innerSize,
            decimal probTrue,
            IRandomProvider random
        )
        {
            int size = innerSize + 2;
            var matrix = new bool[size, size];

            // domyślnie false – ramka „martwa”
            for (int r = 1; r <= innerSize; r++)
            {
                for (int c = 1; c <= innerSize; c++)
                {
                    matrix[r, c] = (decimal)random.NextDouble() < probTrue;
                }
            }

            return matrix;
        }
    }
}
