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
            int totalPositions = 0;
            int matchCount = 0;
            int lastRow = genotype.GetLength(0) - 1;
            int lastCol = genotype.GetLength(1) - 1;

            for (int row = 0; row <= lastRow; row++)
            {
                for (int col = 0; col <= lastCol; col++)
                {
                    bool hasCandidate = false;

                    foreach (var pattern in _patterns)
                    {
                        if (!CanStartPatternAt(genotype, pattern, row, col))
                            continue;

                        hasCandidate = true;

                        if (PatternMatchesAt(genotype, pattern, row, col))
                        {
                            matchCount++;
                            break;
                        }
                    }

                    if (hasCandidate)
                        totalPositions++;
                }
            }

            if (totalPositions == 0)
                return 0;

            return Math.Round((decimal)matchCount / totalPositions, _precisionDigits);
        }

        private bool PatternMatchesAt(bool[,] matrix, bool[,] pattern, int row, int col)
        {
            int patternRows = pattern.GetLength(0);
            int patternCols = pattern.GetLength(1);

            for (int i = 0; i < patternRows; i++)
            {
                for (int j = 0; j < patternCols; j++)
                {
                    if (matrix[row + i, col + j] != pattern[i, j])
                        return false;
                }
            }
            return true;
        }

        private bool CanStartPatternAt(bool[,] genotype, bool[,] pattern, int row, int col)
        {
            if (pattern == null)
                return false;

            int patternRows = pattern.GetLength(0);
            int patternCols = pattern.GetLength(1);
            int lastPatternRow = row + patternRows - 1;
            int lastPatternCol = col + patternCols - 1;

            return patternRows > 0 &&
                   patternCols > 0 &&
                   patternRows == patternCols &&
                   lastPatternRow < genotype.GetLength(0) &&
                   lastPatternCol < genotype.GetLength(1);
        }
    }
}
