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
        private CheckBox _archiveResultsCheckBox = null!;

        public MainWindow()
        {
            InitializeComponent();
            _data = new InitialData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureMatrixSizing();

            precisionInput.Items.Add(0.1m);
            precisionInput.Items.Add(0.01m);
            precisionInput.Items.Add(0.001m);
            precisionInput.Items.Add(0.0001m);

            precisionInput.SelectedItem = 0.001m;
            supervisedTypedRadioButton.Checked = true;

            LoadLastSeed();
            seed.Enabled = useSeed.Checked;
            CreateStagnationControls();
            CreateBitFlipMutationControls();
            CreateNarrowingMutationControls();
            CreateEvenMatrixPaddingControls();
            SetupDefaultAlgorithmOtpions();
            ApplyVisualLayout();
            CreateTestSweepControls();
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
                await RunGeneticAlgorithm();
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "B��d");
            }
}

        private async Task RunGeneticAlgorithm()
        {
            ClearPreviousRunState();

            int experimentCount = (int)_data.NumberOfExperiments;
            int iterationCount = (int)_data.NumberOfIterations;

            runProgressBar.Visible = true;
            runProgressBar.Minimum = 0;
            runProgressBar.Maximum = experimentCount * iterationCount;
            runProgressBar.Value = 0;

            var stopwatch = Stopwatch.StartNew();
            var cumulativeSuccesses = new int[iterationCount + 1];
            var perfectSeedResults = new List<PerfectSeedResult>();
            GeneticAlgorithm lastGa = null;

            for (int experimentIndex = 0; experimentIndex < experimentCount; experimentIndex++)
            {
                var ga = CreateGeneticAlgorithm(experimentIndex);
                ga.StoreHistory = experimentIndex == experimentCount - 1;
                int completedBeforeThisExperiment = experimentIndex * iterationCount;
                var progress = new Progress<int>(value =>
                {
                    int totalProgress = completedBeforeThisExperiment + Math.Min(value, iterationCount);
                    runProgressBar.Value = Math.Min(totalProgress, runProgressBar.Maximum);
                });

                await Task.Run(() => ga.Run(progress));

                AddGenerationSuccess(cumulativeSuccesses, ga.StatisticsHistory);
                if (ga.PerfectSolution != null)
                {
                    perfectSeedResults.Add(new PerfectSeedResult
                    {
                        ExperimentIndex = experimentIndex,
                        Seed = GetExperimentSeed(experimentIndex),
                        Generation = ga.PerfectSolutionGeneration ?? 0,
                        Fitness = ga.PerfectSolution.Fitness,
                        Genotype = (bool[,])ga.PerfectSolution.Genotype.Clone()
                    });
                }

                lastGa = ga;
            }

            stopwatch.Stop();

            runProgressBar.Visible = false;

            if (lastGa == null)
                return;

            _history = lastGa.History
                .Select(generation => generation.Select(individual => individual.Clone()).ToList())
                .ToList();

            FileUtils.SaveCumulativeResults(cumulativeSuccesses, iterationCount);
            FileUtils.SavePerfectSeedResults(perfectSeedResults, _data);

            var lastGeneration = lastGa.CurrentPopulation;

            DisplayLastGeneration(lastGeneration);

            DisplayMatrix(GetMatrixForDisplay(lastGeneration.OrderByDescending(o => o.Fitness).First().Genotype));
            //FileUtils.SaveResultsGa(historyOfIndividuals, InitialData);
            DrawFitnessChart(lastGa.StatisticsHistory);
            DrawCumulativeChart(cumulativeSuccesses, experimentCount);
            DrawMutationChart(iterationCount);

            var elapsed = stopwatch.Elapsed;
            MessageBox.Show(
                $"Genetic algorithm finished {experimentCount} experiment(s) in {elapsed.TotalMilliseconds:N0} ms\n" +
                $"({elapsed.TotalSeconds:F2} seconds)",
                "Execution time",
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
            var random = CreateRandomProvider(experimentIndex);

            var fitness = CreateFitness();
            var selection = CreateSelection(random);
            var crossover = CreateCrossover(random);
            var mutation = CreateMutation(random);
            var population = CreateInitialPopulation(random);

            var builder = GeneticAlgorithmBuilder
                .Create()
                .WithInitialPopulation(population)
                .WithFitness(fitness)
                .WithSelection(selection)
                .WithCrossover(crossover)
                .WithMutation(mutation)
                .WithTermination(
                    new MaxIterationCondition((int)_data.NumberOfIterations)
                );

            if (_data.StagnationEnabled)
                builder.WithStagnationReset(
                    _data.StagnationWindow,
                    _data.StagnationResetFraction,
                    random,
                    _data.StagnationDiversityThreshold
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
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 290));
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

            var runGroup = CreateRunGroup();
            basicTab.Controls.Add(runGroup);
            runGroup.Location = new System.Drawing.Point(970, 72);

            MoveControl(selectionGroup, operatorsTab, 12, 12);
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

        private GroupBox CreateRunGroup()
        {
            var runGroup = new GroupBox
            {
                Text = "Uruchamianie",
                Size = new System.Drawing.Size(235, 180)
            };

            _archiveResultsCheckBox = new CheckBox
            {
                Text = "Archiwizuj wyniki",
                AutoSize = true,
                Checked = true
            };

            MoveControl(startButton, runGroup, 16, 28);
            MoveControl(runProgressBar, runGroup, 16, 66);
            MoveControl(historyViewButton, runGroup, 16, 100);
            MoveControl(_archiveResultsCheckBox, runGroup, 16, 136);

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
            _data.ProbGen1 = propGen1.Value;
            _data.TournamentSelectionSize = tournamentSizeInput.Value;
            _data.TournamentSoftSelectionTreshold = tournamentTresholdInput.Value;
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
            return new SeededRandomProvider(GetExperimentSeed(experimentIndex));
        }

        private int GetExperimentSeed(int experimentIndex)
        {
            return unchecked(_data.RandomSeed + experimentIndex);
        }

        private IFitnessEvaluator CreateFitness()
        {
            return _data.AlgorithmType == AlgorithmType.SUPERVISED
                ? new SupervisedFitnessEvaluator(
                    GetReferenceMatrixForFitness(),
                    precisionDigits: 4
                )
                : new UnsupervisedPatternFitnessEvaluator(
                    _data.UnsupervisedPatternMatrixes,
                    precisionDigits: 4
                );
        }

        private ISelectionStrategy CreateSelection(IRandomProvider random)
        {
            return _data.SelectionType switch
            {
                SelectionType.ROULETTE =>
                    new RouletteSelection(random),

                SelectionType.TOURNAMENT_HARD =>
                    new TournamentSelection(
                        (int)_data.TournamentSelectionSize,
                        random
                    ),

                SelectionType.TOURNAMENT_SOFT =>
                    new SoftTournamentSelection(
                        (int)_data.TournamentSelectionSize,
                        _data.TournamentSoftSelectionTreshold,
                        random
                    ),

                _ => throw new InvalidOperationException("Nieznany typ selekcji")
            };
        }


        private ICrossoverOperator CreateCrossover(IRandomProvider random)
        {
            return _data.CrossType switch
            {
                CrossType.SINGLE_POINT =>
                    new OnePointCrossover(random),

                CrossType.MULTI_POINT =>
                    new MultiPointCrossover(
                        (int)_data.CrossCount,
                        random
                    ),

                _ => throw new InvalidOperationException("Nieznany typ krzy�owania")
            };
        }

        private IMutationOperator CreateMutation(IRandomProvider random)
        {
            var mutations = new List<IMutationOperator>();

            mutations.Add(_data.MutationType switch
            {
                MutationType.EQUALY =>
                    new BitFlipMutation(
                        _data.MutationProbability,
                        _data.BitFlipEarlyExplorationMultiplier,
                        _data.BitFlipLateExplorationMultiplier,
                        random
                    ),

                MutationType.BIT_SWAPING =>
                    new BitSwapMutation(_data.MutationProbability, random),

                MutationType.RANDOM_COORDS =>
                    new NarrowingMutation(
                        _data.MutationProbability,
                        _data.NarrowingMutationMultiplier,
                        _data.NarrowingMutationStep,
                        _data.NarrowingMutationExponent,
                        _data.LimitNarrowingMutation,
                        random
                    ),

                _ => throw new InvalidOperationException()
            });

            if (_data.UniformBlock)
            {
                mutations.Add(
                    new UniformBlockMutation(
                        _data.UniformBlockMutationProbWhite,
                        _data.UniformBlockMutationProbRed,
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
            return Enumerable.Range(0, (int)_data.NumberOfIndividuals)
                .Select(_ =>
                    new Individual(
                        InitialGenotypeFactory.Create(
                            (int)_data.MatrixSize,
                            _data.ProbGen1,
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

                var stopwatch = Stopwatch.StartNew();
                var results = new List<TestObject>(totalConfigurations);
                int experimentCount = (int)testExperimentCount.Value;
                int currentConfiguration = 0;
                decimal resultIndex = 1;

                testyStart.Enabled = false;

                await Task.Run(() =>
                {
                    foreach (var n in nValues)
                    foreach (var pk in pkValues)
                    foreach (var pm in pmValues)
                    foreach (var t in tValues)
                    foreach (var rt in rtValues)
                    foreach (var ps in psValues)
                    foreach (var ipk in ipkValues)
                    {
                        decimal min = decimal.MaxValue;
                        decimal max = decimal.MinValue;
                        decimal sum = 0m;

                        for (int experimentIndex = 0; experimentIndex < experimentCount; experimentIndex++)
                        {
                            ApplyTestConfiguration(n, pk, pm, t, rt, ps, ipk);
                            var ga = CreateGeneticAlgorithm(experimentIndex);
                            ga.Run();

                            decimal best = ga.StatisticsHistory.Count == 0
                                ? 0m
                                : ga.StatisticsHistory.Max(stat => stat.BestFitness);

                            min = Math.Min(min, best);
                            max = Math.Max(max, best);
                            sum += best;
                        }

                        results.Add(new TestObject
                        {
                            Iter = resultIndex++,
                            N = n,
                            pk = pk,
                            pm = pm,
                            T = t,
                            Rt = rt,
                            Ps = ps,
                            Ipk = ipk,
                            MinMark = min,
                            AvgMark = sum / experimentCount,
                            MaxMark = max
                        });

                        int completed = Interlocked.Increment(ref currentConfiguration);
                        BeginInvoke(new Action(() =>
                        {
                            testCounter.Text = $"Test {completed} / {totalConfigurations}";
                            individualCount.Text = $"Ilość osobników {n}";
                            mutationProb.Text = $"Prawdopodobieństwo mutacji {pm}";
                            crossProb.Text = $"Prawdopodobieństwo krzyżowania {pk}";
                            iterationCount.Text = $"Ilość iteracji {t}";
                            tournamentSizeLabelTesty.Text = $"Rozmiar turnieju {rt}";
                            selectionTresholLabelTesty.Text = $"Próg selekcji {ps}";
                            ipkLabelTesty.Text = $"Ilość punktów krzyżowań {ipk}";
                        }));
                    }
                });

                stopwatch.Stop();

                ApplyTestConfigurationFromUi();
                FileUtils.SaveGaTunningResults(results, _data);

                MessageBox.Show(
                    $"Liczba wyników: {results.Count}\nPotrzebny czas: {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}",
                    "Sukces"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }
            finally
            {
                testyStart.Enabled = true;
            }
        }
            //    foreach (var pk in pkValues)
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
            //InitialData.Precision = (decimal)precisionInput.SelectedItem;
            //FileUtils.saveGaTunningResults(list, InitialData);
            ////FileUtils.SaveMResultsGa(globalHistory);

            ////historyOfIndividuals.Clear();
            ////globalHistory.Clear();
            //list.Clear();
            //GC.Collect();


        private void CreateTestSweepControls()
        {
            var group = new GroupBox
            {
                Text = "Parametry testowane",
                Location = new System.Drawing.Point(875, 23),
                Size = new System.Drawing.Size(170, 210)
            };

            _testSweepNCheckBox = CreateTestSweepCheckBox("N", 22, true);
            _testSweepPkCheckBox = CreateTestSweepCheckBox("Pk", 47, true);
            _testSweepPmCheckBox = CreateTestSweepCheckBox("Pm", 72, true);
            _testSweepTCheckBox = CreateTestSweepCheckBox("T", 97, true);
            _testSweepRtCheckBox = CreateTestSweepCheckBox("Rt", 122, false);
            _testSweepPsCheckBox = CreateTestSweepCheckBox("Ps", 147, false);
            _testSweepIpkCheckBox = CreateTestSweepCheckBox("IPK", 172, false);

            group.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                _testSweepNCheckBox,
                _testSweepPkCheckBox,
                _testSweepPmCheckBox,
                _testSweepTCheckBox,
                _testSweepRtCheckBox,
                _testSweepPsCheckBox,
                _testSweepIpkCheckBox
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
                throw new InvalidOperationException("Maksymalna ilość punktów krzyżowania to rozmiar macierzy - 2.");
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
            if (!_data.UseEvenMatrixPadding)
                return _data.SupervisedReferenceMatrix;

            return MatrixPaddingMapper.RemoveRowAndColumn(
                _data.SupervisedReferenceMatrix,
                _data.EmptyRowIndex,
                _data.EmptyColumnIndex
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
                    referenceMatrixModalWindow.Show();
                    break;

                case Core.Enums.AlgorithmType.UNSUPERVISED:
                    ConfigureMatrixSizing();
                    PatternChoosingModalWindow patternChoosingModalWindow = new PatternChoosingModalWindow(_data);
                    patternChoosingModalWindow.Show();
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
            }
        }

        private void unsupervisedTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.AlgorithmType = AlgorithmType.UNSUPERVISED;
                additioanlDataButton.Text = "Wybór macierzy wzorców";
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
