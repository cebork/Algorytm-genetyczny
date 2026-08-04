using Lab2.Core.Domain;
using Lab2.Core.Statistics;
using Lab2.objects;
using Lab2.UI.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static readonly string DetailedGaTunningFilePath = Path.Combine(ResultDirectory, "detailed_tunning_GA.txt");
        private static readonly string GaResultsFilePath = Path.Combine(ResultDirectory, "results_GA.txt");
        private static readonly string DetailedGaResultsFilePath = Path.Combine(ResultDirectory, "detailed_result_GA.txt");
        private static readonly string maxFCCorr = Path.Combine(ResultDirectory, "max_f_C_corr.txt");
        private static readonly string CumulativeFilePath = Path.Combine(ResultDirectory, "cumulative.txt");
        private static readonly string LastSeedFilePath = Path.Combine(ResultDirectory, "last_seed.txt");
        private static readonly string PerfectSeedsFilePath = Path.Combine(ResultDirectory, "perfect_seeds.txt");
        private static readonly string FullBestRunFilePath = Path.Combine(ResultDirectory, "full_best_run.txt");
        private static readonly string TunningBestsDirectory = Path.Combine(ResultDirectory, "Tunning bests");



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

            int cumulativeSuccessCount = 0;
            for (int generation = 0; generation <= lastGeneration; generation++)
            {
                if (generation > 0)
                    builder.Append(' ');

                cumulativeSuccessCount += generationSuccesses[generation];
                builder.Append(cumulativeSuccessCount);
            }

            File.WriteAllText(CumulativeFilePath, builder.ToString());
        }

        public static void ArchiveResults()
        {
            if (!Directory.Exists(ResultDirectory))
                return;

            var filesToArchive = Directory
                .EnumerateFiles(ResultDirectory, "*", SearchOption.TopDirectoryOnly)
                .Where(path => !string.Equals(path, LastSeedFilePath, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var directoriesToArchive = Directory
                .EnumerateDirectories(ResultDirectory, "*", SearchOption.TopDirectoryOnly)
                .Where(path => !string.Equals(Path.GetFileName(path), "Archive", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (filesToArchive.Count == 0 && directoriesToArchive.Count == 0)
                return;

            string archiveRoot = Path.Combine(ResultDirectory, "Archive");
            Directory.CreateDirectory(archiveRoot);

            string archiveDirectory = Path.Combine(
                archiveRoot,
                DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")
            );

            Directory.CreateDirectory(archiveDirectory);

            foreach (string sourcePath in filesToArchive)
            {
                string destinationPath = Path.Combine(archiveDirectory, Path.GetFileName(sourcePath));
                File.Move(sourcePath, destinationPath, overwrite: false);
            }

            foreach (string sourcePath in directoriesToArchive)
            {
                string destinationPath = Path.Combine(archiveDirectory, Path.GetFileName(sourcePath));
                Directory.Move(sourcePath, destinationPath);
            }
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
            writer.WriteLine($"# Typ selekcji: {FormatSelectionDescription(initialData)}");
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
            writer.WriteLine($"# Typ selekcji: {FormatSelectionDescription(initialData)}");
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


        public static void SaveDetailedGaResults(List<DetailedRunObject> runObjects, InitialData initialData)
        {
            Directory.CreateDirectory(ResultDirectory);

            using var writer = new StreamWriter(DetailedGaResultsFilePath, false, Encoding.UTF8);

            writer.WriteLine("# szczegolowe wyniki zwyklego uruchomienia - jeden wiersz na niezalezny run");
            writer.WriteLine($"# Rozmiar macierzy: {initialData.RequestedMatrixSize}");
            writer.WriteLine($"# Efektywny rozmiar macierzy: {initialData.MatrixSize}");
            writer.WriteLine($"# Typ algorytmu: {initialData.AlgorithmType}");
            writer.WriteLine($"# Typ selekcji: {FormatSelectionDescription(initialData)}");
            writer.WriteLine($"# Typ krzyzowania: {initialData.CrossType}");
            writer.WriteLine($"# Typ mutacji: {initialData.MutationType}");
            writer.WriteLine($"# Liczba eksperymentow: {initialData.NumberOfExperiments}");
            writer.WriteLine();
            writer.WriteLine("#  1          2            3           4             5         6       7       8       9       10              11              12              13              14");
            writer.WriteLine("# ExpNo      Seed          N           T             Pk        Pm      Rt      Ps      IPK      MinFitness      AvgFitness      BestFitness     BestGeneration  ElapsedMs");

            foreach (var result in runObjects.OrderBy(run => run.ExperimentIndex))
            {
                writer.WriteLine(
                    $"{result.ExperimentIndex + 1,-10} {result.Seed,12} {result.N,12} {result.T,12} {result.pk,10:F3} {result.pm,8:F4} {result.Rt,7} {result.Ps,7:F3} {result.Ipk,8} {result.MinMark,16:F4} {result.AvgMark,16:F4} {result.BestMark,16:F4} {result.BestGeneration,15} {result.Elapsed.TotalMilliseconds,14:F0}"
                );
            }
        }

        public static void SaveDetailedGaTunningResults(List<DetailedTestObject> testObjects, InitialData initialData)
        {
            Directory.CreateDirectory(ResultDirectory);

            using var writer = new StreamWriter(DetailedGaTunningFilePath, false, Encoding.UTF8);

            writer.WriteLine("# szczegolowe wyniki badania - jeden wiersz na eksperyment");
            writer.WriteLine($"# Rozmiar macierzy: {initialData.RequestedMatrixSize}");
            writer.WriteLine($"# Efektywny rozmiar macierzy: {initialData.MatrixSize}");
            writer.WriteLine($"# Typ algorytmu: {initialData.AlgorithmType}");
            writer.WriteLine($"# Typ selekcji: {FormatSelectionDescription(initialData)}");
            writer.WriteLine($"# Typ krzyzowania: {initialData.CrossType}");
            writer.WriteLine($"# Typ mutacji: {initialData.MutationType}");
            writer.WriteLine($"# Liczba eksperymentow na konfiguracje: {initialData.NumberOfExperiments}");
            writer.WriteLine();
            writer.WriteLine("#  1          2           3            4           5             6         7       8       9       10       11              12");
            writer.WriteLine("# ConfigNo   ExpNo       Seed          N           T             Pk        Pm      Rt      Ps      IPK      BestFitness     BestGeneration");

            foreach (var result in testObjects
                .OrderBy(test => test.ConfigurationIter)
                .ThenBy(test => test.ExperimentIndex))
            {
                writer.WriteLine(
                    $"{result.ConfigurationIter,-10} {result.ExperimentIndex + 1,5} {result.Seed,12} {result.N,12} {result.T,12} {result.pk,10:F3} {result.pm,8:F4} {result.Rt,7} {result.Ps,7:F3} {result.Ipk,8} {result.BestMark,16:F4} {result.BestGeneration,15}"
                );
            }
        }

        public static void SaveFullBestRun(FullRunExport run)
        {
            Directory.CreateDirectory(ResultDirectory);

            using var writer = new StreamWriter(FullBestRunFilePath, false, Encoding.UTF8);
            WriteFullRun(writer, run);
        }

        public static void SaveTunningBestRun(FullRunExport run, IEnumerable<string> sweptParameters)
        {
            Directory.CreateDirectory(TunningBestsDirectory);

            string fileName = BuildTunningBestFileName(run, sweptParameters);
            string filePath = Path.Combine(TunningBestsDirectory, fileName);

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            WriteFullRun(writer, run);
        }

        private static void WriteFullRun(StreamWriter writer, FullRunExport run)
        {
            writer.WriteLine("# Full best GA run export");
            writer.WriteLine("# FormatVersion: 1");
            writer.WriteLine();

            writer.WriteLine("[Metadata]");
            writer.WriteLine($"Mode={run.Mode}");
            writer.WriteLine($"ConfigurationNo={run.ConfigurationNo}");
            writer.WriteLine($"ExperimentNo={run.ExperimentNo}");
            writer.WriteLine($"ExperimentIndex={run.ExperimentIndex}");
            writer.WriteLine($"Seed={run.Seed}");
            writer.WriteLine($"BestFitness={FormatDecimal(run.BestFitness)}");
            writer.WriteLine($"BestGeneration={run.BestGeneration}");
            writer.WriteLine($"ElapsedMs={run.Elapsed.TotalMilliseconds:F0}");
            writer.WriteLine();

            writer.WriteLine("[Configuration]");
            writer.WriteLine($"RequestedMatrixSize={FormatDecimal(run.InitialData.RequestedMatrixSize)}");
            writer.WriteLine($"EffectiveMatrixSize={FormatDecimal(run.InitialData.MatrixSize)}");
            writer.WriteLine($"AlgorithmType={run.InitialData.AlgorithmType}");
            writer.WriteLine($"AlgorithmOption={run.InitialData.AlgorithmOption}");
            writer.WriteLine($"SelectionType={run.InitialData.SelectionType}");
            writer.WriteLine($"AdvancedSelectionEnabled={run.InitialData.AdvancedSelectionEnabled}");
            writer.WriteLine($"SelectionStages={FormatSelectionStages(run.InitialData)}");
            writer.WriteLine($"CrossType={run.InitialData.CrossType}");
            writer.WriteLine($"MutationType={run.InitialData.MutationType}");
            writer.WriteLine($"N={FormatDecimal(run.InitialData.NumberOfIndividuals)}");
            writer.WriteLine($"T={FormatDecimal(run.InitialData.NumberOfIterations)}");
            writer.WriteLine($"Pk={FormatDecimal(run.InitialData.CrossProbability)}");
            writer.WriteLine($"Pm={FormatDecimal(run.InitialData.MutationProbability)}");
            writer.WriteLine($"Rt={FormatDecimal(run.InitialData.TournamentSelectionSize)}");
            writer.WriteLine($"Ps={FormatDecimal(run.InitialData.TournamentSoftSelectionTreshold)}");
            writer.WriteLine($"IPK={FormatDecimal(run.InitialData.CrossCount)}");
            writer.WriteLine($"EliteOn={run.InitialData.EliteOn}");
            writer.WriteLine($"EliteFraction={FormatDecimal(run.InitialData.EliteFraction)}");
            writer.WriteLine($"StopAtFirstCorrect={run.InitialData.StopAtFirstCorrect}");
            writer.WriteLine($"UseEvenMatrixPadding={run.InitialData.UseEvenMatrixPadding}");
            writer.WriteLine($"EmptyRowIndex={run.InitialData.EmptyRowIndex}");
            writer.WriteLine($"EmptyColumnIndex={run.InitialData.EmptyColumnIndex}");
            writer.WriteLine($"StagnationEnabled={run.InitialData.StagnationEnabled}");
            writer.WriteLine($"StagnationWindow={run.InitialData.StagnationWindow}");
            writer.WriteLine($"StagnationResetFraction={FormatDecimal(run.InitialData.StagnationResetFraction)}");
            writer.WriteLine($"StagnationDiversityThreshold={FormatDecimal(run.InitialData.StagnationDiversityThreshold)}");
            writer.WriteLine();

            writer.WriteLine("[Statistics]");
            writer.WriteLine("Generation BestFitness AverageFitness WorstFitness FitnessStdDev Diversity");
            foreach (var stat in run.Statistics.OrderBy(stat => stat.Generation))
            {
                writer.WriteLine(
                    $"{stat.Generation} {FormatDecimal(stat.BestFitness)} {FormatDecimal(stat.AverageFitness)} {FormatDecimal(stat.WorstFitness)} {FormatDecimal(stat.FitnessStdDev)} {stat.Diversity.ToString("0.######", CultureInfo.InvariantCulture)}"
                );
            }
            writer.WriteLine();

            writer.WriteLine("[BestIndividualMatrix]");
            WriteMatrix(writer, run.BestGenotype);
            writer.WriteLine();
        }

        private static string FormatSelectionDescription(InitialData initialData)
        {
            if (!initialData.AdvancedSelectionEnabled)
                return initialData.SelectionType.ToString();

            return $"ADVANCED ({FormatSelectionStages(initialData)})";
        }

        private static string FormatSelectionStages(InitialData initialData)
        {
            if (initialData.SelectionStages == null || initialData.SelectionStages.Count == 0)
                return string.Empty;

            return string.Join("; ", initialData.SelectionStages.Select(stage =>
                $"{stage.SelectionType}<={FormatDecimal(stage.Threshold)}"));
        }
        private static string BuildTunningBestFileName(FullRunExport run, IEnumerable<string> sweptParameters)
        {
            var parts = new List<string> { $"config_{run.ConfigurationNo:000}" };

            foreach (string parameter in sweptParameters)
            {
                string value = parameter switch
                {
                    "N" => FormatDecimal(run.InitialData.NumberOfIndividuals),
                    "T" => FormatDecimal(run.InitialData.NumberOfIterations),
                    "Pk" => FormatDecimal(run.InitialData.CrossProbability),
                    "Pm" => FormatDecimal(run.InitialData.MutationProbability),
                    "Rt" => FormatDecimal(run.InitialData.TournamentSelectionSize),
                    "Ps" => FormatDecimal(run.InitialData.TournamentSoftSelectionTreshold),
                    "IPK" => FormatDecimal(run.InitialData.CrossCount),
                    _ => string.Empty
                };

                if (!string.IsNullOrWhiteSpace(value))
                    parts.Add($"{parameter}-{value}");
            }

            return SanitizeFileName(string.Join("_", parts) + ".txt");
        }

        private static string SanitizeFileName(string fileName)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidChar, '_');
            }

            return fileName;
        }

        private static string FormatDecimal(decimal value)
        {
            return value.ToString("0.######", CultureInfo.InvariantCulture);
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
    public sealed class FullRunExport
    {
        public string Mode { get; init; } = string.Empty;
        public int ConfigurationNo { get; init; }
        public int ExperimentIndex { get; init; }
        public int ExperimentNo => ExperimentIndex + 1;
        public int Seed { get; init; }
        public decimal BestFitness { get; init; }
        public int BestGeneration { get; init; }
        public TimeSpan Elapsed { get; init; }
        public InitialData InitialData { get; init; } = null!;
        public bool[,] BestGenotype { get; init; } = null!;
        public IReadOnlyList<PopulationStatistics> Statistics { get; init; } = Array.Empty<PopulationStatistics>();
    }
}








