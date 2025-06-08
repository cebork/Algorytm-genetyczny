using Lab2.UI.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Lab2.objects;
using Lab2.Core.Domain;
using System.Text;

namespace Lab2.Services
{
    public static class FileUtils
    {
        private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string PatternsFilePath = Path.Combine(DataDirectory, "patterns.json");
        private static readonly string ReferenceMatrixesFilePath = Path.Combine(DataDirectory, "referenceMatrixes.json");

        private static readonly string ResultDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Results");
        private static readonly string GaTunningFilePath = Path.Combine(ResultDirectory, "tunning_GA.txt");
        private static readonly string GaResultsFilePath = Path.Combine(ResultDirectory, "results_GA.txt");
        private static readonly string mGaResultsFilePath = Path.Combine(ResultDirectory, "m_results_GA.txt");
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

        public static void saveGaTunningResults(List<TestObject> testObjects, InitialData initialData)
        {
            Directory.CreateDirectory(ResultDirectory);
            using (var writer = new StreamWriter(GaTunningFilePath, false, Encoding.UTF8))
            {
                writer.WriteLine("# parametry badania");
                writer.WriteLine($"# Rozmiar macierzy: {initialData.MatrixSize}");
                if (initialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED)
                {
                    writer.WriteLine("# Macierz referencyjna");

                    bool[,] matrix = initialData.SupervisedReferenceMatrix;
                    int rows = matrix.GetLength(0);
                    int cols = matrix.GetLength(1);

                    for (int row = 0; row < rows; row++)
                    {
                        var line = new StringBuilder("# ");
                        for (int col = 0; col < cols; col++)
                        {
                            line.Append(matrix[row, col] ? "1 " : "0 ");
                        }
                        writer.WriteLine(line.ToString().TrimEnd());
                    }
                }
                if (initialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED)
                {
                    writer.WriteLine("# Macierze wzorców");

                    bool[][,] matrices = initialData.UnsupervisedPatternMatrixes;

                    for (int index = 0; index < matrices.Length; index++)
                    {
                        var matrix = matrices[index];
                        int rows = matrix.GetLength(0);
                        int cols = matrix.GetLength(1);

                        writer.WriteLine($"# Macierz {index}");

                        for (int row = 0; row < rows; row++)
                        {
                            var line = new StringBuilder("# ");
                            for (int col = 0; col < cols; col++)
                            {
                                line.Append(matrix[row, col] ? "1 " : "0 ");
                            }
                            writer.WriteLine(line.ToString().TrimEnd());
                        }

                        writer.WriteLine();
                    }
                }

                writer.WriteLine("# parametry badania");
                writer.WriteLine("# Wyniki badania");
                writer.WriteLine("#  1         2           3             4         5                6                       7                      8");
                writer.WriteLine("# No.        N           T             Pk       Pm       f_min_C_corr_N       f_avg_C_corr_N       f_max_C_corr_N");
                int index2 = 1;
                foreach (var r in testObjects.OrderByDescending(tes => tes.AvgMark))
                {
                    writer.WriteLine(
                        $"{index2,-4} {r.N,10} {r.T,12} {r.pk,12:F3} {r.pm,10:F4} {r.MinMark,18:F3} {r.AvgMark,21:F3} {r.MaxMark,21:F3}"
                    );
                    index2++;
                }
            }
        }

