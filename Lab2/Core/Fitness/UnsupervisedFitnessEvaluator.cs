using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Fitness
{
    public class UnsupervisedPatternFitnessEvaluator : IFitnessEvaluator
    {
        private readonly bool[][,] _patterns;
        private readonly int _precisionDigits;

        public UnsupervisedPatternFitnessEvaluator(bool[][,] patterns, int precisionDigits)
        {
            _patterns = patterns;
            _precisionDigits = precisionDigits;
        }

        public decimal Evaluate(bool[,] genotype)
        {
            int size = genotype.GetLength(0);
            int totalPositions = 0;
            int matchCount = 0;

            for (int i = 1; i < size - 1; i++)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    foreach (var pattern in _patterns)
                    {
                        if (PatternMatchesAt(genotype, pattern, i, j))
                        {
                            matchCount++;
                            break;
                        }
                    }
                    totalPositions++;
                }
            }

            if (totalPositions == 0)
                return 0;

            return Math.Round((decimal)matchCount / totalPositions, _precisionDigits);
        }

        private bool PatternMatchesAt(bool[,] matrix, bool[,] pattern, int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (matrix[row + i, col + j] != pattern[i + 1, j + 1])
                        return false;
                }
            }
            return true;
        }
    }
}
