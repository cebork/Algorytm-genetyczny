using Lab2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Operators.Mutation
{
    public class BitSwapMutation : IMutationOperator
    {
        private readonly decimal _mutationProbability;
        private readonly IRandomProvider _random;
        private const double LocalSwapProbability = 0.7;
        private const int LocalSearchRadius = 2;

        public BitSwapMutation(decimal mutationProbability, IRandomProvider random)
        {
            _mutationProbability = mutationProbability;
            _random = random;
        }

        public bool[,] Mutate(bool[,] genotype, int iteration, int maxIterations)
        {
            if (_mutationProbability <= 0)
                return genotype;

            int rows = genotype.GetLength(0);
            int cols = genotype.GetLength(1);
            int innerRows = rows - 2;
            int innerCols = cols - 2;
            int totalInnerBits = innerRows * innerCols;

            if (totalInnerBits < 2)
                return genotype;

            var trueCells = new List<(int Row, int Col)>();
            var falseCells = new List<(int Row, int Col)>();

            for (int row = 1; row < rows - 1; row++)
            {
                for (int col = 1; col < cols - 1; col++)
                {
                    if (genotype[row, col])
                        trueCells.Add((row, col));
                    else
                        falseCells.Add((row, col));
                }
            }

            int maxUsefulSwaps = Math.Min(trueCells.Count, falseCells.Count);
            if (maxUsefulSwaps == 0)
                return genotype;

            int swapCount = CalculateSwapCount(maxUsefulSwaps);
            if (swapCount == 0)
                return genotype;

            var used = new bool[rows, cols];

            for (int swap = 0; swap < swapCount; swap++)
            {
                bool startFromTrue = _random.NextDouble() < 0.5;
                var sourceList = startFromTrue ? trueCells : falseCells;
                var targetList = startFromTrue ? falseCells : trueCells;

                if (!TryPickUnused(sourceList, used, out var source))
                    break;

                bool useLocal = _random.NextDouble() <= LocalSwapProbability;
                if (!TryPickTarget(targetList, used, source, useLocal, out var target))
                    break;

                genotype[source.Row, source.Col] = !genotype[source.Row, source.Col];
                genotype[target.Row, target.Col] = !genotype[target.Row, target.Col];
                used[source.Row, source.Col] = true;
                used[target.Row, target.Col] = true;
            }

            return genotype;
        }

        private int CalculateSwapCount(int maxUsefulSwaps)
        {
            double expectedSwaps = (double)_mutationProbability * maxUsefulSwaps;
            expectedSwaps = Math.Min(maxUsefulSwaps, Math.Max(0.0, expectedSwaps));

            int swapCount = (int)Math.Floor(expectedSwaps);
            double fraction = expectedSwaps - swapCount;

            if (_random.NextDouble() < fraction)
                swapCount++;

            return Math.Min(maxUsefulSwaps, swapCount);
        }

        private bool TryPickUnused(
            List<(int Row, int Col)> cells,
            bool[,] used,
            out (int Row, int Col) selected)
        {
            int availableCount = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (!used[cell.Row, cell.Col])
                    availableCount++;
            }

            if (availableCount == 0)
            {
                selected = default;
                return false;
            }

            int pick = _random.Next(0, availableCount);
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (used[cell.Row, cell.Col])
                    continue;

                if (pick == 0)
                {
                    selected = cell;
                    return true;
                }

                pick--;
            }

            selected = default;
            return false;
        }

        private bool TryPickTarget(
            List<(int Row, int Col)> cells,
            bool[,] used,
            (int Row, int Col) source,
            bool preferLocal,
            out (int Row, int Col) selected)
        {
            if (preferLocal && TryPickLocalTarget(cells, used, source, out selected))
                return true;

            return TryPickUnused(cells, used, out selected);
        }

        private bool TryPickLocalTarget(
            List<(int Row, int Col)> cells,
            bool[,] used,
            (int Row, int Col) source,
            out (int Row, int Col) selected)
        {
            var localCandidates = new List<(int Row, int Col)>();

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (used[cell.Row, cell.Col])
                    continue;

                int distance = Math.Abs(cell.Row - source.Row) + Math.Abs(cell.Col - source.Col);
                if (distance > 0 && distance <= LocalSearchRadius)
                    localCandidates.Add(cell);
            }

            if (localCandidates.Count == 0)
            {
                selected = default;
                return false;
            }

            selected = localCandidates[_random.Next(0, localCandidates.Count)];
            return true;
        }
    }
}
