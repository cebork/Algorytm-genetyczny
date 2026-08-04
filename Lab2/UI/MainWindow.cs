using Lab2.Core.Algorithm;
using Lab2.Core.Domain;
using Lab2.Core.Enums;
using Lab2.Core.Fitness;
using Lab2.Core.Operators.Crossover;
using Lab2.Core.Operators.Mutation;
using Lab2.Core.Operators.Selection;
using Lab2.Core.Random;
using Lab2.Core.Statistics;
using Lab2.Core.Termination;
using Lab2.Infrastructure;
using Lab2.objects;
using Lab2.Services;
using Lab2.UI;
using MathNet.Numerics;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace Lab2
{
    public partial class MainWindow : Form
    {
        private readonly InitialData _data;

        private List<List<Individual>>  _history = new();

        // Stagnation reset controls (added programmatically)
        private CheckBox _stagnationOnCheckBox;
        private NumericUpDown _stagnationWindowInput;
        private NumericUpDown _stagnationFractionInput;
        private NumericUpDown _stagnationDiversityThresholdInput = null!;
        private GroupBox _narrowingMutationGroupBox = null!;
        private GroupBox _bitFlipMutationGroupBox = null!;
        private NumericUpDown _bitFlipEarlyMultiplierInput = null!;
        private NumericUpDown _bitFlipLateMultiplierInput = null!;
        private NumericUpDown _narrowingMultiplierInput = null!;
        private NumericUpDown _narrowingStepInput = null!;
        private NumericUpDown _narrowingExponentInput = null!;
        private CheckBox _limitNarrowingMutationCheckBox = null!;
        private GroupBox _stagnationGroupBox = null!;
        private GroupBox _evenMatrixPaddingGroupBox = null!;
        private NumericUpDown _emptyRowInput = null!;
        private NumericUpDown _emptyColumnInput = null!;
        private System.Windows.Forms.DataVisualization.Charting.Chart _mutationChart = null!;
        private bool _visualLayoutApplied;
        private CheckBox _testSweepNCheckBox = null!;
        private CheckBox _testSweepPkCheckBox = null!;
        private CheckBox _testSweepPmCheckBox = null!;
        private CheckBox _testSweepTCheckBox = null!;
        private CheckBox _testSweepRtCheckBox = null!;
        private CheckBox _testSweepPsCheckBox = null!;
        private CheckBox _testSweepIpkCheckBox = null!;
        private CheckBox _testUseSameSeedsCheckBox = null!;
        private Label _testMaxParallelLabel = null!;
        private NumericUpDown _testMaxParallelInput = null!;
        private DataGridView _testJobsGrid = null!;
        private GroupBox _testRunStatusGroupBox = null!;
        private Label _testStatusStateLabel = null!;
        private Label _testStatusConfigurationLabel = null!;
        private Label _testStatusExperimentLabel = null!;
        private Label _testStatusGenerationLabel = null!;
        private Label _testStatusSeedLabel = null!;
        private Label _testStatusBestLabel = null!;
        private Label _testStatusConfigStatsLabel = null!;
        private Label _testStatusProgressLabel = null!;
        private Label _testStatusElapsedLabel = null!;
        private Label _testStatusAverageLabel = null!;
        private Label _testStatusEtaLabel = null!;
        private Label _testStatusSeedModeLabel = null!;
        private TabPage _runJobsPage = null!;
        private NumericUpDown _runMaxParallelInput = null!;
        private DataGridView _runJobsGrid = null!;
        private GroupBox _runStatusGroupBox = null!;
        private Label _runStatusStateLabel = null!;
        private Label _runStatusExperimentLabel = null!;
        private Label _runStatusGenerationLabel = null!;
        private Label _runStatusSeedLabel = null!;
        private Label _runStatusBestLabel = null!;
        private Label _runStatusProgressLabel = null!;
        private Label _runStatusElapsedLabel = null!;
        private Label _runStatusAverageLabel = null!;
        private Label _runStatusEtaLabel = null!;
        private Label _runStatusActiveLabel = null!;
        private GroupBox _selectedDataPreviewGroupBox = null!;
        private FlowLayoutPanel _selectedDataPreviewFlow = null!;
        private Label _selectedDataPreviewSummaryLabel = null!;
        private CheckBox _archiveResultsCheckBox = null!;
        private GroupBox _advancedSelectionGroupBox = null!;
        private CheckBox _advancedSelectionCheckBox = null!;
        private DataGridView _advancedSelectionGrid = null!;
        private System.Windows.Forms.Button _advancedSelectionAddButton = null!;
        private System.Windows.Forms.Button _advancedSelectionRemoveButton = null!;
        private int? _testSeedOverride;
        private volatile bool _isComputationRunning;

        public MainWindow()
        {
            InitializeComponent();
            _data = new InitialData();
            FormClosing += MainWindow_FormClosing;
        }

        private void MainWindow_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!_isComputationRunning)
                return;

            var result = MessageBox.Show(
                "Trwa obliczanie algorytmu genetycznego. Zamknięcie aplikacji teraz przerwie bieżący przebieg " +
                "i wszystkie niezapisane wyniki zostaną utracone.\n\nCzy na pewno chcesz zamknąć aplikację?",
                "Obliczenia w toku",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
                e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureMatrixSizing();

            supervisedTypedRadioButton.Checked = true;

            LoadLastSeed();
            seed.Enabled = useSeed.Checked;
            CreateStagnationControls();
            CreateBitFlipMutationControls();
            CreateNarrowingMutationControls();
            CreateEvenMatrixPaddingControls();
            CreateAdvancedSelectionControls();
            SetupDefaultAlgorithmOtpions();
            ApplyVisualLayout();
            CreateTestSweepControls();
            CreateTestRunStatusControls();
            CreateTestJobsGridControls();
            CreateRunJobsPageControls();
        }


        private async void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                ReadUiData();
                new ValidationFacade().ValidateOrThrow(_data);
                if (_archiveResultsCheckBox.Checked)
                    FileUtils.ArchiveResults();

                FileUtils.SaveLastSeed(_data.RandomSeed);
                startButton.Enabled = false;
                _isComputationRunning = true;
                await RunGeneticAlgorithm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
            finally
            {
                startButton.Enabled = true;
                _isComputationRunning = false;
            }
        }

        private async Task RunGeneticAlgorithm()
        {
            ClearPreviousRunState();

            int experimentCount = (int)_data.NumberOfExperiments;
            int iterationCount = (int)_data.NumberOfIterations;
            int maxParallel = _runMaxParallelInput == null ? 1 : (int)_runMaxParallelInput.Value;
            var jobs = BuildRunJobs(CloneInitialData(_data), experimentCount);

            runProgressBar.Visible = true;
            runProgressBar.Minimum = 0;
            runProgressBar.Maximum = experimentCount;
            runProgressBar.Value = 0;

            ResetRunStatus();
            InitializeRunJobsGrid(jobs);
            _runStatusStateLabel.Text = "Stan: przygotowanie";
            _runStatusProgressLabel.Text = $"Postęp eksperymentów: 0 / {experimentCount}";
            _runStatusAverageLabel.Text = $"Maks. równoległe: {maxParallel}";
            if (_runJobsPage != null)
                tabs.SelectedTab = _runJobsPage;

            var stopwatch = Stopwatch.StartNew();
            int completedExperiments = 0;
            int activeJobs = 0;

            using var semaphore = new SemaphoreSlim(maxParallel, maxParallel);
            var tasks = jobs.Select(async job =>
            {
                await semaphore.WaitAsync();
                int activeNow = Interlocked.Increment(ref activeJobs);
                var jobStopwatch = Stopwatch.StartNew();

                UpdateRunStatus(() =>
                {
                    _runStatusStateLabel.Text = "Stan: eksperymenty w toku";
                    _runStatusExperimentLabel.Text = $"Eksperyment: {job.ExperimentNo} / {experimentCount}";
                    _runStatusActiveLabel.Text = $"Aktywne: {activeNow} / {maxParallel}";
                    _runStatusSeedLabel.Text = $"Seed: {job.Seed}";
                    _runStatusGenerationLabel.Text = $"Job {job.JobNo}: 0 / {iterationCount}";
                });
                UpdateRunJobGridRow(job, "Uruchomiony", 0, elapsed: TimeSpan.Zero);

                try
                {
                    var result = await Task.Run(() =>
                    {
                        long lastProgressUiUpdate = 0;
                        var ga = CreateGeneticAlgorithm(job.Data, job.Seed);
                        var progress = new Progress<int>(generation =>
                        {
                            long now = Stopwatch.GetTimestamp();
                            bool shouldUpdate = generation == 0 ||
                                generation >= iterationCount ||
                                now - Interlocked.Read(ref lastProgressUiUpdate) >= Stopwatch.Frequency / 4;

                            if (!shouldUpdate)
                                return;

                            Interlocked.Exchange(ref lastProgressUiUpdate, now);
                            UpdateRunJobGridRow(job, "W toku", generation, elapsed: jobStopwatch.Elapsed);
                            UpdateRunStatus(() =>
                            {
                                _runStatusGenerationLabel.Text = $"Job {job.JobNo}: {Math.Min(generation, iterationCount)} / {iterationCount}";
                                _runStatusElapsedLabel.Text = $"Czas: {FormatTestDuration(stopwatch.Elapsed)}";
                            });
                        });

                        ga.Run(progress);

                        var bestStatistic = ga.StatisticsHistory.Count == 0
                            ? null
                            : ga.StatisticsHistory.OrderByDescending(stat => stat.BestFitness).First();

                        return new RunJobResult
                        {
                            Job = job,
                            MinFitness = bestStatistic?.WorstFitness ?? 0m,
                            AvgFitness = bestStatistic?.AverageFitness ?? 0m,
                            BestFitness = bestStatistic?.BestFitness ?? 0m,
                            BestGeneration = bestStatistic?.Generation ?? 0,
                            Elapsed = jobStopwatch.Elapsed,
                            Statistics = ga.StatisticsHistory.ToList(),
                            PerfectSolution = ga.PerfectSolution?.Clone(),
                            PerfectSolutionGeneration = ga.PerfectSolutionGeneration,
                            BestSolution = ga.BestSolution?.Clone(),
                            FinalPopulation = ga.CurrentPopulation.Select(individual => individual.Clone()).ToList()
                        };
                    });

                    jobStopwatch.Stop();
                    int completedExperimentCount = Interlocked.Increment(ref completedExperiments);
                    TimeSpan averageExperimentTime = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / Math.Max(1, completedExperimentCount));
                    TimeSpan eta = TimeSpan.FromTicks(averageExperimentTime.Ticks * Math.Max(0, experimentCount - completedExperimentCount));
                    int progressValue = Math.Min(runProgressBar.Maximum, completedExperimentCount);

                    UpdateRunJobGridRow(job, "Zakończony", iterationCount, result.BestFitness, result.BestGeneration, result.Elapsed);
                    UpdateRunStatus(() =>
                    {
                        runProgressBar.Value = progressValue;
                        _runStatusExperimentLabel.Text = $"Eksperyment: {job.ExperimentNo} / {experimentCount}";
                        _runStatusBestLabel.Text = $"Ostatni wynik: {result.BestFitness:F4} (job {job.JobNo}, gen. {result.BestGeneration})";
                        _runStatusProgressLabel.Text = $"Postęp eksperymentów: {completedExperimentCount} / {experimentCount}";
                        _runStatusElapsedLabel.Text = $"Czas: {FormatTestDuration(stopwatch.Elapsed)}";
                        _runStatusAverageLabel.Text = $"Śr. czas eksperymentu: {FormatTestDuration(averageExperimentTime)}";
                        _runStatusEtaLabel.Text = $"Pozostało: {FormatTestDuration(eta)}";
                    });

                    return result;
                }
                catch
                {
                    UpdateRunJobGridRow(job, "Błąd", elapsed: jobStopwatch.Elapsed);
                    throw;
                }
                finally
                {
                    int activeAfter = Interlocked.Decrement(ref activeJobs);
                    UpdateRunStatus(() =>
                    {
                        _runStatusActiveLabel.Text = $"Aktywne: {activeAfter} / {maxParallel}";
                    });
                    semaphore.Release();
                }
            }).ToArray();

            var runResults = await Task.WhenAll(tasks);
            stopwatch.Stop();
            runProgressBar.Visible = false;

            if (runResults.Length == 0)
                return;

            var cumulativeSuccesses = new int[iterationCount + 1];
            foreach (var result in runResults)
                AddGenerationSuccess(cumulativeSuccesses, result.Statistics);

            var perfectSeedResults = runResults
                .Where(result => result.PerfectSolution != null)
                .Select(result => new PerfectSeedResult
                {
                    ExperimentIndex = result.Job.ExperimentIndex,
                    Seed = result.Job.Seed,
                    Generation = result.PerfectSolutionGeneration ?? 0,
                    Fitness = result.PerfectSolution!.Fitness,
                    Genotype = (bool[,])result.PerfectSolution.Genotype.Clone()
                })
                .ToList();

            FileUtils.SaveCumulativeResults(cumulativeSuccesses, iterationCount);
            FileUtils.SavePerfectSeedResults(perfectSeedResults, _data);

            var detailedRunResults = runResults
                .OrderBy(result => result.Job.ExperimentIndex)
                .Select(result => new DetailedRunObject
                {
                    ExperimentIndex = result.Job.ExperimentIndex,
                    Seed = result.Job.Seed,
                    N = result.Job.Data.NumberOfIndividuals,
                    pk = result.Job.Data.CrossProbability,
                    pm = result.Job.Data.MutationProbability,
                    T = result.Job.Data.NumberOfIterations,
                    Rt = result.Job.Data.TournamentSelectionSize,
                    Ps = result.Job.Data.TournamentSoftSelectionTreshold,
                    Ipk = result.Job.Data.CrossCount,
                    MinMark = result.MinFitness,
                    AvgMark = result.AvgFitness,
                    BestMark = result.BestFitness,
                    BestGeneration = result.BestGeneration,
                    Elapsed = result.Elapsed
                })
                .ToList();
            FileUtils.SaveDetailedGaResults(detailedRunResults, _data);

            var bestRun = runResults
                .OrderByDescending(result => result.BestFitness)
                .ThenBy(result => result.BestGeneration)
                .First();

            var bestGeneration = bestRun.FinalPopulation;
            var bestIndividual = bestRun.BestSolution ?? bestGeneration.OrderByDescending(o => o.Fitness).First();

            FileUtils.SaveFullBestRun(BuildFullRunExport(
                "multirun",
                1,
                bestRun.Job.ExperimentIndex,
                bestRun.Job.Seed,
                bestRun.BestFitness,
                bestRun.BestGeneration,
                bestRun.Elapsed,
                bestRun.Job.Data,
                bestIndividual.Genotype,
                bestRun.Statistics));

            _history.Clear();

            DisplayLastGeneration(bestGeneration);
            DisplayMatrix(GetMatrixForDisplay(bestIndividual.Genotype));
            DrawFitnessChart(bestRun.Statistics);
            DrawCumulativeChart(cumulativeSuccesses, experimentCount);
            DrawMutationChart(iterationCount);

            UpdateRunStatus(() =>
            {
                _runStatusStateLabel.Text = "Stan: zakończono";
                _runStatusBestLabel.Text = $"Najlepszy wynik: {bestRun.BestFitness:F4} (exp. {bestRun.Job.ExperimentNo}, seed {bestRun.Job.Seed}, gen. {bestRun.BestGeneration})";
                _runStatusElapsedLabel.Text = $"Czas: {FormatTestDuration(stopwatch.Elapsed)}";
                _runStatusEtaLabel.Text = "Pozostało: 00:00";
                _runStatusActiveLabel.Text = $"Aktywne: 0 / {maxParallel}";
            });

            var elapsed = stopwatch.Elapsed;
            MessageBox.Show(
                $"Algorytm genetyczny zakończył {experimentCount} niezależnych uruchomień w {elapsed.TotalMilliseconds:N0} ms\n" +
                $"Najlepszy wynik: {bestRun.BestFitness:F4}, eksperyment {bestRun.Job.ExperimentNo}, seed {bestRun.Job.Seed}",
                "Czas wykonania",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

        }
        private void ClearPreviousRunState()
        {
            _history.Clear();

            osobniki.DataSource = null;
            osobniki.Rows.Clear();
            osobniki.Columns.Clear();

            display.Rows.Clear();
            display.Columns.Clear();

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            cumulativeChart.Series.Clear();
            cumulativeChart.ChartAreas.Clear();
            if (_mutationChart != null)
            {
                _mutationChart.Series.Clear();
                _mutationChart.ChartAreas.Clear();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private GeneticAlgorithm CreateGeneticAlgorithm(int experimentIndex)
        {
            return CreateGeneticAlgorithm(_data, GetActiveExperimentSeed(experimentIndex));
        }

        private GeneticAlgorithm CreateGeneticAlgorithm(InitialData data, int seed)
        {
            var random = new SeededRandomProvider(seed);

            var fitness = CreateFitness(data);
            var selection = CreateSelection(data, random);
            var crossover = CreateCrossover(data, random);
            var mutation = CreateMutation(data, random);
            var population = CreateInitialPopulation(data, random);

            var builder = GeneticAlgorithmBuilder
                .Create()
                .WithInitialPopulation(population)
                .WithFitness(fitness)
                .WithSelection(selection)
                .WithCrossover(crossover)
                .WithCrossoverProbability(data.CrossProbability, random)
                .WithElitism(data.EliteOn, data.EliteFraction)
                .StopAtFirstCorrect(data.StopAtFirstCorrect)
                .WithMutation(mutation)
                .WithTermination(
                    new MaxIterationCondition((int)data.NumberOfIterations)
                );

            if (data.StagnationEnabled)
                builder.WithStagnationReset(
                    data.StagnationWindow,
                    data.StagnationResetFraction,
                    random,
                    data.StagnationDiversityThreshold
                );

            return builder.Build();
        }
        private static void AddGenerationSuccess(
            int[] generationSuccesses,
            IReadOnlyList<PopulationStatistics> statistics
        )
        {
            const decimal SuccessThreshold = 0.975m;

            for (int i = 0; i < statistics.Count; i++)
            {
                if (statistics[i].BestFitness >= SuccessThreshold)
                {
                    int generation = Math.Min(statistics[i].Generation, generationSuccesses.Length - 1);
                    generationSuccesses[generation]++;
                    return;
                }
            }
        }

        private void CreateAdvancedSelectionControls()
        {
            _advancedSelectionGroupBox = new GroupBox
            {
                Text = "Zaawansowana selekcja",
                Size = new System.Drawing.Size(420, 128),
                Enabled = false
            };

            _advancedSelectionCheckBox = new CheckBox
            {
                Text = "Łącz selekcje wg progu generacji",
                AutoSize = true,
                Location = new System.Drawing.Point(8, 20)
            };
            _advancedSelectionCheckBox.CheckedChanged += (_, _) => UpdateAdvancedSelectionControls();

            _advancedSelectionGrid = new DataGridView
            {
                Location = new System.Drawing.Point(8, 46),
                Size = new System.Drawing.Size(288, 72),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            var selectionColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Selekcja",
                Name = "SelectionType",
                ValueType = typeof(SelectionType),
                FlatStyle = FlatStyle.Flat
            };
            selectionColumn.Items.Add(SelectionType.ROULETTE);
            selectionColumn.Items.Add(SelectionType.TOURNAMENT_HARD);
            selectionColumn.Items.Add(SelectionType.TOURNAMENT_SOFT);
            selectionColumn.Items.Add(SelectionType.RANKING);

            var thresholdColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Próg",
                Name = "Threshold"
            };

            _advancedSelectionGrid.Columns.Add(selectionColumn);
            _advancedSelectionGrid.Columns.Add(thresholdColumn);
            _advancedSelectionGrid.Rows.Add(SelectionType.ROULETTE, 0.2m);
            _advancedSelectionGrid.Rows.Add(SelectionType.TOURNAMENT_HARD, 1.0m);

            _advancedSelectionAddButton = new System.Windows.Forms.Button
            {
                Text = "Dodaj",
                Location = new System.Drawing.Point(305, 46),
                Size = new System.Drawing.Size(96, 28)
            };
            _advancedSelectionAddButton.Click += (_, _) => AddAdvancedSelectionStageRow();

            _advancedSelectionRemoveButton = new System.Windows.Forms.Button
            {
                Text = "Usuń",
                Location = new System.Drawing.Point(305, 82),
                Size = new System.Drawing.Size(96, 28)
            };
            _advancedSelectionRemoveButton.Click += (_, _) => RemoveAdvancedSelectionStageRow();

            _advancedSelectionGroupBox.Controls.Add(_advancedSelectionCheckBox);
            _advancedSelectionGroupBox.Controls.Add(_advancedSelectionGrid);
            _advancedSelectionGroupBox.Controls.Add(_advancedSelectionAddButton);
            _advancedSelectionGroupBox.Controls.Add(_advancedSelectionRemoveButton);
            Controls.Add(_advancedSelectionGroupBox);
            UpdateAdvancedSelectionControls();
        }

        private void UpdateAdvancedSelectionControls()
        {
            if (_advancedSelectionGrid == null)
                return;

            bool enabled = _advancedSelectionGroupBox.Enabled && _advancedSelectionCheckBox.Checked;
            _advancedSelectionGrid.Enabled = enabled;
            _advancedSelectionAddButton.Enabled = enabled;
            _advancedSelectionRemoveButton.Enabled = enabled;
        }

        private void AddAdvancedSelectionStageRow()
        {
            decimal lastThreshold = 0m;
            if (_advancedSelectionGrid.Rows.Count > 0 &&
                TryReadThreshold(_advancedSelectionGrid.Rows[^1].Cells[1].Value, out decimal parsedThreshold))
            {
                lastThreshold = parsedThreshold;
            }

            if (lastThreshold >= 1m)
            {
                MessageBox.Show("Nie można dodać kolejnego etapu po progu 1.0.", "Zaawansowana selekcja");
                return;
            }

            decimal nextThreshold = Math.Min(1m, lastThreshold + 0.2m);
            _advancedSelectionGrid.Rows.Add(SelectionType.ROULETTE, nextThreshold);
        }

        private void RemoveAdvancedSelectionStageRow()
        {
            if (_advancedSelectionGrid.Rows.Count <= 0)
                return;

            int index = _advancedSelectionGrid.SelectedRows.Count > 0
                ? _advancedSelectionGrid.SelectedRows[0].Index
                : _advancedSelectionGrid.Rows.Count - 1;

            _advancedSelectionGrid.Rows.RemoveAt(index);
        }

        private List<SelectionStage> GetAdvancedSelectionStagesFromGrid()
        {
            var stages = new List<SelectionStage>();

            if (_advancedSelectionGrid == null)
                return stages;

            foreach (DataGridViewRow row in _advancedSelectionGrid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var selectionValue = row.Cells[0].Value;
                var thresholdValue = row.Cells[1].Value;

                SelectionType selectionType = selectionValue is SelectionType typedSelection
                    ? typedSelection
                    : Enum.TryParse(selectionValue?.ToString(), out SelectionType parsedSelection)
                        ? parsedSelection
                        : SelectionType.ROULETTE;

                if (!TryReadThreshold(thresholdValue, out decimal threshold))
                    throw new InvalidOperationException("Próg zaawansowanej selekcji musi być liczbą z zakresu 0..1.");

                stages.Add(new SelectionStage
                {
                    SelectionType = selectionType,
                    Threshold = threshold
                });
            }

            return stages;
        }

        private static bool TryReadThreshold(object value, out decimal threshold)
        {
            if (value is decimal decimalValue)
            {
                threshold = decimalValue;
                return true;
            }

            string text = Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;
            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out threshold) ||
                   decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out threshold);
        }
        private void CreateStagnationControls()
        {
            // GroupBox positioned below the eliteGroupBox (1644, 79 + 153 + 8)
            var group = new GroupBox
            {
                Text = "Stagnacja",
                Location = new System.Drawing.Point(1644, 240),
                Size = new System.Drawing.Size(200, 190)
            };

            _stagnationOnCheckBox = new CheckBox
            {
                Text = "Włącz",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 22)
            };

            var windowLabel = new Label
            {
                Text = "Okno stagnacji (gen.)",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 52)
            };

            _stagnationWindowInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 68),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 5,
                Maximum = 500,
                Value = 50,
                Increment = 5
            };

            var fractionLabel = new Label
            {
                Text = "Odsetek wymiany",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 100)
            };

            _stagnationFractionInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 116),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 0.01m,
                Maximum = 0.5m,
                Value = 0.1m,
                Increment = 0.05m,
                DecimalPlaces = 2
            };

            var diversityLabel = new Label
            {
                Text = "Próg różnorodności",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 146)
            };

            _stagnationDiversityThresholdInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 162),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 0m,
                Maximum = 1m,
                Value = 0.05m,
                Increment = 0.01m,
                DecimalPlaces = 2
            };

            group.Controls.Add(_stagnationOnCheckBox);
            group.Controls.Add(windowLabel);
            group.Controls.Add(_stagnationWindowInput);
            group.Controls.Add(fractionLabel);
            group.Controls.Add(_stagnationFractionInput);
            group.Controls.Add(diversityLabel);
            group.Controls.Add(_stagnationDiversityThresholdInput);
            _stagnationGroupBox = group;
            Controls.Add(group);
        }

        private void ApplyVisualLayout()
        {
            if (_visualLayoutApplied)
                return;

            _visualLayoutApplied = true;
            SuspendLayout();

            Text = $"Praca magisterska v{GetApplicationVersion()}";
            MinimumSize = new System.Drawing.Size(1180, 760);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(8),
                BackColor = System.Drawing.SystemColors.Control
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 310));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var configurationTabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Name = "configurationTabs"
            };

            var basicTab = CreateConfigurationTab("Podstawowe");
            var operatorsTab = CreateConfigurationTab("Operatory");
            var advancedTab = CreateConfigurationTab("Zaawansowane");

            configurationTabs.TabPages.Add(basicTab);
            configurationTabs.TabPages.Add(operatorsTab);
            configurationTabs.TabPages.Add(advancedTab);

            Controls.Add(root);
            root.BringToFront();
            root.Controls.Add(configurationTabs, 0, 0);
            root.Controls.Add(tabs, 0, 1);

            tabs.Dock = DockStyle.Fill;
            tabs.Margin = new Padding(0, 8, 0, 0);
            CreateMutationChartTab();

            MoveControl(algorithmTypeGroupBox, basicTab, 12, 12);
            MoveControl(probGen1Group, basicTab, 265, 12);
            MoveControl(stopAtFirstCorrect, basicTab, 380, 27);
            MoveControl(inputDataGroupBox, basicTab, 12, 72);
            MoveControl(GAmodification, basicTab, 810, 72);

            CreateSelectedDataPreviewControls(basicTab);

            var runGroup = CreateRunGroup();
            basicTab.Controls.Add(runGroup);
            runGroup.Location = new System.Drawing.Point(970, 72);

            MoveControl(selectionGroup, operatorsTab, 12, 12);
            MoveControl(_advancedSelectionGroupBox, operatorsTab, 12, 175);
            MoveControl(crossGroup, operatorsTab, 230, 12);
            MoveControl(mutationGroup, operatorsTab, 345, 12);
            MoveControl(_bitFlipMutationGroupBox, operatorsTab, 480, 12);
            MoveControl(_narrowingMutationGroupBox, operatorsTab, 690, 12);

            MoveControl(unformBlocksGroupBox, advancedTab, 12, 12);
            MoveControl(eliteGroupBox, advancedTab, 270, 12);
            MoveControl(_stagnationGroupBox, advancedTab, 490, 12);
            MoveControl(_evenMatrixPaddingGroupBox, advancedTab, 710, 12);
            MoveControl(iterations, advancedTab, 930, 32);

            startButton.Text = "Uruchom";
            startButton.Width = 150;
            historyViewButton.Width = 150;
            runProgressBar.Width = 190;
            runProgressBar.Visible = false;

            ResumeLayout(true);
        }

        private static TabPage CreateConfigurationTab(string text)
        {
            return new TabPage(text)
            {
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = System.Drawing.SystemColors.Control
            };
        }

        private static string GetApplicationVersion()
        {
            var version = typeof(MainWindow).Assembly.GetName().Version;
            return version == null
                ? "1.0.0"
                : $"{version.Major}.{version.Minor}.{version.Build}";
        }

        private void CreateSelectedDataPreviewControls(System.Windows.Forms.Control parent)
        {
            _selectedDataPreviewGroupBox = new GroupBox
            {
                Text = "Podgląd danych",
                Size = new System.Drawing.Size(360, 180)
            };

            _selectedDataPreviewSummaryLabel = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(10, 22),
                MaximumSize = new System.Drawing.Size(335, 0)
            };

            _selectedDataPreviewFlow = new FlowLayoutPanel
            {
                Location = new System.Drawing.Point(10, 48),
                Size = new System.Drawing.Size(340, 122),
                AutoScroll = true,
                WrapContents = true
            };

            _selectedDataPreviewGroupBox.Controls.Add(_selectedDataPreviewSummaryLabel);
            _selectedDataPreviewGroupBox.Controls.Add(_selectedDataPreviewFlow);
            MoveControl(_selectedDataPreviewGroupBox, parent, 1215, 72);
            UpdateSelectedDataPreview();
        }

        private void UpdateSelectedDataPreview()
        {
            if (_selectedDataPreviewFlow == null || _selectedDataPreviewSummaryLabel == null)
                return;

            _selectedDataPreviewFlow.SuspendLayout();
            _selectedDataPreviewFlow.Controls.Clear();

            if (_data.AlgorithmType == AlgorithmType.UNSUPERVISED)
            {
                var patterns = _data.UnsupervisedPatternMatrixes;
                if (patterns == null || patterns.Length == 0)
                {
                    _selectedDataPreviewSummaryLabel.Text = "Wybrane patterny: brak";
                }
                else
                {
                    _selectedDataPreviewSummaryLabel.Text = $"Wybrane patterny: {patterns.Length}";
                    for (int i = 0; i < patterns.Length; i++)
                    {
                        _selectedDataPreviewFlow.Controls.Add(CreateMatrixPreviewTile($"Pattern {i + 1}", patterns[i]));
                    }
                }
            }
            else
            {
                var matrix = _data.SupervisedReferenceMatrix;
                if (matrix == null || matrix.Length == 0)
                {
                    _selectedDataPreviewSummaryLabel.Text = "Macierz referencyjna: brak";
                }
                else
                {
                    _selectedDataPreviewSummaryLabel.Text = $"Macierz referencyjna: {matrix.GetLength(0)} x {matrix.GetLength(1)}";
                    _selectedDataPreviewFlow.Controls.Add(CreateMatrixPreviewTile("Referencyjna", matrix));
                }
            }

            _selectedDataPreviewFlow.ResumeLayout();
        }

        private static System.Windows.Forms.Control CreateMatrixPreviewTile(string title, bool[,] matrix)
        {
            var tile = new Panel
            {
                Size = new System.Drawing.Size(96, 104),
                Margin = new Padding(4),
                BackColor = System.Drawing.SystemColors.ControlLightLight,
                BorderStyle = BorderStyle.FixedSingle
            };

            var label = new Label
            {
                Text = title,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Location = new System.Drawing.Point(3, 3),
                Size = new System.Drawing.Size(88, 18)
            };

            var matrixPanel = new Panel
            {
                Location = new System.Drawing.Point(10, 26),
                Size = new System.Drawing.Size(74, 74),
                BackColor = Color.White
            };

            matrixPanel.Paint += (_, e) => PaintMatrixPreview(e.Graphics, matrixPanel.ClientRectangle, matrix);
            matrixPanel.Resize += (_, _) => matrixPanel.Invalidate();

            tile.Controls.Add(label);
            tile.Controls.Add(matrixPanel);
            return tile;
        }

        private static void PaintMatrixPreview(Graphics graphics, Rectangle bounds, bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            if (rows == 0 || cols == 0)
                return;

            float cellWidth = bounds.Width / (float)cols;
            float cellHeight = bounds.Height / (float)rows;

            using var redBrush = new SolidBrush(Color.Red);
            using var whiteBrush = new SolidBrush(Color.White);
            using var gridPen = new Pen(Color.Gainsboro);

            graphics.FillRectangle(whiteBrush, bounds);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var cell = new RectangleF(
                        bounds.Left + col * cellWidth,
                        bounds.Top + row * cellHeight,
                        cellWidth,
                        cellHeight);

                    graphics.FillRectangle(matrix[row, col] ? redBrush : whiteBrush, cell);
                    if (cellWidth >= 4 && cellHeight >= 4)
                        graphics.DrawRectangle(gridPen, cell.X, cell.Y, cell.Width, cell.Height);
                }
            }

            graphics.DrawRectangle(Pens.Gray, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1);
        }
        private GroupBox CreateRunGroup()
        {
            var runGroup = new GroupBox
            {
                Text = "Uruchamianie",
                Size = new System.Drawing.Size(235, 215)
            };

            _archiveResultsCheckBox = new CheckBox
            {
                Text = "Archiwizuj wyniki",
                AutoSize = true,
                Checked = true
            };

            var runMaxParallelLabel = new Label
            {
                Text = "Maks. równoległe",
                AutoSize = true,
                Location = new System.Drawing.Point(16, 137)
            };

            _runMaxParallelInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(136, 133),
                Size = new System.Drawing.Size(70, 23),
                Minimum = 1,
                Maximum = 1024,
                Value = 1
            };

            MoveControl(startButton, runGroup, 16, 28);
            MoveControl(runProgressBar, runGroup, 16, 66);
            MoveControl(historyViewButton, runGroup, 16, 100);
            runGroup.Controls.Add(runMaxParallelLabel);
            runGroup.Controls.Add(_runMaxParallelInput);
            MoveControl(_archiveResultsCheckBox, runGroup, 16, 172);

            return runGroup;
        }
        private void CreateMutationChartTab()
        {
            if (_mutationChart != null)
                return;

            var mutationChartPage = new TabPage
            {
                Text = "Mutacja",
                Padding = new Padding(3),
                UseVisualStyleBackColor = true
            };

            _mutationChart = new System.Windows.Forms.DataVisualization.Charting.Chart
            {
                Dock = DockStyle.Fill,
                Text = "mutationChart"
            };

            _mutationChart.Legends.Add(new Legend());
            mutationChartPage.Controls.Add(_mutationChart);
            tabs.TabPages.Add(mutationChartPage);
        }

        private static void MoveControl(System.Windows.Forms.Control control, System.Windows.Forms.Control parent, int x, int y)
        {
            control.Parent = parent;
            control.Location = new System.Drawing.Point(x, y);
        }

        private void CreateBitFlipMutationControls()
        {
            _bitFlipMutationGroupBox = new GroupBox
            {
                Text = "Mutacja równomierna",
                Location = new System.Drawing.Point(1644, 220),
                Size = new System.Drawing.Size(200, 122),
                Enabled = false
            };

            var earlyLabel = new Label
            {
                Text = "Mnożnik początkowy",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 24)
            };

            _bitFlipEarlyMultiplierInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 40),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 0m,
                Maximum = 10m,
                Value = 1.6m,
                Increment = 0.1m,
                DecimalPlaces = 2
            };

            var lateLabel = new Label
            {
                Text = "Mnożnik końcowy",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 72)
            };

            _bitFlipLateMultiplierInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 88),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 0m,
                Maximum = 10m,
                Value = 0.7m,
                Increment = 0.1m,
                DecimalPlaces = 2
            };

            _bitFlipMutationGroupBox.Controls.Add(earlyLabel);
            _bitFlipMutationGroupBox.Controls.Add(_bitFlipEarlyMultiplierInput);
            _bitFlipMutationGroupBox.Controls.Add(lateLabel);
            _bitFlipMutationGroupBox.Controls.Add(_bitFlipLateMultiplierInput);
            Controls.Add(_bitFlipMutationGroupBox);
        }

        private void CreateNarrowingMutationControls()
        {
            _narrowingMutationGroupBox = new GroupBox
            {
                Text = "Mutacja zwężająca",
                Location = new System.Drawing.Point(1644, 440),
                Size = new System.Drawing.Size(200, 215),
                Enabled = false
            };

            var multiplierLabel = new Label
            {
                Text = "Mnożnik",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 24)
            };

            _narrowingMultiplierInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 40),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 0m,
                Maximum = 10m,
                Value = 1m,
                Increment = 0.1m,
                DecimalPlaces = 2
            };

            var stepLabel = new Label
            {
                Text = "Krok zejścia (gen.)",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 72)
            };

            _narrowingStepInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 88),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 1,
                Maximum = 10000,
                Value = 1,
                Increment = 1
            };

            var exponentLabel = new Label
            {
                Text = "Wykładnik b",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 120)
            };

            _narrowingExponentInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 136),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 0m,
                Maximum = 10m,
                Value = 1m,
                Increment = 0.1m,
                DecimalPlaces = 2
            };

            _limitNarrowingMutationCheckBox = new CheckBox
            {
                Text = "Limit dolny do pm",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 170)
            };

            _narrowingMutationGroupBox.Controls.Add(multiplierLabel);
            _narrowingMutationGroupBox.Controls.Add(_narrowingMultiplierInput);
            _narrowingMutationGroupBox.Controls.Add(stepLabel);
            _narrowingMutationGroupBox.Controls.Add(_narrowingStepInput);
            _narrowingMutationGroupBox.Controls.Add(exponentLabel);
            _narrowingMutationGroupBox.Controls.Add(_narrowingExponentInput);
            _narrowingMutationGroupBox.Controls.Add(_limitNarrowingMutationCheckBox);
            Controls.Add(_narrowingMutationGroupBox);
        }

        private void CreateEvenMatrixPaddingControls()
        {
            _evenMatrixPaddingGroupBox = new GroupBox
            {
                Text = "Parzysty rozmiar",
                Location = new System.Drawing.Point(1644, 570),
                Size = new System.Drawing.Size(200, 125),
                Enabled = false
            };

            var rowLabel = new Label
            {
                Text = "Pusty wiersz",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 24)
            };

            _emptyRowInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 40),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 1,
                Maximum = 1,
                Value = 1
            };

            var columnLabel = new Label
            {
                Text = "Pusta kolumna",
                AutoSize = true,
                Location = new System.Drawing.Point(6, 72)
            };

            _emptyColumnInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(6, 88),
                Size = new System.Drawing.Size(138, 23),
                Minimum = 1,
                Maximum = 1,
                Value = 1
            };

            _evenMatrixPaddingGroupBox.Controls.Add(rowLabel);
            _evenMatrixPaddingGroupBox.Controls.Add(_emptyRowInput);
            _evenMatrixPaddingGroupBox.Controls.Add(columnLabel);
            _evenMatrixPaddingGroupBox.Controls.Add(_emptyColumnInput);
            Controls.Add(_evenMatrixPaddingGroupBox);

            UpdateEvenMatrixPaddingControls();
        }

        private void ConfigureMatrixSizing()
        {
            int requestedSize = (int)matrixSizeInput.Value;
            bool usePadding = requestedSize > 1 && requestedSize % 2 == 0;

            _data.RequestedMatrixSize = requestedSize;
            _data.UseEvenMatrixPadding = usePadding;
            _data.MatrixSize = usePadding ? requestedSize - 1 : requestedSize;

            if (_emptyRowInput != null && _emptyColumnInput != null)
            {
                _data.EmptyRowIndex = (int)_emptyRowInput.Value;
                _data.EmptyColumnIndex = (int)_emptyColumnInput.Value;
            }
        }

        private void UpdateEvenMatrixPaddingControls()
        {
            if (_evenMatrixPaddingGroupBox == null)
                return;

            int requestedSize = (int)matrixSizeInput.Value;
            bool enabled = requestedSize > 1 && requestedSize % 2 == 0;

            _evenMatrixPaddingGroupBox.Enabled = enabled;
            _emptyRowInput.Maximum = Math.Max(1, requestedSize);
            _emptyColumnInput.Maximum = Math.Max(1, requestedSize);

            if (_emptyRowInput.Value > _emptyRowInput.Maximum)
                _emptyRowInput.Value = _emptyRowInput.Maximum;

            if (_emptyColumnInput.Value > _emptyColumnInput.Maximum)
                _emptyColumnInput.Value = _emptyColumnInput.Maximum;
        }

        private void ReadUiData()
        {
            ConfigureMatrixSizing();
            UpdateEvenMatrixPaddingControls();
            _data.NumberOfIndividuals = individualNumberInput.Value;
            _data.NumberOfIterations = iterationNumberInput.Value;
            _data.NumberOfExperiments = experimentNumber.Value;
            _data.MutationProbability = mutationProbabilityInput.Value;
            _data.CrossProbability = crossProbabilityInput.Value;
            _data.CrossCount = crossPoints.Value;
            _data.EliteOn = eliteOn.Checked;
            _data.EliteFraction = eliteToMove.Value / 100m;
            _data.StopAtFirstCorrect = stopAtFirstCorrect.Checked;
            _data.ProbGen1 = propGen1.Value;
            _data.TournamentSelectionSize = tournamentSizeInput.Value;
            _data.TournamentSoftSelectionTreshold = tournamentTresholdInput.Value;
            _data.AdvancedSelectionEnabled = _advancedSelectionCheckBox.Checked;
            _data.SelectionStages = GetAdvancedSelectionStagesFromGrid();
            _data.UniformBlockMutationProbWhite = uniformProbWhite.Value;
            _data.UniformBlockMutationProbRed = uniformProbRed.Value;
            _data.BitFlipEarlyExplorationMultiplier = _bitFlipEarlyMultiplierInput.Value;
            _data.BitFlipLateExplorationMultiplier = _bitFlipLateMultiplierInput.Value;
            if (_data.AlgorithmOption == AlgorithmOption.CLASSICAL)
            {
                _data.BitFlipEarlyExplorationMultiplier = 1m;
                _data.BitFlipLateExplorationMultiplier = 1m;
            }
            _data.NarrowingMutationMultiplier = _narrowingMultiplierInput.Value;
            _data.NarrowingMutationStep = (int)_narrowingStepInput.Value;
            _data.NarrowingMutationExponent = _narrowingExponentInput.Value;
            _data.LimitNarrowingMutation = _limitNarrowingMutationCheckBox.Checked;
            _data.StagnationEnabled = _stagnationOnCheckBox.Checked;
            _data.StagnationWindow = (int)_stagnationWindowInput.Value;
            _data.StagnationResetFraction = _stagnationFractionInput.Value;
            _data.StagnationDiversityThreshold = _stagnationDiversityThresholdInput.Value;
            _data.EmptyRowIndex = (int)_emptyRowInput.Value;
            _data.EmptyColumnIndex = (int)_emptyColumnInput.Value;
            _data.UseSeed = useSeed.Checked;
            _data.RandomSeed = ResolveRunSeed();
            seed.Text = _data.RandomSeed.ToString(CultureInfo.InvariantCulture);
        }

        private void LoadLastSeed()
        {
            int? lastSeed = FileUtils.LoadLastSeed();
            if (lastSeed.HasValue)
                seed.Text = lastSeed.Value.ToString(CultureInfo.InvariantCulture);
        }

        private int ResolveRunSeed()
        {
            if (!useSeed.Checked)
                return Environment.TickCount;

            if (int.TryParse(seed.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedSeed))
                return parsedSeed;

            throw new InvalidOperationException("Seed musi być liczbą całkowitą.");
        }

        private IRandomProvider CreateRandomProvider(int experimentIndex)
        {
            return new SeededRandomProvider(GetActiveExperimentSeed(experimentIndex));
        }

        private int GetActiveExperimentSeed(int experimentIndex)
        {
            return _testSeedOverride ?? GetExperimentSeed(experimentIndex);
        }

        private int GetExperimentSeed(int experimentIndex)
        {
            return unchecked(_data.RandomSeed + experimentIndex);
        }

        private int GetIndependentTestSeed(int configurationIndex, int experimentIndex, int experimentCount)
        {
            return unchecked(_data.RandomSeed + configurationIndex * experimentCount + experimentIndex);
        }

        private IFitnessEvaluator CreateFitness()
        {
            return CreateFitness(_data);
        }

        private IFitnessEvaluator CreateFitness(InitialData data)
        {
            return data.AlgorithmType == AlgorithmType.SUPERVISED
                ? new SupervisedFitnessEvaluator(
                    GetReferenceMatrixForFitness(data),
                    precisionDigits: 4
                )
                : new UnsupervisedPatternFitnessEvaluator(
                    data.UnsupervisedPatternMatrixes,
                    precisionDigits: 4
                );
        }

        private ISelectionStrategy CreateSelection(IRandomProvider random)
        {
            return CreateSelection(_data, random);
        }

        private ISelectionStrategy CreateSelection(InitialData data, IRandomProvider random)
        {
            if (data.AdvancedSelectionEnabled)
            {
                var stages = data.SelectionStages
                    .Select(stage => new ThresholdSelectionStage(
                        CreateSingleSelection(stage.SelectionType, data, random),
                        stage.Threshold))
                    .ToList();

                return new ThresholdSelectionStrategy(stages);
            }

            return CreateSingleSelection(data.SelectionType, data, random);
        }

        private ISelectionStrategy CreateSingleSelection(SelectionType selectionType, InitialData data, IRandomProvider random)
        {
            return selectionType switch
            {
                SelectionType.ROULETTE =>
                    new RouletteSelection(random),

                SelectionType.TOURNAMENT_HARD =>
                    new TournamentSelection(
                        (int)data.TournamentSelectionSize,
                        random
                    ),

                SelectionType.TOURNAMENT_SOFT =>
                    new SoftTournamentSelection(
                        (int)data.TournamentSelectionSize,
                        data.TournamentSoftSelectionTreshold,
                        random
                    ),

                SelectionType.RANKING =>
                    new RankingSelection(random),

                _ => throw new InvalidOperationException("Nieznany typ selekcji")
            };
        }


        private ICrossoverOperator CreateCrossover(IRandomProvider random)
        {
            return CreateCrossover(_data, random);
        }

        private ICrossoverOperator CreateCrossover(InitialData data, IRandomProvider random)
        {
            return data.CrossType switch
            {
                CrossType.SINGLE_POINT =>
                    new OnePointCrossover(random),

                CrossType.MULTI_POINT =>
                    new MultiPointCrossover(
                        (int)data.CrossCount,
                        random
                    ),

                _ => throw new InvalidOperationException("Nieznany typ krzyżowania")
            };
        }

        private IMutationOperator CreateMutation(IRandomProvider random)
        {
            return CreateMutation(_data, random);
        }

        private IMutationOperator CreateMutation(InitialData data, IRandomProvider random)
        {
            var mutations = new List<IMutationOperator>();

            mutations.Add(data.MutationType switch
            {
                MutationType.EQUALY =>
                    new BitFlipMutation(
                        data.MutationProbability,
                        data.BitFlipEarlyExplorationMultiplier,
                        data.BitFlipLateExplorationMultiplier,
                        random
                    ),

                MutationType.BIT_SWAPING =>
                    new BitSwapMutation(data.MutationProbability, random),

                MutationType.RANDOM_COORDS =>
                    new NarrowingMutation(
                        data.MutationProbability,
                        data.NarrowingMutationMultiplier,
                        data.NarrowingMutationStep,
                        data.NarrowingMutationExponent,
                        data.LimitNarrowingMutation,
                        random
                    ),

                _ => throw new InvalidOperationException()
            });

            if (data.UniformBlock)
            {
                mutations.Add(
                    new UniformBlockMutation(
                        data.UniformBlockMutationProbWhite,
                        data.UniformBlockMutationProbRed,
                        random
                    )
                );
            }

            return mutations.Count == 1
                ? mutations[0]
                : new CompositeMutation(mutations.ToArray());
        }

        private List<Individual> CreateInitialPopulation(IRandomProvider random)
        {
            return CreateInitialPopulation(_data, random);
        }

        private List<Individual> CreateInitialPopulation(InitialData data, IRandomProvider random)
        {
            return Enumerable.Range(0, (int)data.NumberOfIndividuals)
                .Select(_ =>
                    new Individual(
                        InitialGenotypeFactory.Create(
                            (int)data.MatrixSize,
                            data.ProbGen1,
                            random
                        )
                    )
                )
                .ToList();
        }
        private void DisplayLastGeneration(IReadOnlyList<Individual> lastGeneration)
        {
            int totalCount = lastGeneration.Count;
            int lp = 1;

            var sumUps = lastGeneration
                .OrderByDescending(i => i.Fitness)
                .GroupBy(i => i.Fitness)
                .Select(group => new SumUp
                {
                    lp = lp++,
                    Matrix = group.First().Genotype,
                    f_C_Corr = group.Key,
                    Percentage = (decimal)group.Count() / totalCount * 100m
                })
                .ToList();

            osobniki.DataSource = sumUps;
        }


        private void DrawFitnessChart(IReadOnlyList<PopulationStatistics> stats)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            chart1.Series.Add(CreateSeries(
                "Maximum",
                Color.Blue,
                stats.Select(s => (s.Generation, s.BestFitness))
            ));

            chart1.Series.Add(CreateSeries(
                "Average",
                Color.Red,
                stats.Select(s => (s.Generation, s.AverageFitness))
            ));

            chart1.Series.Add(CreateSeries(
                "Minimum",
                Color.Green,
                stats.Select(s => (s.Generation, s.WorstFitness))
            ));

            var chartArea = new ChartArea
            {
                AxisX =
            {
                Title = "Generations",
                Minimum = 0,
                IsMarginVisible = false,
                TitleFont = new Font("Arial", 15),
                LabelStyle = { Font = new Font("Arial", 14) }
            },
                    AxisY =
            {
                Title = "Fitness",
                Minimum = 0.0,
                Maximum = 1.0,
                TitleFont = new Font("Arial", 15),
                LabelStyle = { Font = new Font("Arial", 14) }
            },
                BackColor = Color.White
            };

            chart1.ChartAreas.Add(chartArea);
            chart1.Legends[0].Font = new Font("Arial", 15);
        }

        private void DrawCumulativeChart(int[] cumulativeSuccesses, int experimentCount)
        {
            cumulativeChart.Series.Clear();
            cumulativeChart.ChartAreas.Clear();

            var series = new Series
            {
                Name = "Cumulative",
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
                BorderWidth = 2
            };

            int cumulativeSuccessCount = 0;
            for (int generation = 0; generation < cumulativeSuccesses.Length; generation++)
            {
                cumulativeSuccessCount += cumulativeSuccesses[generation];
                decimal percentage = experimentCount == 0
                    ? 0
                    : cumulativeSuccessCount * 100m / experimentCount;

                series.Points.AddXY(generation, percentage);
            }

            var chartArea = new ChartArea
            {
                AxisX =
                {
                    Title = "Generations",
                    Minimum = 0,
                    IsMarginVisible = false,
                    TitleFont = new Font("Arial", 15),
                    LabelStyle = { Font = new Font("Arial", 14) }
                },
                AxisY =
                {
                    Title = "Solved experiments [%]",
                    Minimum = 0.0,
                    Maximum = 100.0,
                    TitleFont = new Font("Arial", 15),
                    LabelStyle = { Font = new Font("Arial", 14) }
                },
                BackColor = Color.White
            };

            cumulativeChart.ChartAreas.Add(chartArea);
            cumulativeChart.Series.Add(series);

            if (cumulativeChart.Legends.Count == 0)
                cumulativeChart.Legends.Add(new Legend());

            cumulativeChart.Legends[0].Font = new Font("Arial", 15);
        }

        private void DrawMutationChart(int iterationCount)
        {
            if (_mutationChart == null)
                return;

            _mutationChart.Series.Clear();
            _mutationChart.ChartAreas.Clear();

            var points = Enumerable.Range(0, iterationCount + 1)
                .Select(iteration =>
                {
                    decimal probability = _data.MutationType == MutationType.RANDOM_COORDS
                        ? NarrowingMutation.CalculateEffectiveProbability(
                            _data.MutationProbability,
                            _data.NarrowingMutationMultiplier,
                            _data.NarrowingMutationStep,
                            _data.NarrowingMutationExponent,
                            _data.LimitNarrowingMutation,
                            iteration,
                            iterationCount
                        )
                        : _data.MutationProbability;

                    return (iteration, probability);
                })
                .ToList();

            double yMaximum = Math.Max(0.01, (double)points.Max(point => point.probability));

            _mutationChart.Series.Add(CreateSeries(
                "p*",
                Color.DarkOrange,
                points
            ));

            var chartArea = new ChartArea
            {
                AxisX =
                {
                    Title = "Generations",
                    Minimum = 0,
                    IsMarginVisible = false,
                    TitleFont = new Font("Arial", 15),
                    LabelStyle = { Font = new Font("Arial", 14) }
                },
                AxisY =
                {
                    Title = "Mutation probability",
                    Minimum = 0.0,
                    Maximum = yMaximum,
                    TitleFont = new Font("Arial", 15),
                    LabelStyle = { Font = new Font("Arial", 14) }
                },
                BackColor = Color.White
            };

            _mutationChart.ChartAreas.Add(chartArea);

            if (_mutationChart.Legends.Count == 0)
                _mutationChart.Legends.Add(new Legend());

            _mutationChart.Legends[0].Font = new Font("Arial", 15);
        }

        private Series CreateSeries(
            string name,
            Color color,
            IEnumerable<(int x, decimal y)> points
        )
        {
            var series = new Series
            {
                Name = name,
                ChartType = SeriesChartType.Line,
                Color = color,
                BorderWidth = 2
            };

            foreach (var (x, y) in points)
            {
                series.Points.AddXY(x, y);
            }

            return series;
        }




        private sealed class RunJob
        {
            public int JobNo { get; init; }
            public int ExperimentIndex { get; init; }
            public int ExperimentNo => ExperimentIndex + 1;
            public int Seed { get; init; }
            public InitialData Data { get; init; } = null!;
            public int GridRowIndex { get; set; } = -1;
        }

        private sealed class RunJobResult
        {
            public RunJob Job { get; init; } = null!;
            public decimal MinFitness { get; init; }
            public decimal AvgFitness { get; init; }
            public decimal BestFitness { get; init; }
            public int BestGeneration { get; init; }
            public TimeSpan Elapsed { get; init; }
            public List<PopulationStatistics> Statistics { get; init; } = new();
            public Individual? PerfectSolution { get; init; }
            public int? PerfectSolutionGeneration { get; init; }
            public Individual? BestSolution { get; init; }
            public List<Individual> FinalPopulation { get; init; } = new();
        }

        private List<RunJob> BuildRunJobs(InitialData baseData, int experimentCount)
        {
            var jobs = new List<RunJob>();
            for (int experimentIndex = 0; experimentIndex < experimentCount; experimentIndex++)
            {
                jobs.Add(new RunJob
                {
                    JobNo = experimentIndex + 1,
                    ExperimentIndex = experimentIndex,
                    Seed = unchecked(baseData.RandomSeed + experimentIndex),
                    Data = CloneInitialData(baseData)
                });
            }

            return jobs;
        }
        private sealed class TestJob
        {
            public int JobNo { get; init; }
            public int ConfigurationNo { get; init; }
            public int ConfigurationIndex { get; init; }
            public int ExperimentIndex { get; init; }
            public int ExperimentNo => ExperimentIndex + 1;
            public int Seed { get; init; }
            public decimal N { get; init; }
            public decimal Pk { get; init; }
            public decimal Pm { get; init; }
            public decimal T { get; init; }
            public decimal Rt { get; init; }
            public decimal Ps { get; init; }
            public decimal Ipk { get; init; }
            public InitialData Data { get; init; } = null!;
            public int GridRowIndex { get; set; } = -1;
        }


        private sealed class TestJobResult
        {
            public TestJob Job { get; init; } = null!;
            public decimal MinFitness { get; init; }
            public decimal AvgFitness { get; init; }
            public decimal BestFitness { get; init; }
            public int BestGeneration { get; init; }
            public TimeSpan Elapsed { get; init; }
            public List<PopulationStatistics> Statistics { get; init; } = new();
            public Individual? BestSolution { get; init; }
            public List<Individual> FinalPopulation { get; init; } = new();
        }

        private List<TestJob> BuildTestJobs(
            InitialData baseData,
            IReadOnlyList<decimal> nValues,
            IReadOnlyList<decimal> pkValues,
            IReadOnlyList<decimal> pmValues,
            IReadOnlyList<decimal> tValues,
            IReadOnlyList<decimal> rtValues,
            IReadOnlyList<decimal> psValues,
            IReadOnlyList<decimal> ipkValues,
            int experimentCount,
            bool useSameSeeds)
        {
            var jobs = new List<TestJob>();
            int configurationIndex = 0;
            int jobNo = 1;

            foreach (var n in nValues)
            foreach (var pk in pkValues)
            foreach (var pm in pmValues)
            foreach (var t in tValues)
            foreach (var rt in rtValues)
            foreach (var ps in psValues)
            foreach (var ipk in ipkValues)
            {
                int configurationNo = configurationIndex + 1;
                for (int experimentIndex = 0; experimentIndex < experimentCount; experimentIndex++)
                {
                    int seed = useSameSeeds
                        ? unchecked(baseData.RandomSeed + experimentIndex)
                        : unchecked(baseData.RandomSeed + configurationIndex * experimentCount + experimentIndex);

                    var jobData = CloneInitialData(baseData);
                    ApplyTestConfiguration(jobData, n, pk, pm, t, rt, ps, ipk);

                    jobs.Add(new TestJob
                    {
                        JobNo = jobNo++,
                        ConfigurationNo = configurationNo,
                        ConfigurationIndex = configurationIndex,
                        ExperimentIndex = experimentIndex,
                        Seed = seed,
                        N = n,
                        Pk = pk,
                        Pm = pm,
                        T = t,
                        Rt = rt,
                        Ps = ps,
                        Ipk = ipk,
                        Data = jobData
                    });
                }

                configurationIndex++;
            }

            return jobs;
        }

        private static InitialData CloneInitialData(InitialData source)
        {
            return new InitialData
            {
                AlgorithmType = source.AlgorithmType,
                MatrixSize = source.MatrixSize,
                RequestedMatrixSize = source.RequestedMatrixSize,
                UseEvenMatrixPadding = source.UseEvenMatrixPadding,
                EmptyRowIndex = source.EmptyRowIndex,
                EmptyColumnIndex = source.EmptyColumnIndex,
                NumberOfIndividuals = source.NumberOfIndividuals,
                CrossProbability = source.CrossProbability,
                MutationProbability = source.MutationProbability,
                BitFlipEarlyExplorationMultiplier = source.BitFlipEarlyExplorationMultiplier,
                BitFlipLateExplorationMultiplier = source.BitFlipLateExplorationMultiplier,
                NarrowingMutationMultiplier = source.NarrowingMutationMultiplier,
                NarrowingMutationStep = source.NarrowingMutationStep,
                NarrowingMutationExponent = source.NarrowingMutationExponent,
                LimitNarrowingMutation = source.LimitNarrowingMutation,
                NumberOfIterations = source.NumberOfIterations,
                SupervisedReferenceMatrix = CloneMatrix(source.SupervisedReferenceMatrix),
                UnsupervisedPatternMatrixes = CloneMatrices(source.UnsupervisedPatternMatrixes),
                NumberOfExperiments = source.NumberOfExperiments,
                AlgorithmOption = source.AlgorithmOption,
                SelectionType = source.SelectionType,
                AdvancedSelectionEnabled = source.AdvancedSelectionEnabled,
                SelectionStages = source.SelectionStages.Select(stage => new SelectionStage
                {
                    SelectionType = stage.SelectionType,
                    Threshold = stage.Threshold
                }).ToList(),
                TournamentSelectionSize = source.TournamentSelectionSize,
                TournamentSoftSelectionTreshold = source.TournamentSoftSelectionTreshold,
                CrossType = source.CrossType,
                CrossCount = source.CrossCount,
                MutationType = source.MutationType,
                UniformBlock = source.UniformBlock,
                ProbGen1 = source.ProbGen1,
                UniformBlockMutationProbWhite = source.UniformBlockMutationProbWhite,
                UniformBlockMutationProbRed = source.UniformBlockMutationProbRed,
                EliteOn = source.EliteOn,
                EliteFraction = source.EliteFraction,
                StopAtFirstCorrect = source.StopAtFirstCorrect,
                UseSeed = source.UseSeed,
                RandomSeed = source.RandomSeed,
                StagnationEnabled = source.StagnationEnabled,
                StagnationWindow = source.StagnationWindow,
                StagnationResetFraction = source.StagnationResetFraction,
                StagnationDiversityThreshold = source.StagnationDiversityThreshold
            };
        }

        private static bool[,] CloneMatrix(bool[,] matrix)
        {
            return matrix == null ? null! : (bool[,])matrix.Clone();
        }

        private static bool[][,] CloneMatrices(bool[][,] matrices)
        {
            if (matrices == null)
                return null!;

            var clones = new bool[matrices.Length][,];
            for (int i = 0; i < matrices.Length; i++)
            {
                clones[i] = CloneMatrix(matrices[i]);
            }

            return clones;
        }


        private FullRunExport BuildFullRunExport(
            string mode,
            int configurationNo,
            int experimentIndex,
            int seed,
            decimal bestFitness,
            int bestGeneration,
            TimeSpan elapsed,
            InitialData data,
            bool[,] bestGenotype,
            IReadOnlyList<PopulationStatistics> statistics)
        {
            return new FullRunExport
            {
                Mode = mode,
                ConfigurationNo = configurationNo,
                ExperimentIndex = experimentIndex,
                Seed = seed,
                BestFitness = bestFitness,
                BestGeneration = bestGeneration,
                Elapsed = elapsed,
                InitialData = CloneInitialData(data),
                BestGenotype = (bool[,])bestGenotype.Clone(),
                Statistics = statistics.ToList()
            };
        }

        private List<string> GetSweptParameterNames()
        {
            var names = new List<string>();

            if (_testSweepNCheckBox.Checked) names.Add("N");
            if (_testSweepTCheckBox.Checked) names.Add("T");
            if (_testSweepPkCheckBox.Checked) names.Add("Pk");
            if (_testSweepPmCheckBox.Checked) names.Add("Pm");
            if (_testSweepRtCheckBox.Checked) names.Add("Rt");
            if (_testSweepPsCheckBox.Checked) names.Add("Ps");
            if (_testSweepIpkCheckBox.Checked) names.Add("IPK");

            if (names.Count == 0)
                names.AddRange(new[] { "N", "T", "Pk", "Pm", "Rt", "Ps", "IPK" });

            return names;
        }
        private static void ApplyTestConfiguration(InitialData data, decimal n, decimal pk, decimal pm, decimal t, decimal rt, decimal ps, decimal ipk)
        {
            data.NumberOfIndividuals = n;
            data.CrossProbability = pk;
            data.MutationProbability = pm;
            data.NumberOfIterations = t;
            data.TournamentSelectionSize = rt;
            data.TournamentSoftSelectionTreshold = ps;
            data.CrossCount = ipk;
        }
        private async void testyStart_Click(object sender, EventArgs e)
        {
            try
            {
                ReadUiData();
                _data.NumberOfExperiments = testExperimentCount.Value;
                new ValidationFacade().ValidateOrThrow(_data);
                ValidateTestSweepRanges();
                if (_archiveResultsCheckBox.Checked)
                    FileUtils.ArchiveResults();

                FileUtils.SaveLastSeed(_data.RandomSeed);

                var nValues = GetTestValues(_testSweepNCheckBox, NaInput, NbInput, NstepInput, individualNumberInput.Value);
                var pkValues = GetTestValues(_testSweepPkCheckBox, pkaInput, PkbbInput, PkstepInput, crossProbabilityInput.Value);
                var pmValues = GetTestValues(_testSweepPmCheckBox, pmaInput, PmbInput, PmstepInput, mutationProbabilityInput.Value);
                var tValues = GetTestValues(_testSweepTCheckBox, TaInput, TbInput, TstepInput, iterationNumberInput.Value);
                var rtValues = GetTestValues(_testSweepRtCheckBox, Rt_a_Input, rt_b_input, rt_step_input, tournamentSizeInput.Value);
                var psValues = GetTestValues(_testSweepPsCheckBox, Ps_a, Ps_b_Input, Ps_step, tournamentTresholdInput.Value);
                var ipkValues = GetTestValues(_testSweepIpkCheckBox, ipk_input, ipk_b_input, ipk_step_input, crossPoints.Value);

                int totalConfigurations = nValues.Count * pkValues.Count * pmValues.Count * tValues.Count *
                    rtValues.Count * psValues.Count * ipkValues.Count;

                if (totalConfigurations <= 0)
                    throw new InvalidOperationException("Brak konfiguracji testowych do uruchomienia.");

                int experimentCount = (int)testExperimentCount.Value;
                bool useSameSeeds = _testUseSameSeedsCheckBox.Checked;
                int maxParallel = (int)_testMaxParallelInput.Value;
                var baseData = CloneInitialData(_data);
                var jobs = BuildTestJobs(baseData, nValues, pkValues, pmValues, tValues, rtValues, psValues, ipkValues, experimentCount, useSameSeeds);
                long totalExperimentRuns = jobs.Count;

                var stopwatch = Stopwatch.StartNew();
                int completedExperiments = 0;
                int activeJobs = 0;
                int testProgressMaximum;

                testyStart.Enabled = false;
                _isComputationRunning = true;
                runProgressBar.Visible = true;
                runProgressBar.Minimum = 0;
                runProgressBar.Maximum = totalExperimentRuns > int.MaxValue ? int.MaxValue : (int)totalExperimentRuns;
                testProgressMaximum = runProgressBar.Maximum;
                runProgressBar.Value = 0;

                ResetTestRunStatus();
                InitializeTestJobsGrid(jobs);
                _testStatusStateLabel.Text = "Stan: przygotowanie";
                _testStatusProgressLabel.Text = $"Postęp eksperymentów: 0 / {totalExperimentRuns}";
                _testStatusSeedModeLabel.Text = useSameSeeds
                    ? "Tryb seedów: te same ziarna dla konfiguracji"
                    : "Tryb seedów: osobne ziarno dla każdego testu";
                _testStatusAverageLabel.Text = $"Maks. równoległe: {maxParallel}";

                using var semaphore = new SemaphoreSlim(maxParallel, maxParallel);
                var tasks = jobs.Select(async job =>
                {
                    await semaphore.WaitAsync();
                    int activeNow = Interlocked.Increment(ref activeJobs);
                    var jobStopwatch = Stopwatch.StartNew();

                    UpdateTestRunStatus(() =>
                    {
                        _testStatusStateLabel.Text = "Stan: eksperymenty w toku";
                        _testStatusConfigurationLabel.Text = $"Konfiguracja: {job.ConfigurationNo} / {totalConfigurations}  N={job.N}, Pk={job.Pk}, Pm={job.Pm}, T={job.T}, Rt={job.Rt}, Ps={job.Ps}, IPK={job.Ipk}";
                        _testStatusExperimentLabel.Text = $"Aktywne: {activeNow} / {maxParallel}";
                        _testStatusSeedLabel.Text = $"Seed: {job.Seed}";
                        _testStatusGenerationLabel.Text = $"Job {job.JobNo}: 0 / {job.T}";
                    });
                    UpdateTestJobGridRow(job, "Uruchomiony", 0, elapsed: TimeSpan.Zero);

                    try
                    {
                        var result = await Task.Run(() =>
                        {
                            long lastProgressUiUpdate = 0;
                            var ga = CreateGeneticAlgorithm(job.Data, job.Seed);
                            var progress = new Progress<int>(generation =>
                            {
                                long now = Stopwatch.GetTimestamp();
                                bool shouldUpdate = generation == 0 ||
                                    generation >= (int)job.T ||
                                    now - Interlocked.Read(ref lastProgressUiUpdate) >= Stopwatch.Frequency / 4;

                                if (!shouldUpdate)
                                    return;

                                Interlocked.Exchange(ref lastProgressUiUpdate, now);
                                UpdateTestJobGridRow(job, "W toku", generation, elapsed: jobStopwatch.Elapsed);
                                UpdateTestRunStatus(() =>
                                {
                                    _testStatusGenerationLabel.Text = $"Job {job.JobNo}: {Math.Min(generation, (int)job.T)} / {job.T}";
                                    _testStatusElapsedLabel.Text = $"Czas: {FormatTestDuration(stopwatch.Elapsed)}";
                                });
                            });

                            ga.Run(progress);

                            var bestStatistic = ga.StatisticsHistory.Count == 0
                                ? null
                                : ga.StatisticsHistory.OrderByDescending(stat => stat.BestFitness).First();

                            return new TestJobResult
                            {
                                Job = job,
                                BestFitness = bestStatistic?.BestFitness ?? 0m,
                                BestGeneration = bestStatistic?.Generation ?? 0,
                                Elapsed = jobStopwatch.Elapsed,
                                Statistics = ga.StatisticsHistory.ToList(),
                                BestSolution = ga.BestSolution?.Clone(),
                                FinalPopulation = ga.CurrentPopulation.Select(individual => individual.Clone()).ToList()
                            };
                        });

                        jobStopwatch.Stop();
                        int completedExperimentCount = Interlocked.Increment(ref completedExperiments);
                        TimeSpan averageExperimentTime = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / Math.Max(1, completedExperimentCount));
                        TimeSpan eta = TimeSpan.FromTicks(averageExperimentTime.Ticks * Math.Max(0, totalExperimentRuns - completedExperimentCount));
                        int progressValue = Math.Min(testProgressMaximum, completedExperimentCount);

                        UpdateTestJobGridRow(job, "Zakończony", (int)job.T, result.BestFitness, result.BestGeneration, result.Elapsed);
                        UpdateTestRunStatus(() =>
                        {
                            runProgressBar.Value = progressValue;
                            testCounter.Text = $"Test {completedExperimentCount} / {totalExperimentRuns}";
                            individualCount.Text = $"Liczba osobników {job.N}";
                            mutationProb.Text = $"Prawdopodobieństwo mutacji {job.Pm}";
                            crossProb.Text = $"Prawdopodobieństwo krzyżowania {job.Pk}";
                            iterationCount.Text = $"Liczba iteracji {job.T}";
                            tournamentSizeLabelTesty.Text = $"Rozmiar turnieju {job.Rt}";
                            selectionTresholLabelTesty.Text = $"Próg selekcji {job.Ps}";
                            ipkLabelTesty.Text = $"Liczba punktów krzyżowań {job.Ipk}";
                            _testStatusBestLabel.Text = $"Ostatni wynik: {result.BestFitness:F4} (job {job.JobNo}, gen. {result.BestGeneration})";
                            _testStatusProgressLabel.Text = $"Postęp eksperymentów: {completedExperimentCount} / {totalExperimentRuns}";
                            _testStatusElapsedLabel.Text = $"Czas: {FormatTestDuration(stopwatch.Elapsed)}";
                            _testStatusAverageLabel.Text = $"Śr. czas eksperymentu: {FormatTestDuration(averageExperimentTime)}";
                            _testStatusEtaLabel.Text = $"Pozostało: {FormatTestDuration(eta)}";
                        });

                        return result;
                    }
                    catch
                    {
                        UpdateTestJobGridRow(job, "Błąd", elapsed: jobStopwatch.Elapsed);
                        throw;
                    }
                    finally
                    {
                        int activeAfter = Interlocked.Decrement(ref activeJobs);
                        UpdateTestRunStatus(() =>
                        {
                            _testStatusExperimentLabel.Text = $"Aktywne: {activeAfter} / {maxParallel}";
                        });
                        semaphore.Release();
                    }
                }).ToArray();

                var jobResults = await Task.WhenAll(tasks);
                stopwatch.Stop();

                var results = jobResults
                    .GroupBy(result => result.Job.ConfigurationNo)
                    .OrderBy(group => group.Key)
                    .Select(group =>
                    {
                        var first = group.First().Job;
                        return new TestObject
                        {
                            Iter = first.ConfigurationNo,
                            N = first.N,
                            pk = first.Pk,
                            pm = first.Pm,
                            T = first.T,
                            Rt = first.Rt,
                            Ps = first.Ps,
                            Ipk = first.Ipk,
                            MinMark = group.Min(result => result.BestFitness),
                            AvgMark = group.Average(result => result.BestFitness),
                            MaxMark = group.Max(result => result.BestFitness)
                        };
                    })
                    .ToList();

                var detailedResults = jobResults
                    .OrderBy(result => result.Job.ConfigurationNo)
                    .ThenBy(result => result.Job.ExperimentIndex)
                    .Select(result => new DetailedTestObject
                    {
                        ConfigurationIter = result.Job.ConfigurationNo,
                        ExperimentIndex = result.Job.ExperimentIndex,
                        Seed = result.Job.Seed,
                        N = result.Job.N,
                        pk = result.Job.Pk,
                        pm = result.Job.Pm,
                        T = result.Job.T,
                        Rt = result.Job.Rt,
                        Ps = result.Job.Ps,
                        Ipk = result.Job.Ipk,
                        BestMark = result.BestFitness,
                        BestGeneration = result.BestGeneration
                    })
                    .ToList();

                UpdateTestRunStatus(() =>
                {
                    _testStatusStateLabel.Text = "Stan: zakończono";
                    _testStatusElapsedLabel.Text = $"Czas: {FormatTestDuration(stopwatch.Elapsed)}";
                    _testStatusEtaLabel.Text = "Pozostało: 00:00";
                    _testStatusConfigStatsLabel.Text = $"Konfiguracje: {results.Count}, joby: {jobResults.Length}";
                });

                var sweptParameters = GetSweptParameterNames();
                var bestRunsByConfiguration = jobResults
                    .GroupBy(result => result.Job.ConfigurationNo)
                    .Select(group => group
                        .OrderByDescending(result => result.BestFitness)
                        .ThenBy(result => result.BestGeneration)
                        .First())
                    .OrderBy(result => result.Job.ConfigurationNo)
                    .ToList();

                foreach (var bestConfigurationRun in bestRunsByConfiguration)
                {
                    var bestIndividual = bestConfigurationRun.BestSolution
                        ?? bestConfigurationRun.FinalPopulation.OrderByDescending(o => o.Fitness).First();

                    FileUtils.SaveTunningBestRun(BuildFullRunExport(
                        "tunning",
                        bestConfigurationRun.Job.ConfigurationNo,
                        bestConfigurationRun.Job.ExperimentIndex,
                        bestConfigurationRun.Job.Seed,
                        bestConfigurationRun.BestFitness,
                        bestConfigurationRun.BestGeneration,
                        bestConfigurationRun.Elapsed,
                        bestConfigurationRun.Job.Data,
                        bestIndividual.Genotype,
                        bestConfigurationRun.Statistics), sweptParameters);
                }

                _testSeedOverride = null;
                ApplyTestConfigurationFromUi();
                FileUtils.SaveGaTunningResults(results, _data);
                FileUtils.SaveDetailedGaTunningResults(detailedResults, _data);

                MessageBox.Show(
                    $"Liczba wyników: {results.Count}\nLiczba jobów: {jobResults.Length}\nPotrzebny czas: {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}",
                    "Sukces"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
            finally
            {
                _testSeedOverride = null;
                testyStart.Enabled = true;
                _isComputationRunning = false;
            }
        }            //    foreach (var pk in pkValues)
            //    {
            //        foreach (var pm in pmValues)
            //        {
            //            foreach (var t in tValues)
            //            {
            //                foreach (var rt in rtValues)
            //                {
            //                    foreach (var ps in psValues)
            //                    {
            //                        foreach (var ipk in ipkValues)
            //                        {
            //                            InitialData.TournamentSelectionSize = rt;
            //                            InitialData.TournamentSoftSelectionTreshold = ps;
            //                            InitialData.CrossCount = ipk;

            //                            testCounter.Text = $"Test {++currentCount} / {maxCount}";
            //                            individualCount.Text = $"Ilo�� osobnik�w {n}";
            //                            mutationProb.Text = $"Prawdopodobie�stwo mutacji {pm}";
            //                            crossProb.Text = $"Prawdopodobie�stwo krzy�owania {pk}";
            //                            iterationCount.Text = $"Ilo�� iteracji {t}";
            //                            tournamentSizeLabelTesty.Text = $"Rozmiar turnieju {rt}";
            //                            selectionTresholLabelTesty.Text = $"Pr�g selekcji {ps}";
            //                            ipkLabelTesty.Text = $"Ilo�� punkt�w krzy�owa� {ipk}";
            //                            Application.DoEvents();

            //                            List<List<Individual>> localHistory = new();

            //                            for (int x = 0; x < testExperimentCount.Value; x++)
            //                            {
            //                                var individuals = Enumerable.Range(1, (int)n)
            //                                    .Select(i => new Individual(
            //                                        i,
            //                                        matrixSizeInput.Value, 
            //                                        0.001m,
            //                                        pk,
            //                                        pm,
            //                                        InitialData.SupervisedReferenceMatrix, 
            //                                        InitialData.AlgorithmType,
            //                                        InitialData.UnsupervisedPatternMatrixes,
            //                                        InitialData.ProbGen1,
            //                                        InitialData.UniformBlockMutationProbWhite,
            //                                        InitialData.UniformBlockMutationProbRed
            //                                    ))
            //                                    .ToList();

            //                                for (int t2 = 0; t2 < t; t2++)
            //                                {
            //                                    if (InitialData.SelectionType == SelectionType.ROULETTE)
            //                                    {
            //                                        SelectionUtils.SetUpFitValue(individuals);
            //                                        SelectionUtils.SetUpDistribuator(individuals);
            //                                        SelectionUtils.SetUpNewOsobnikAfterSelection(individuals);
            //                                    }
            //                                    else if (InitialData.SelectionType == SelectionType.TOURNAMENT_HARD)
            //                                    {
            //                                        SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentHard(individuals, InitialData.TournamentSelectionSize);
            //                                    }
            //                                    else if (InitialData.SelectionType == SelectionType.TOURNAMENT_SOFT)
            //                                    {
            //                                        SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentSoft(individuals, InitialData.TournamentSelectionSize, InitialData.TournamentSoftSelectionTreshold);
            //                                    }

            //                                    for (int i = 0; i < individuals.Count; i++)
            //                                        individuals[i].SetParent();

            //                                    if (InitialData.CrossType == CrossType.SINGLE_POINT)
            //                                    {
            //                                        CrossUtils.SetCutPoint(individuals);
            //                                        CrossUtils.CrossOsobniks(individuals);
            //                                        CrossUtils.CreatePopulationAfterCrossing(individuals);
            //                                    }
            //                                    else
            //                                    {
            //                                        CrossUtils.CreatePopulationAfterCrossingNPoints(individuals, InitialData.CrossCount);
            //                                    }

            //                                    for (int i = 0; i < individuals.Count; i++)
            //                                    {
            //                                        if (InitialData.UniformBlock)
            //                                        {
            //                                            individuals[i].MutateUniformBlock();
            //                                        }
            //                                        switch (InitialData.MutationType)
            //                                        {
            //                                            case MutationType.EQUALY:
            //                                                individuals[i].Mutate();
            //                                                break;
            //                                            case MutationType.BIT_SWAPING:
            //                                                individuals[i].BitSwapMutation();
            //                                                break;
            //                                            case MutationType.RANDOM_COORDS:
            //                                                individuals[i].MutateByNarrowing(i, (int)t);
            //                                                break;
            //                                        }

            //                                        individuals[i].MarkAfterMutation = individuals[i].SetOcena(individuals[i].MatrixAfterMutation);
            //                                    }

            //                                    var copiedIndividuals = individuals.ToList();

            //                                    individuals = new List<Individual>();

            //                                    int eliteCount = InitialData.EliteOn ? (int)InitialData.EliteToMove : 0;

            //                                    eliteCount = Math.Min(eliteCount, copiedIndividuals.Count);

            //                                    var elites = copiedIndividuals
            //                                        .OrderByDescending(ind => ind.MarkAfterMutation)
            //                                        .Take(eliteCount)
            //                                        .ToList();

            //                                    int idx = 1;
            //                                    foreach (var elite in elites)
            //                                    {
            //                                        individuals.Add(new Individual(
            //                                            idx++,
            //                                            matrixSizeInput.Value,
            //                                            0.003m,
            //                                            crossProbabilityInput.Value,
            //                                            mutationProbabilityInput.Value,
            //                                            elite.MatrixAfterMutation,
            //                                            InitialData.SupervisedReferenceMatrix,
            //                                            InitialData.AlgorithmType,
            //                                            InitialData.UnsupervisedPatternMatrixes,
            //                                            InitialData.UniformBlockMutationProbWhite,
            //                                            InitialData.UniformBlockMutationProbRed
            //                                        ));
            //                                    }

            //                                    foreach (var ind in copiedIndividuals.Skip(eliteCount))
            //                                    {
            //                                        individuals.Add(new Individual(
            //                                            idx++,
            //                                            matrixSizeInput.Value,
            //                                            0.003m,
            //                                            crossProbabilityInput.Value,
            //                                            mutationProbabilityInput.Value,
            //                                            ind.MatrixAfterMutation,
            //                                            InitialData.SupervisedReferenceMatrix,
            //                                            InitialData.AlgorithmType,
            //                                            InitialData.UnsupervisedPatternMatrixes,
            //                                            InitialData.UniformBlockMutationProbWhite,
            //                                            InitialData.UniformBlockMutationProbRed
            //                                        ));
            //                                    }
            //                                }

            //                                localHistory.Add(individuals.ToList());
            //                            }

            //                            var avgMark = localHistory.SelectMany(os => os).Average(o => o.Mark);
            //                            var minMark = localHistory.SelectMany(os => os).Min(o => o.Mark);
            //                            var maxMark = localHistory.SelectMany(os => os).Max(o => o.Mark);

            //                            var testObject = new TestObject
            //                            {
            //                                Iter = iter++,
            //                                N = n,
            //                                pk = pk,
            //                                pm = pm,
            //                                T = t,
            //                                AvgMark = avgMark,
            //                                MaxMark = maxMark,
            //                                MinMark = minMark
            //                            };

            //                            var initial = new InitialData()
            //                            {
            //                                MatrixSize = matrixSizeInput.Value,
            //                                NumberOfIndividuals = n,
            //                                NumberOfIterations = t,
            //                                MutationProbability = pm,
            //                                CrossProbability = pk,
            //                                SupervisedReferenceMatrix = InitialData.SupervisedReferenceMatrix,
            //                                UnsupervisedPatternMatrixes = InitialData.UnsupervisedPatternMatrixes
            //                            };

            //                            //globalHistory.Add(initial, localHistory);
            //                            list.Add(testObject);

            //                            localHistory.Clear();
            //                            GC.Collect();
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //watch.Stop();
            //var elapsed = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
            //string elapsedFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);

            //MessageBox.Show("Liczba wynik�w: " + list.Count + "\nPotrzebny czas: " + elapsedFormatted, "Sukces");

            //InitialData.MatrixSize = matrixSizeInput.Value;
            //FileUtils.saveGaTunningResults(list, InitialData);
            ////FileUtils.SaveMResultsGa(globalHistory);

            ////historyOfIndividuals.Clear();
            ////globalHistory.Clear();
            //list.Clear();
            //GC.Collect();


        private void CreateRunJobsPageControls()
        {
            if (_runJobsPage != null)
                return;

            _runJobsPage = new TabPage
            {
                Text = "Uruchomienia",
                Padding = new Padding(12),
                UseVisualStyleBackColor = true
            };

            _runStatusGroupBox = new GroupBox
            {
                Text = "Status uruchomień",
                Location = new System.Drawing.Point(12, 12),
                Size = new System.Drawing.Size(590, 185)
            };

            _runStatusStateLabel = CreateRunStatusLabel(24);
            _runStatusExperimentLabel = CreateRunStatusLabel(49);
            _runStatusGenerationLabel = CreateRunStatusLabel(74);
            _runStatusSeedLabel = CreateRunStatusLabel(99);
            _runStatusBestLabel = CreateRunStatusLabel(124);
            _runStatusProgressLabel = CreateRunStatusLabel(149);
            _runStatusElapsedLabel = CreateRunStatusLabel(24, 330);
            _runStatusAverageLabel = CreateRunStatusLabel(49, 330);
            _runStatusEtaLabel = CreateRunStatusLabel(74, 330);
            _runStatusActiveLabel = CreateRunStatusLabel(99, 330);

            _runStatusGroupBox.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                _runStatusStateLabel,
                _runStatusExperimentLabel,
                _runStatusGenerationLabel,
                _runStatusSeedLabel,
                _runStatusBestLabel,
                _runStatusProgressLabel,
                _runStatusElapsedLabel,
                _runStatusAverageLabel,
                _runStatusEtaLabel,
                _runStatusActiveLabel
            });

            _runJobsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(12, 215),
                Size = new System.Drawing.Size(1060, 380),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            AddRunJobColumn("Job", "Job", 55);
            AddRunJobColumn("Experiment", "Exp.", 55);
            AddRunJobColumn("Seed", "Seed", 100);
            AddRunJobColumn("Status", "Status", 110);
            AddRunJobColumn("Generation", "Gen", 90);
            AddRunJobColumn("Best", "Best", 90);
            AddRunJobColumn("BestGeneration", "Best gen", 80);
            AddRunJobColumn("Elapsed", "Czas", 80);

            _runJobsPage.Controls.Add(_runStatusGroupBox);
            _runJobsPage.Controls.Add(_runJobsGrid);
            tabs.TabPages.Add(_runJobsPage);
            ResetRunStatus();
        }

        private static Label CreateRunStatusLabel(int top, int left = 12)
        {
            int maxWidth = left > 250 ? 235 : 300;
            return new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(left, top),
                MaximumSize = new System.Drawing.Size(maxWidth, 0)
            };
        }

        private void ResetRunStatus()
        {
            if (_runStatusStateLabel == null)
                return;

            _runStatusStateLabel.Text = "Stan: bezczynny";
            _runStatusExperimentLabel.Text = "Eksperyment: -";
            _runStatusGenerationLabel.Text = "Generacja: -";
            _runStatusSeedLabel.Text = "Seed: -";
            _runStatusBestLabel.Text = "Najlepszy wynik: -";
            _runStatusProgressLabel.Text = "Postęp eksperymentów: -";
            _runStatusElapsedLabel.Text = "Czas: -";
            _runStatusAverageLabel.Text = "Śr. czas eksperymentu: -";
            _runStatusEtaLabel.Text = "Pozostało: -";
            _runStatusActiveLabel.Text = "Aktywne: -";
        }

        private void UpdateRunStatus(Action update)
        {
            if (IsDisposed)
                return;

            try
            {
                if (InvokeRequired)
                    BeginInvoke(update);
                else
                    update();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void AddRunJobColumn(string name, string header, int width)
        {
            _runJobsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                Width = width,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }

        private void InitializeRunJobsGrid(IReadOnlyList<RunJob> jobs)
        {
            if (_runJobsGrid == null)
                return;

            _runJobsGrid.Rows.Clear();
            foreach (var job in jobs)
            {
                int rowIndex = _runJobsGrid.Rows.Add(
                    job.JobNo,
                    job.ExperimentNo,
                    job.Seed,
                    "Oczekuje",
                    $"0 / {job.Data.NumberOfIterations}",
                    "-",
                    "-",
                    "-"
                );
                job.GridRowIndex = rowIndex;
            }
        }

        private void UpdateRunJobGridRow(RunJob job, string status, int? generation = null, decimal? best = null, int? bestGeneration = null, TimeSpan? elapsed = null)
        {
            UpdateRunStatus(() =>
            {
                if (_runJobsGrid == null || job.GridRowIndex < 0 || job.GridRowIndex >= _runJobsGrid.Rows.Count)
                    return;

                var row = _runJobsGrid.Rows[job.GridRowIndex];
                row.Cells["Status"].Value = status;
                if (generation.HasValue)
                    row.Cells["Generation"].Value = $"{Math.Min(generation.Value, (int)job.Data.NumberOfIterations)} / {job.Data.NumberOfIterations}";
                if (best.HasValue)
                    row.Cells["Best"].Value = best.Value.ToString("F4", CultureInfo.InvariantCulture);
                if (bestGeneration.HasValue)
                    row.Cells["BestGeneration"].Value = bestGeneration.Value;
                if (elapsed.HasValue)
                    row.Cells["Elapsed"].Value = FormatTestDuration(elapsed.Value);
            });
        }
        private void CreateTestRunStatusControls()
        {
            _testRunStatusGroupBox = new GroupBox
            {
                Text = "Status testów",
                Location = new System.Drawing.Point(1115, 23),
                Size = new System.Drawing.Size(480, 285)
            };

            _testStatusStateLabel = CreateTestStatusLabel(22);
            _testStatusConfigurationLabel = CreateTestStatusLabel(47);
            _testStatusExperimentLabel = CreateTestStatusLabel(72);
            _testStatusGenerationLabel = CreateTestStatusLabel(97);
            _testStatusSeedLabel = CreateTestStatusLabel(122);
            _testStatusBestLabel = CreateTestStatusLabel(147);
            _testStatusConfigStatsLabel = CreateTestStatusLabel(172);
            _testStatusProgressLabel = CreateTestStatusLabel(197);
            _testStatusElapsedLabel = CreateTestStatusLabel(222);
            _testStatusAverageLabel = CreateTestStatusLabel(247);
            _testStatusEtaLabel = CreateTestStatusLabel(22, 315);
            _testStatusSeedModeLabel = CreateTestStatusLabel(47, 315);

            _testRunStatusGroupBox.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                _testStatusStateLabel,
                _testStatusConfigurationLabel,
                _testStatusExperimentLabel,
                _testStatusGenerationLabel,
                _testStatusSeedLabel,
                _testStatusBestLabel,
                _testStatusConfigStatsLabel,
                _testStatusProgressLabel,
                _testStatusElapsedLabel,
                _testStatusAverageLabel,
                _testStatusEtaLabel,
                _testStatusSeedModeLabel
            });

            tabPage3.Controls.Add(_testRunStatusGroupBox);
            ResetTestRunStatus();
        }

        private static Label CreateTestStatusLabel(int top, int left = 12)
        {
            int maxWidth = left > 250 ? 150 : 285;
            return new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(left, top),
                MaximumSize = new System.Drawing.Size(maxWidth, 0)
            };
        }

        private void ResetTestRunStatus()
        {
            _testStatusStateLabel.Text = "Stan: bezczynny";
            _testStatusConfigurationLabel.Text = "Konfiguracja: -";
            _testStatusExperimentLabel.Text = "Eksperyment: -";
            _testStatusGenerationLabel.Text = "Generacja: -";
            _testStatusSeedLabel.Text = "Seed: -";
            _testStatusBestLabel.Text = "Najlepszy wynik: -";
            _testStatusConfigStatsLabel.Text = "Min/Avg/Max konfiguracji: -";
            _testStatusProgressLabel.Text = "Postęp eksperymentów: -";
            _testStatusElapsedLabel.Text = "Czas: -";
            _testStatusAverageLabel.Text = "Śr. czas eksperymentu: -";
            _testStatusEtaLabel.Text = "Pozostało: -";
            _testStatusSeedModeLabel.Text = "Tryb seedów: -";
        }

        private void UpdateTestRunStatus(Action update)
        {
            if (IsDisposed)
                return;

            try
            {
                if (InvokeRequired)
                    BeginInvoke(update);
                else
                    update();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static string FormatTestDuration(TimeSpan duration)
        {
            return duration.TotalHours >= 1
                ? duration.ToString(@"hh\:mm\:ss")
                : duration.ToString(@"mm\:ss");
        }
        private void CreateTestJobsGridControls()
        {
            _testJobsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(23, 535),
                Size = new System.Drawing.Size(1572, 220),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            AddTestJobColumn("Job", "Job", 55);
            AddTestJobColumn("Config", "Konf.", 55);
            AddTestJobColumn("Experiment", "Exp.", 50);
            AddTestJobColumn("Seed", "Seed", 90);
            AddTestJobColumn("Status", "Status", 95);
            AddTestJobColumn("Generation", "Gen", 70);
            AddTestJobColumn("Best", "Best", 80);
            AddTestJobColumn("BestGeneration", "Best gen", 70);
            AddTestJobColumn("Elapsed", "Czas", 70);
            AddTestJobColumn("Parameters", "Parametry", 230);

            tabPage3.Controls.Add(_testJobsGrid);
        }

        private void AddTestJobColumn(string name, string header, int width)
        {
            _testJobsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                Width = width,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }

        private void InitializeTestJobsGrid(IReadOnlyList<TestJob> jobs)
        {
            _testJobsGrid.Rows.Clear();
            foreach (var job in jobs)
            {
                int rowIndex = _testJobsGrid.Rows.Add(
                    job.JobNo,
                    job.ConfigurationNo,
                    job.ExperimentNo,
                    job.Seed,
                    "Oczekuje",
                    $"0 / {job.T}",
                    "-",
                    "-",
                    "-",
                    $"N={job.N}, Pk={job.Pk}, Pm={job.Pm}, Rt={job.Rt}, Ps={job.Ps}, IPK={job.Ipk}"
                );
                job.GridRowIndex = rowIndex;
            }
        }

        private void UpdateTestJobGridRow(TestJob job, string status, int? generation = null, decimal? best = null, int? bestGeneration = null, TimeSpan? elapsed = null)
        {
            UpdateTestRunStatus(() =>
            {
                if (_testJobsGrid == null || job.GridRowIndex < 0 || job.GridRowIndex >= _testJobsGrid.Rows.Count)
                    return;

                var row = _testJobsGrid.Rows[job.GridRowIndex];
                row.Cells["Status"].Value = status;
                if (generation.HasValue)
                    row.Cells["Generation"].Value = $"{Math.Min(generation.Value, (int)job.T)} / {job.T}";
                if (best.HasValue)
                    row.Cells["Best"].Value = best.Value.ToString("F4", CultureInfo.InvariantCulture);
                if (bestGeneration.HasValue)
                    row.Cells["BestGeneration"].Value = bestGeneration.Value;
                if (elapsed.HasValue)
                    row.Cells["Elapsed"].Value = FormatTestDuration(elapsed.Value);
            });
        }
        private void CreateTestSweepControls()
        {
            var group = new GroupBox
            {
                Text = "Parametry testowane",
                Location = new System.Drawing.Point(875, 23),
                Size = new System.Drawing.Size(230, 285)
            };

            _testSweepNCheckBox = CreateTestSweepCheckBox("N", 22, true);
            _testSweepPkCheckBox = CreateTestSweepCheckBox("Pk", 47, true);
            _testSweepPmCheckBox = CreateTestSweepCheckBox("Pm", 72, true);
            _testSweepTCheckBox = CreateTestSweepCheckBox("T", 97, true);
            _testSweepRtCheckBox = CreateTestSweepCheckBox("Rt", 122, false);
            _testSweepPsCheckBox = CreateTestSweepCheckBox("Ps", 147, false);
            _testSweepIpkCheckBox = CreateTestSweepCheckBox("IPK", 172, false);
            _testUseSameSeedsCheckBox = CreateTestSweepCheckBox("Używaj tych samych ziaren", 202, true);
            _testMaxParallelLabel = new Label
            {
                Text = "Maks. równoległe",
                AutoSize = true,
                Location = new System.Drawing.Point(12, 232)
            };
            _testMaxParallelInput = new NumericUpDown
            {
                Location = new System.Drawing.Point(130, 228),
                Size = new System.Drawing.Size(70, 23),
                Minimum = 1,
                Maximum = 1024,
                Value = Math.Max(1, Environment.ProcessorCount)
            };

            group.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                _testSweepNCheckBox,
                _testSweepPkCheckBox,
                _testSweepPmCheckBox,
                _testSweepTCheckBox,
                _testSweepRtCheckBox,
                _testSweepPsCheckBox,
                _testSweepIpkCheckBox,
                _testUseSameSeedsCheckBox,
                _testMaxParallelLabel,
                _testMaxParallelInput
            });

            tabPage3.Controls.Add(group);
        }

        private static CheckBox CreateTestSweepCheckBox(string text, int top, bool isChecked)
        {
            return new CheckBox
            {
                Text = text,
                Checked = isChecked,
                AutoSize = true,
                Location = new System.Drawing.Point(12, top)
            };
        }

        private void ValidateTestSweepRanges()
        {
            if (testExperimentCount.Value <= 0)
                throw new InvalidOperationException("Liczba eksperymentów musi być większa od 0.");

            ValidateRange(_testSweepNCheckBox, NaInput, NbInput, NstepInput, "N");
            ValidateRange(_testSweepPkCheckBox, pkaInput, PkbbInput, PkstepInput, "Pk");
            ValidateRange(_testSweepPmCheckBox, pmaInput, PmbInput, PmstepInput, "Pm");
            ValidateRange(_testSweepTCheckBox, TaInput, TbInput, TstepInput, "T");
            ValidateRange(_testSweepRtCheckBox, Rt_a_Input, rt_b_input, rt_step_input, "Rt");
            ValidateRange(_testSweepPsCheckBox, Ps_a, Ps_b_Input, Ps_step, "Ps");
            ValidateRange(_testSweepIpkCheckBox, ipk_input, ipk_b_input, ipk_step_input, "IPK");

            if (_testSweepIpkCheckBox.Checked && ipk_b_input.Value > _data.MatrixSize - 2)
                throw new InvalidOperationException("Maksymalna liczba punktów krzyżowania to rozmiar macierzy - 2.");
        }

        private static void ValidateRange(
            CheckBox enabled,
            NumericUpDown start,
            NumericUpDown end,
            NumericUpDown step,
            string label
        )
        {
            if (!enabled.Checked)
                return;

            if (start.Value > end.Value)
                throw new InvalidOperationException($"Zakres {label} ma początek większy od końca.");

            if (step.Value <= 0)
                throw new InvalidOperationException($"Krok {label} musi być większy od 0.");
        }

        private static List<decimal> GetTestValues(
            CheckBox enabled,
            NumericUpDown start,
            NumericUpDown end,
            NumericUpDown step,
            decimal currentValue
        )
        {
            if (!enabled.Checked)
                return new List<decimal> { currentValue };

            var values = new List<decimal>();
            for (decimal value = start.Value; value <= end.Value; value += step.Value)
            {
                values.Add(decimal.Round(value, 10));
            }

            return values;
        }

        private void ApplyTestConfiguration(decimal n, decimal pk, decimal pm, decimal t, decimal rt, decimal ps, decimal ipk)
        {
            _data.NumberOfIndividuals = n;
            _data.CrossProbability = pk;
            _data.MutationProbability = pm;
            _data.NumberOfIterations = t;
            _data.TournamentSelectionSize = rt;
            _data.TournamentSoftSelectionTreshold = ps;
            _data.CrossCount = ipk;
        }

        private void ApplyTestConfigurationFromUi()
        {
            ApplyTestConfiguration(
                individualNumberInput.Value,
                crossProbabilityInput.Value,
                mutationProbabilityInput.Value,
                iterationNumberInput.Value,
                tournamentSizeInput.Value,
                tournamentTresholdInput.Value,
                crossPoints.Value
            );
        }

        private bool[,] GetReferenceMatrixForFitness()
        {
            return GetReferenceMatrixForFitness(_data);
        }

        private bool[,] GetReferenceMatrixForFitness(InitialData data)
        {
            if (!data.UseEvenMatrixPadding)
                return data.SupervisedReferenceMatrix;

            return MatrixPaddingMapper.RemoveRowAndColumn(
                data.SupervisedReferenceMatrix,
                data.EmptyRowIndex,
                data.EmptyColumnIndex
            );
        }

        private bool[,] GetMatrixForDisplay(bool[,] genotype)
        {
            if (!_data.UseEvenMatrixPadding)
                return genotype;

            return MatrixPaddingMapper.AddEmptyActiveRowAndColumnToGenotype(
                genotype,
                (int)_data.RequestedMatrixSize,
                _data.EmptyRowIndex,
                _data.EmptyColumnIndex
            );
        }


        private void DisplayMatrix(bool[,] matrix)
        {

            display.Rows.Clear();
            display.Columns.Clear();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);


            display.AllowUserToAddRows = false;


            display.ColumnHeadersVisible = false;
            display.RowHeadersVisible = false;
            display.ColumnCount = cols;


            foreach (DataGridViewColumn column in display.Columns)
            {
                column.Width = 25;
            }


            for (int row = 0; row < rows; row++)
            {
                display.Rows.Add();
                display.Rows[row].Height = 25;
            }


            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var cell = display.Rows[row].Cells[col];
                    if (matrix[row, col])
                    {
                        cell.Style.BackColor = Color.Red;
                    }
                    else
                    {
                        cell.Style.BackColor = Color.White;
                    }
                }
            }

            display.ReadOnly = true;
            display.Enabled = false;
        }

        private void additioanlDataButton_Click(object sender, EventArgs e)
        {
            switch (_data.AlgorithmType)
            {
                case Core.Enums.AlgorithmType.SUPERVISED:
                    ConfigureMatrixSizing();
                    ReferenceMatrixModalWindow referenceMatrixModalWindow = new ReferenceMatrixModalWindow(_data);
                    referenceMatrixModalWindow.FormClosed += (_, _) => UpdateSelectedDataPreview();
                    referenceMatrixModalWindow.Show(this);
                    break;

                case Core.Enums.AlgorithmType.UNSUPERVISED:
                    ConfigureMatrixSizing();
                    PatternChoosingModalWindow patternChoosingModalWindow = new PatternChoosingModalWindow(_data);
                    patternChoosingModalWindow.FormClosed += (_, _) => UpdateSelectedDataPreview();
                    patternChoosingModalWindow.Show(this);
                    break;

                default:
                    Console.WriteLine("Unknown algorithm type");
                    break;
            }

        }

        private void matrixSizeInput_ValueChanged(object sender, EventArgs e)
        {
            UpdateEvenMatrixPaddingControls();
            ConfigureMatrixSizing();
        }

        private void supervisedTypedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.AlgorithmType = AlgorithmType.SUPERVISED;
                additioanlDataButton.Text = "Wybór macierzy referencyjnej";
                UpdateSelectedDataPreview();
            }
        }

        private void unsupervisedTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.AlgorithmType = AlgorithmType.UNSUPERVISED;
                additioanlDataButton.Text = "Wybór macierzy wzorców";
                UpdateSelectedDataPreview();
            }
        }

        private void AlgorithmRun(IProgress<int> progress, decimal precision)
        {
            //int stagnationTime = 0;
            //Individual currentBest = null;
            //int stagnationLimit = 50;
            //bool solutionFound = false;
            //List<Individual> individuals = new List<Individual>();
            //for (int i = 1; i <= individualNumberInput.Value; i++)
            //{
            //    individuals.Add(new Individual(
            //        i,
            //        matrixSizeInput.Value, 
            //        precision, 
            //        crossProbabilityInput.Value,
            //        mutationProbabilityInput.Value, 
            //        InitialData.SupervisedReferenceMatrix, 
            //        InitialData.AlgorithmType, 
            //        InitialData.UnsupervisedPatternMatrixes, 
            //        InitialData.ProbGen1,
            //        InitialData.UniformBlockMutationProbWhite,
            //        InitialData.UniformBlockMutationProbRed
            //    ));
            //}
            //for (int t = 0; t < iterationNumberInput.Value; t++)
            //{

            //    if (InitialData.SelectionType.Equals(SelectionType.ROULETTE))
            //    {
            //        SelectionUtils.SetUpFitValue(individuals);
            //        SelectionUtils.SetUpDistribuator(individuals);
            //        SelectionUtils.SetUpNewOsobnikAfterSelection(individuals);
            //    }
            //    else if (InitialData.SelectionType.Equals(SelectionType.TOURNAMENT_HARD))
            //    {
            //        SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentHard(individuals, InitialData.TournamentSelectionSize);
            //    }
            //    else if (InitialData.SelectionType.Equals(SelectionType.TOURNAMENT_SOFT))
            //    {
            //        SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentSoft(individuals, InitialData.TournamentSelectionSize, InitialData.TournamentSoftSelectionTreshold);
            //    }

            //    for (int i = 0; i < individuals.Count; i++)
            //    {
            //        individuals[i].SetParent();
            //    }
            //    if (InitialData.CrossType.Equals(CrossType.SINGLE_POINT))
            //    {
            //        CrossUtils.SetCutPoint(individuals);
            //        CrossUtils.CrossOsobniks(individuals);
            //        CrossUtils.CreatePopulationAfterCrossing(individuals);
            //    }
            //    else
            //    {
            //        CrossUtils.CreatePopulationAfterCrossingNPoints(individuals, InitialData.CrossCount);
            //    }

            //    for (int i = 0; i < individuals.Count; i++)
            //    {
            //        if (InitialData.UniformBlock)
            //        {
            //            individuals[i].MutateUniformBlock();
            //        }

            //        if (InitialData.MutationType.Equals(MutationType.EQUALY))
            //        {
            //            individuals[i].Mutate();
            //        }
            //        else if (InitialData.MutationType.Equals(MutationType.EQUALY))
            //        {
            //            individuals[i].BitSwapMutation();
            //        }
            //        else if (InitialData.MutationType.Equals(MutationType.RANDOM_COORDS))
            //        {
            //            individuals[i].MutateByNarrowing(i, (int)InitialData.NumberOfIterations);
            //        }
            //        individuals[i].MarkAfterMutation = individuals[i].SetOcena(individuals[i].MatrixAfterMutation);
            //    }


            //    //Individual bestThisIteration = null;
            //    //decimal bestMark = decimal.MinValue;

            //    //for (int i = 0; i < individuals.Count; i++)
            //    //{
            //    //    if (individuals[i].MarkAfterMutation > bestMark)
            //    //    {
            //    //        bestMark = individuals[i].MarkAfterMutation;
            //    //        bestThisIteration = individuals[i];
            //    //    }
            //    //}

            //    //if (currentBest == null)
            //    //{
            //    //    currentBest = bestThisIteration;
            //    //    stagnationTime = 0;
            //    //}
            //    //else if (bestThisIteration.MarkAfterMutation > currentBest.MarkAfterMutation)
            //    //{
            //    //    currentBest = bestThisIteration;
            //    //    stagnationTime = 0;
            //    //}
            //    //else
            //    //{
            //    //    stagnationTime++;
            //    //}

            //    //if (stagnationTime >= stagnationLimit)
            //    //{
            //    //    RemoveStagnation(
            //    //        individuals,
            //    //        precision,
            //    //        500,
            //    //        1m
            //    //    );

            //    //    stagnationTime = 0;
            //    //}


            //    const decimal Threshold = 0.975m;

            //    bool found = individuals.Any(i => i.MarkAfterMutation >= Threshold);
            //    if (!solutionFound && found )
            //    {
            //        solutionFound = true;
            //        cumlatiiveArrayBase[t] += 1;
            //    }
            //    historyOfIndividuals.Add(individuals);

            //    List<Individual> coppiedIndividuals = individuals.ToList();
            //    individuals = new List<Individual>();
            //    int eliteCount = InitialData.EliteOn ? (int)InitialData.EliteToMove : 0;
            //    eliteCount = Math.Min(eliteCount, coppiedIndividuals.Count);
            //    var elites = coppiedIndividuals
            //        .OrderByDescending(ind => ind.MarkAfterMutation)
            //        .Take(eliteCount)
            //        .ToList();
            //    int idx = 1;
            //    foreach (var elite in elites)
            //    {
            //        individuals.Add(new Individual(
            //            idx++,
            //            matrixSizeInput.Value,
            //            precision,
            //            crossProbabilityInput.Value,
            //            mutationProbabilityInput.Value,
            //            elite.MatrixAfterMutation,
            //            InitialData.SupervisedReferenceMatrix,
            //            InitialData.AlgorithmType,
            //            InitialData.UnsupervisedPatternMatrixes,
            //            InitialData.UniformBlockMutationProbWhite,
            //            InitialData.UniformBlockMutationProbRed
            //        ));
            //    }
            //    foreach (var ind in coppiedIndividuals.Skip(eliteCount))
            //    {
            //        individuals.Add(new Individual(
            //            idx++,
            //            matrixSizeInput.Value,
            //            precision,
            //            crossProbabilityInput.Value,
            //            mutationProbabilityInput.Value,
            //            ind.MatrixAfterMutation,
            //            InitialData.SupervisedReferenceMatrix,
            //            InitialData.AlgorithmType,
            //            InitialData.UnsupervisedPatternMatrixes,
            //            InitialData.UniformBlockMutationProbWhite,
            //            InitialData.UniformBlockMutationProbRed
            //        ));
            //    }

                
            //    progress?.Report(t + 1);


            //}


        }

        private void RemoveStagnation(
            List<Individual> individuals,
            decimal precision,
            decimal amountToRemoveStagnation,
            decimal probabilityOfRemovalStagnation
        )
        {
            //if (individuals == null || individuals.Count == 0)
            //    return;

            //int amount = (int)(individuals.Count * amountToRemoveStagnation);
            //if (amount <= 0)
            //    return;

            //var random = RandomSingleton.Instance;

            //for (int k = 0; k < amount; k++)
            //{
            //    if (random.NextDouble() > (double)probabilityOfRemovalStagnation)
            //        continue;

            //    int index = random.Next(individuals.Count);

            //    individuals[index] = new Individual(
            //        index,
            //        matrixSizeInput.Value,
            //        precision,
            //        crossProbabilityInput.Value,
            //        mutationProbabilityInput.Value,
            //        InitialData.SupervisedReferenceMatrix,
            //        InitialData.AlgorithmType,
            //        InitialData.UnsupervisedPatternMatrixes,
            //        InitialData.ProbGen1,
            //        InitialData.UniformBlockMutationProbWhite,
            //        InitialData.UniformBlockMutationProbRed
            //    );
            //}
        }


        //private List<decimal> GenerateDecimalRange(decimal start, decimal end, decimal step)
        //{
        //    //if (step <= 0)
        //    //    throw new ArgumentException("Step must be positive and non-zero.");

        //    //var result = new List<decimal>();
        //    //for (decimal value = start; value <= end; value += step)
        //    //{
        //    //    result.Add(decimal.Round(value, 10));
        //    //}
        //    //return result;
        //}

        private void classicalGA_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.AlgorithmOption = AlgorithmOption.CLASSICAL;
                _data.SelectionType = SelectionType.ROULETTE;
                _data.CrossType = CrossType.SINGLE_POINT;
                _data.MutationType = MutationType.EQUALY;
                SetUniformMutationMultipliers(1m, 1m);
                SetupDefaultAlgorithmOtpions();
            }
        }

        private void modifiedGARadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.AlgorithmOption = AlgorithmOption.MODIFIED;
                _data.SelectionType = SelectionType.ROULETTE;
                _data.CrossType = CrossType.SINGLE_POINT;
                _data.MutationType = MutationType.EQUALY;

                selectionGroup.Enabled = true;
                _advancedSelectionGroupBox.Enabled = true;
                UpdateAdvancedSelectionControls();
                crossGroup.Enabled = true;
                mutationGroup.Enabled = true;
                _bitFlipMutationGroupBox.Enabled = _data.MutationType == MutationType.EQUALY;
                unformBlocksGroupBox.Enabled = true;
                eliteGroupBox.Enabled = true;
            }
        }

        private void SetupDefaultAlgorithmOtpions()
        {

            classicalGARadio.Checked = true;
            ruletteRadio.Checked = true;
            singlePointRadio.Checked = true;
            evenlyRadio.Checked = true;
            uniformBlock.Checked = false;
            eliteOn.Checked = false;

            selectionGroup.Enabled = false;
            _advancedSelectionCheckBox.Checked = false;
            _advancedSelectionGroupBox.Enabled = false;
            UpdateAdvancedSelectionControls();
            crossGroup.Enabled = false;
            mutationGroup.Enabled = false;
            _bitFlipMutationGroupBox.Enabled = false;
            _narrowingMutationGroupBox.Enabled = false;
            unformBlocksGroupBox.Enabled = false;
            eliteGroupBox.Enabled = false;
        }

        private void SetUniformMutationMultipliers(decimal earlyMultiplier, decimal lateMultiplier)
        {
            _bitFlipEarlyMultiplierInput.Value = earlyMultiplier;
            _bitFlipLateMultiplierInput.Value = lateMultiplier;
        }

        private void ruletteRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.SelectionType = SelectionType.ROULETTE;
                tournamentSizeInput.Enabled = false;
                tournamentTresholdInput.Enabled = false;

                tournamentSizeGroupBox.Enabled = false;
                selectionThresholdGroupBox.Enabled = false;
            }
        }

        private void tournamentHardRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.SelectionType = SelectionType.TOURNAMENT_HARD;
                tournamentSizeInput.Enabled = true;
                tournamentTresholdInput.Enabled = false;


                tournamentSizeGroupBox.Enabled = true;
                selectionThresholdGroupBox.Enabled = false;
            }
        }

        private void tournamentSoftRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.SelectionType = SelectionType.TOURNAMENT_SOFT;
                tournamentSizeInput.Enabled = true;
                tournamentTresholdInput.Enabled = true;

                tournamentSizeGroupBox.Enabled = true;
                selectionThresholdGroupBox.Enabled = true;
            }
        }

        private void rankingRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.SelectionType = SelectionType.RANKING;
                tournamentSizeInput.Enabled = false;
                tournamentTresholdInput.Enabled = false;

                tournamentSizeGroupBox.Enabled = false;
                selectionThresholdGroupBox.Enabled = false;
            }
        }

        private void singlePointRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.CrossType = CrossType.SINGLE_POINT;
                crossPoints.Enabled = false;
                crossCountGroupbox.Enabled = false;
            }
        }

        private void multiPointRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.CrossType = CrossType.MULTI_POINT;
                crossPoints.Enabled = true;
                crossCountGroupbox.Enabled = true;
            }
        }

        private void evenlyRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.MutationType = MutationType.EQUALY;
                _bitFlipMutationGroupBox.Enabled = modifiedGARadio.Checked;
                _narrowingMutationGroupBox.Enabled = false;
            }
        }

        private void bitSwapingRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.MutationType = MutationType.BIT_SWAPING;
                _bitFlipMutationGroupBox.Enabled = false;
                _narrowingMutationGroupBox.Enabled = false;
            }
        }

        private void randomMutationInput_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.MutationType = MutationType.RANDOM_COORDS;
                _bitFlipMutationGroupBox.Enabled = false;
                _narrowingMutationGroupBox.Enabled = true;
            }
        }

        private void useSeed_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null && checkbox.Checked)
            {
                seed.Enabled = checkbox.Checked;
            }
            else
            {
                seed.Enabled = checkbox.Checked;
            }
        }

        private void historyViewButton_Click(object sender, EventArgs e)
        {
            if (_history.Count != 0)
            {
                HistoryViewModalWindow historyViewModalWindow = new HistoryViewModalWindow(_history);
                historyViewModalWindow.Show();
            }
            else
            {
                MessageBox.Show("Brak elementów do podglądu. Uruchom algorytm przed otwarciem historii.");
            }


        }

        private void uniformBlock_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null && checkbox.Checked)
            {
                _data.UniformBlock = checkbox.Checked;
            }
        }
    }
}




































