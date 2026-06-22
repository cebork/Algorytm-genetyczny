using Lab2.Core.Domain;
using Lab2.objects;
using Lab2.UI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab2.Infrastructure
{

    public static class FileUtils
    {

        private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string PatternsFilePath = Path.Combine(DataDirectory, "patterns.json");
        private static readonly string ReferenceMatrixesFilePath = Path.Combine(DataDirectory, "referenceMatrixes.json");

        private static readonly string ResultDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Results");
        private static readonly string GaTunningFilePath = Path.Combine(ResultDirectory, "tunning_GA.txt");
        private static readonly string GaResultsFilePath = Path.Combine(ResultDirectory, "results_GA.txt");
        private static readonly string maxFCCorr = Path.Combine(ResultDirectory, "max_f_C_corr.txt");
        private static readonly string CumulativeFilePath = Path.Combine(ResultDirectory, "cumulative.txt");
        private static readonly string LastSeedFilePath = Path.Combine(ResultDirectory, "last_seed.txt");
        private static readonly string PerfectSeedsFilePath = Path.Combine(ResultDirectory, "perfect_seeds.txt");



        private class SerializablePattern
        {
            public string PatternName { get; set; }
            public int PatternSize { get; set; }
            public bool[][] PatternMatrix { get; set; }
        }


        private class SerializableReferenceMatrix
        {
            public string ReferenceMatrixName { get; set; }
            public int MatrixSize { get; set; }
            public bool[][] ReferenceMatrixMatrix { get; set; }
        }

        public static List<PatternChoosingDisplayColumns> LoadAllPatterns()
        {
            if (!File.Exists(PatternsFilePath)) return new List<PatternChoosingDisplayColumns>();

            var json = File.ReadAllText(PatternsFilePath);
            var loadedList = JsonSerializer.Deserialize<List<SerializablePattern>>(json);

            return loadedList?.Select(item => new PatternChoosingDisplayColumns
            {
                PatternName = item.PatternName,
                PatternSize = item.PatternSize,
                PatternMatrix = To2DArray(item.PatternMatrix)
            }).ToList() ?? new List<PatternChoosingDisplayColumns>();
        }

        private static bool[,] To2DArray(bool[][] jagged)
        {
            int rows = jagged.Length;
            int cols = jagged[0].Length;
            var result = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = jagged[i][j];
                }
            }

            return result;
        }

        public static void AppendPatternToFile(PatternChoosingDisplayColumns pattern)
        {
            Directory.CreateDirectory(DataDirectory);

            var existing = new List<SerializablePattern>();

            if (File.Exists(PatternsFilePath))
            {
                var json = File.ReadAllText(PatternsFilePath);
                existing = JsonSerializer.Deserialize<List<SerializablePattern>>(json) ?? new List<SerializablePattern>();
            }

            var serializable = new SerializablePattern
            {
                PatternName = pattern.PatternName,
                PatternSize = pattern.PatternSize,
                PatternMatrix = ToJaggedArray(pattern.PatternMatrix)
            };

            existing.Add(serializable);

            var newJson = JsonSerializer.Serialize(existing, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(PatternsFilePath, newJson);
        }

        private static bool[][] ToJaggedArray(bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            var result = new bool[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new bool[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = matrix[i, j];
                }
            }

            return result;
        }

        public static List<ReferenceMatrixDisplayColumns> LoadAllReferenceMatrixes(int matrixSize)
        {
            if (!File.Exists(ReferenceMatrixesFilePath)) return new List<ReferenceMatrixDisplayColumns>();

            var json = File.ReadAllText(ReferenceMatrixesFilePath);
            var loadedList = JsonSerializer.Deserialize<List<SerializableReferenceMatrix>>(json);

            return loadedList?.Where(item => item.MatrixSize == matrixSize).Select(item => new ReferenceMatrixDisplayColumns
            {
                ReferenceMatrixName = item.ReferenceMatrixName,
                MatrixSize = item.MatrixSize,
                ReferenceMatrix = To2DArray(item.ReferenceMatrixMatrix)
            }).ToList() ?? new List<ReferenceMatrixDisplayColumns>();
        }

        public static void AppendReferenceMatrixToFile(ReferenceMatrixDisplayColumns referenceMatrixDisplayColumns)
        {
            Directory.CreateDirectory(DataDirectory);

            var existing = new List<SerializableReferenceMatrix>();

            if (File.Exists(ReferenceMatrixesFilePath))
            {
                var json = File.ReadAllText(ReferenceMatrixesFilePath);
                existing = JsonSerializer.Deserialize<List<SerializableReferenceMatrix>>(json) ?? new List<SerializableReferenceMatrix>();
            }

            var serializable = new SerializableReferenceMatrix
            {
                ReferenceMatrixName = referenceMatrixDisplayColumns.ReferenceMatrixName,
                MatrixSize = referenceMatrixDisplayColumns.MatrixSize,
                ReferenceMatrixMatrix = ToJaggedArray(referenceMatrixDisplayColumns.ReferenceMatrix),
            };

            existing.Add(serializable);

            var newJson = JsonSerializer.Serialize(existing, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ReferenceMatrixesFilePath, newJson);
        }

        public static void SaveCumulativeResults(int[] generationSuccesses, int iterationCount)
        {
            Directory.CreateDirectory(ResultDirectory);

            var builder = new StringBuilder();
            int lastGeneration = Math.Min(iterationCount, generationSuccesses.Length - 1);

            for (int generation = 0; generation <= lastGeneration; generation++)
            {
                if (generation > 0)
                    builder.Append(' ');

                builder.Append(generation);
            }

            builder.AppendLine();

            for (int generation = 0; generation <= lastGeneration; generation++)
            {
                if (generation > 0)
                    builder.Append(' ');

                builder.Append(generationSuccesses[generation]);
            }

            File.WriteAllText(CumulativeFilePath, builder.ToString());
        }

        public static int? LoadLastSeed()
        {
            if (!File.Exists(LastSeedFilePath))
                return null;

            string text = File.ReadAllText(LastSeedFilePath).Trim();
            return int.TryParse(text, out int value) ? value : null;
        }

        public static void SaveLastSeed(int seed)
        {
            Directory.CreateDirectory(ResultDirectory);
            File.WriteAllText(LastSeedFilePath, seed.ToString());
        }

        public static void SavePerfectSeedResults(
            IEnumerable<PerfectSeedResult> results,
            InitialData initialData
        )
        {
            Directory.CreateDirectory(ResultDirectory);

            using var writer = new StreamWriter(PerfectSeedsFilePath, false, Encoding.UTF8);

            writer.WriteLine("# Seedy, dla ktorych znaleziono perfekcyjne rozwiazanie");
            writer.WriteLine($"# Rozmiar macierzy: {initialData.RequestedMatrixSize}");
            writer.WriteLine($"# Efektywny rozmiar macierzy: {initialData.MatrixSize}");
            writer.WriteLine($"# Liczba eksperymentow: {initialData.NumberOfExperiments}");
            writer.WriteLine($"# Typ algorytmu: {initialData.AlgorithmType}");
            writer.WriteLine($"# Typ selekcji: {initialData.SelectionType}");
            writer.WriteLine($"# Typ krzyzowania: {initialData.CrossType}");
            writer.WriteLine($"# Typ mutacji: {initialData.MutationType}");
            writer.WriteLine();

            var orderedResults = results
                .OrderBy(result => result.ExperimentIndex)
                .ToList();

            if (orderedResults.Count == 0)
            {
                writer.WriteLine("Brak perfekcyjnych rozwiazan w tym multirun.");
                return;
            }

            foreach (var result in orderedResults)
            {
                writer.WriteLine($"Eksperyment: {result.ExperimentIndex + 1}");
                writer.WriteLine($"Seed: {result.Seed}");
                writer.WriteLine($"Pokolenie: {result.Generation}");
                writer.WriteLine($"Fitness: {result.Fitness:F6}");
                writer.WriteLine("Macierz:");
                WriteMatrix(writer, result.Genotype);
                writer.WriteLine();
            }
        }

        public static void SaveGaTunningResults(List<TestObject> testObjects, InitialData initialData)
        {
            Directory.CreateDirectory(ResultDirectory);

            using var writer = new StreamWriter(GaTunningFilePath, false, Encoding.UTF8);

            writer.WriteLine("# parametry badania");
            writer.WriteLine($"# Rozmiar macierzy: {initialData.RequestedMatrixSize}");
            writer.WriteLine($"# Efektywny rozmiar macierzy: {initialData.MatrixSize}");
            writer.WriteLine($"# Typ algorytmu: {initialData.AlgorithmType}");
            writer.WriteLine($"# Typ selekcji: {initialData.SelectionType}");
            writer.WriteLine($"# Typ krzyzowania: {initialData.CrossType}");
            writer.WriteLine($"# Typ mutacji: {initialData.MutationType}");
            writer.WriteLine($"# Liczba eksperymentow na konfiguracje: {initialData.NumberOfExperiments}");
            writer.WriteLine();
            writer.WriteLine("# Wyniki badania posortowane malejaco po sredniej ocenie");
            writer.WriteLine("#  1         2           3             4         5       6       7        8                9                       10");
            writer.WriteLine("# No.        N           T             Pk        Pm      Rt      Ps       IPK      f_min_C_corr_N       f_avg_C_corr_N       f_max_C_corr_N");

            int index = 1;
            foreach (var result in testObjects.OrderByDescending(test => test.AvgMark))
            {
                writer.WriteLine(
                    $"{index,-4} {result.N,10} {result.T,12} {result.pk,12:F3} {result.pm,10:F4} {result.Rt,7} {result.Ps,7:F3} {result.Ipk,8} {result.MinMark,18:F4} {result.AvgMark,21:F4} {result.MaxMark,21:F4}"
                );
                index++;
            }
        }

        private static void WriteMatrix(StreamWriter writer, bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    writer.Write(matrix[row, col] ? '1' : '0');
                    if (col < cols - 1)
                        writer.Write(' ');
                }

                writer.WriteLine();
            }
        }

    }

    public sealed class PerfectSeedResult
    {
        public int ExperimentIndex { get; init; }
        public int Seed { get; init; }
        public int Generation { get; init; }
        public decimal Fitness { get; init; }
        public bool[,] Genotype { get; init; } = null!;
    }
}