        public static void SaveResultsGa(List<List<Individual>> individuals, InitialData initialData, string filePath = null, bool append = false)
        {
            filePath ??= GaResultsFilePath;
            Directory.CreateDirectory(ResultDirectory);
            using (var writer = new StreamWriter(filePath, append, Encoding.UTF8))
            {
                writer.WriteLine("# parametry badania");
                writer.WriteLine($"# Rozmiar macierzy: {initialData.MatrixSize}");
                writer.WriteLine($"# Pk: {initialData.CrossProbability}");
                writer.WriteLine($"# Pm: {initialData.MutationProbability}");
                writer.WriteLine($"# N: {initialData.NumberOfIndividuals}");
                writer.WriteLine($"# T: {initialData.NumberOfIterations}");
                if (initialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED)
                {
                    writer.WriteLine("# Macierz referencyjna");

                    bool[,] matrix = initialData.SupervisedReferenceMatrix;
                    int rows = matrix.GetLength(0);
                    int cols = matrix.GetLength(1);

                    for (int row = 0; row < rows; row++)
                    {
                        var line = new StringBuilder("# ");
                        for (int col = 0; col < cols; col++)
                        {
                            line.Append(matrix[row, col] ? "1 " : "0 ");
                        }
                        writer.WriteLine(line.ToString().TrimEnd());
                    }
                }
                if (initialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED)
                {
                    writer.WriteLine("# Macierze wzorców");

                    bool[][,] matrices = initialData.UnsupervisedPatternMatrixes;

                    for (int index = 0; index < matrices.Length; index++)
                    {
                        var matrix = matrices[index];
                        int rows = matrix.GetLength(0);
                        int cols = matrix.GetLength(1);

                        writer.WriteLine($"# Macierz {index}");

                        for (int row = 0; row < rows; row++)
                        {
                            var line = new StringBuilder("# ");
                            for (int col = 0; col < cols; col++)
                            {
                                line.Append(matrix[row, col] ? "1 " : "0 ");
                            }
                            writer.WriteLine(line.ToString().TrimEnd());
                        }

                        writer.WriteLine();
                    }
                }

                writer.WriteLine("# parametry badania");
                writer.WriteLine("# Wyniki badania");
                writer.WriteLine("#   1                 2                     3                  4                 5            6              7");
                writer.WriteLine("# iter         f_min_C_corr_N       f_avg_C_corr_N       f_max_C_corr_N       f_min_C      f_avg_C       f_max_C");

                var rows2 = individuals.Select((ind, i) => new
                {
                    Index = i + 1,
                    FMinCorrN = ind.Min(i => i.MarkAfterMutation),
                    FAvgCorrN = ind.Average(i => i.MarkAfterMutation),
                    FMaxCorrN = ind.Max(i => i.MarkAfterMutation),
                    FMinC = ind.Min(i => i.NotNormalizedMarkAfterMutation),
                    FAvgC = ind.Average(i => i.NotNormalizedMarkAfterMutation),
                    FMaxC = ind.Max(i => i.NotNormalizedMarkAfterMutation),
                });

                var best = rows2.OrderByDescending(x => x.FMaxCorrN).First();
                var last = rows2.OrderByDescending(x => x.Index).First();
                int index2 = 1;
                foreach (var r in rows2)
                {
                    writer.WriteLine(
                        $"{r.Index,-12}{r.FMinCorrN,15:F3}{r.FAvgCorrN,20:F3}{r.FMaxCorrN,20:F3}{r.FMinC,15:F3}{r.FAvgC,15:F3}{r.FMaxC,15:F3}"
                    );
                    index2++;
                }

                writer.WriteLine(
                    $"{"best",-12}{best.FMinCorrN,15:F3}{best.FAvgCorrN,20:F3}{best.FMaxCorrN,20:F3}{best.FMinC,15:F3}{best.FAvgC,15:F3}{best.FMaxC,15:F3}"
                );
                writer.WriteLine(
                    $"{"last",-12}{last.FMinCorrN,15:F3}{last.FAvgCorrN,20:F3}{last.FMaxCorrN,20:F3}{last.FMinC,15:F3}{last.FAvgC,15:F3}{last.FMaxC,15:F3}"
                );
            }
        }

        public static void SaveMResultsGa(Dictionary<InitialData, List<List<Individual>>> keyValuePairsIndividuals)
        {
            if (File.Exists(mGaResultsFilePath))
            {
                File.Delete(mGaResultsFilePath);
            }
            foreach (var kvp in keyValuePairsIndividuals)
            {
                InitialData initialData = kvp.Key;
                List<List<Individual>> history = kvp.Value;

                SaveResultsGa(history, initialData, mGaResultsFilePath, true);
            }
        }
    }
}
