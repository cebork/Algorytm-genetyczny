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
        private NumericUpDown _narrowingMultiplierInput = null!;
        private NumericUpDown _narrowingStepInput = null!;

        public MainWindow()
        {
            InitializeComponent();
            _data = new InitialData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _data.MatrixSize = matrixSizeInput.Value;

            precisionInput.Items.Add(0.1m);
            precisionInput.Items.Add(0.01m);
            precisionInput.Items.Add(0.001m);
            precisionInput.Items.Add(0.0001m);

            precisionInput.SelectedItem = 0.001m;
            supervisedTypedRadioButton.Checked = true;

            seed.Enabled = false;
            CreateStagnationControls();
            CreateNarrowingMutationControls();
            SetupDefaultAlgorithmOtpions();
        }


        private async void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                ReadUiData();
                new ValidationFacade().ValidateOrThrow(_data);
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
            GeneticAlgorithm lastGa = null;

            for (int experimentIndex = 0; experimentIndex < experimentCount; experimentIndex++)
            {
                var ga = CreateGeneticAlgorithm(experimentIndex);
                int completedBeforeThisExperiment = experimentIndex * iterationCount;
                var progress = new Progress<int>(value =>
                {
                    int totalProgress = completedBeforeThisExperiment + Math.Min(value, iterationCount);
                    runProgressBar.Value = Math.Min(totalProgress, runProgressBar.Maximum);
                });

                await Task.Run(() => ga.Run(progress));

                AddGenerationSuccess(cumulativeSuccesses, ga.StatisticsHistory);
                lastGa = ga;
            }

            stopwatch.Stop();

            runProgressBar.Visible = false;

            if (lastGa == null)
                return;

            FileUtils.SaveCumulativeResults(cumulativeSuccesses, iterationCount);

            var lastGeneration = lastGa.CurrentPopulation;

            DisplayLastGeneration(lastGeneration);

            DisplayMatrix(lastGeneration.OrderByDescending(o => o.Fitness).First().Genotype);
            //FileUtils.SaveResultsGa(historyOfIndividuals, InitialData);
            DrawFitnessChart(lastGa.StatisticsHistory);
            DrawCumulativeChart(cumulativeSuccesses, experimentCount);

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
            Controls.Add(group);
        }

        private void CreateNarrowingMutationControls()
        {
            _narrowingMutationGroupBox = new GroupBox
            {
                Text = "Mutacja zwężająca",
                Location = new System.Drawing.Point(1644, 440),
                Size = new System.Drawing.Size(200, 125),
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

            _narrowingMutationGroupBox.Controls.Add(multiplierLabel);
            _narrowingMutationGroupBox.Controls.Add(_narrowingMultiplierInput);
            _narrowingMutationGroupBox.Controls.Add(stepLabel);
            _narrowingMutationGroupBox.Controls.Add(_narrowingStepInput);
            Controls.Add(_narrowingMutationGroupBox);
        }

        private void ReadUiData()
        {
            _data.MatrixSize = matrixSizeInput.Value;
            _data.NumberOfIndividuals = individualNumberInput.Value;
            _data.NumberOfIterations = iterationNumberInput.Value;
            _data.NumberOfExperiments = experimentNumber.Value;
            _data.MutationProbability = mutationProbabilityInput.Value;
            _data.CrossProbability = crossProbabilityInput.Value;
            _data.CrossCount = crossPoints.Value;
            _data.ProbGen1 = propGen1.Value;
            _data.TournamentSelectionSize = tournamentSizeInput.Value;
            _data.UniformBlockMutationProbWhite = uniformProbWhite.Value;
            _data.UniformBlockMutationProbRed = uniformProbRed.Value;
            _data.NarrowingMutationMultiplier = _narrowingMultiplierInput.Value;
            _data.NarrowingMutationStep = (int)_narrowingStepInput.Value;
            _data.StagnationEnabled = _stagnationOnCheckBox.Checked;
            _data.StagnationWindow = (int)_stagnationWindowInput.Value;
            _data.StagnationResetFraction = _stagnationFractionInput.Value;
            _data.StagnationDiversityThreshold = _stagnationDiversityThresholdInput.Value;
        }

        private IRandomProvider CreateRandomProvider()
        {
            return useSeed.Checked
                ? new SeededRandomProvider(int.Parse(seed.Text))
                : new SeededRandomProvider(Environment.TickCount);
        }

        private IRandomProvider CreateRandomProvider(int experimentIndex)
        {
            int seedValue = useSeed.Checked
                ? int.Parse(seed.Text) + experimentIndex
                : Environment.TickCount + experimentIndex;

            return new SeededRandomProvider(seedValue);
        }

        private IFitnessEvaluator CreateFitness()
        {
            return _data.AlgorithmType == AlgorithmType.SUPERVISED
                ? new SupervisedFitnessEvaluator(
                    _data.SupervisedReferenceMatrix,
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
                    new BitFlipMutation(_data.MutationProbability, random),

                MutationType.BIT_SWAPING =>
                    new BitSwapMutation(_data.MutationProbability, random),

                MutationType.RANDOM_COORDS =>
                    new NarrowingMutation(
                        _data.MutationProbability,
                        _data.NarrowingMutationMultiplier,
                        _data.NarrowingMutationStep,
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

            for (int generation = 0; generation < cumulativeSuccesses.Length; generation++)
            {
                decimal percentage = experimentCount == 0
                    ? 0
                    : cumulativeSuccesses[generation] * 100m / experimentCount;

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




        private void testyStart_Click(object sender, EventArgs e)
        {
            //if (!useSeed.Checked)
            //    RandomSingleton.Reset();
            //else
            //{
            //    if (int.TryParse(seed.Text, out int parsedSeed))
            //    {
            //        RandomSingleton.SetSeed(parsedSeed);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Invalid seed value. Please enter a valid integer.");
            //    }
            //}
            //InitialData.MatrixSize = matrixSizeInput.Value;
            //InitialData.Precision = (decimal)precisionInput.SelectedItem;
            //InitialData.NumberOfIndividuals = individualNumberInput.Value;
            //InitialData.CrossProbability = crossProbabilityInput.Value;
            //InitialData.MutationProbability = mutationProbabilityInput.Value;
            //InitialData.NumberOfIterations = iterationNumberInput.Value;
            //InitialData.NumberOfExperiments = experimentNumber.Value;
            //InitialData.CrossCount = crossPoints.Value;
            //InitialData.ProbGen1 = propGen1.Value;
            //InitialData.TournamentSelectionSize = tournamentSizeInput.Value;
            //InitialData.TournamentSoftSelectionTreshold = tournamentTresholdInput.Value;
            //historyOfIndividuals.Clear();
            //if (testExperimentCount.Value <= 0)
            //{
            //    MessageBox.Show("Liczba eksperyment�w musi by� wi�ksza od 0", "");
            //    return;
            //}

            //if (NaInput.Value <= 0 || TaInput.Value <= 0)
            //{
            //    MessageBox.Show("Ilo�� osobnik�w oraz ilo�� iteracji musi by� wi�ksza od 0", "");
            //    return;
            //}
            //if (NaInput.Value >= NbInput.Value || pkaInput.Value >= PkbbInput.Value || pmaInput.Value >= PmbInput.Value || TaInput.Value >= TbInput.Value || Ps_a.Value >= Ps_b_Input.Value || Rt_a_Input.Value >= rt_b_input.Value || ipk_input.Value >= ipk_b_input.Value)
            //{
            //    MessageBox.Show("Warto�� przedzia��w test�w nie mo�e by� odwrotna lub zerowa", "");
            //    return;
            //}
            //if (NstepInput.Value == 0 || PkstepInput.Value == 0 || PmstepInput.Value == 0 || TstepInput.Value == 0 || rt_step_input.Value == 0 || Ps_step.Value == 0 || ipk_step_input.Value == 0)
            //{
            //    MessageBox.Show("Warto�� kroku nie mo�e by� zerowa", "");
            //    return;
            //}

            //if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED && (InitialData.SupervisedReferenceMatrix == null || InitialData.SupervisedReferenceMatrix.Length == 0) ||
            //    InitialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED && (InitialData.UnsupervisedPatternMatrixes == null || InitialData.UnsupervisedPatternMatrixes.Length == 0))
            //{
            //    MessageBox.Show("Nie wybrano macierzy wzorców lub macierzy referencyjnej", "");
            //    return;
            //}

            //if (ipk_b_input.Value > matrixSizeInput.Value - 2)
            //{
            //    MessageBox.Show("Maksymalna ilo�� ci�� to rozmiar macierzy - 2", "");
            //    return;
            //}

            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //////Dictionary<InitialData, List<List<Individual>>> globalHistory = new();
            //List<TestObject> list = new();

            //var NValues = GenerateDecimalRange(NaInput.Value, NbInput.Value, NstepInput.Value);
            //var pkValues = GenerateDecimalRange(pkaInput.Value, PkbbInput.Value, PkstepInput.Value);
            //var pmValues = GenerateDecimalRange(pmaInput.Value, PmbInput.Value, PmstepInput.Value);
            //var tValues = GenerateDecimalRange(TaInput.Value, TbInput.Value, TstepInput.Value);
            //var rtValues = GenerateDecimalRange(Rt_a_Input.Value, rt_b_input.Value, rt_step_input.Value);
            //var psValues = GenerateDecimalRange(Ps_b_Input.Value, Ps_b_Input.Value, Ps_step.Value);
            //var ipkValues = GenerateDecimalRange(ipk_input.Value, ipk_b_input.Value, ipk_step_input.Value);
            //int maxCount = NValues.Count * pkValues.Count * pmValues.Count * tValues.Count * psValues.Count * rtValues.Count * ipkValues.Count;
            //int currentCount = 0;
            //decimal iter = 1;

            //MessageBox.Show("Zamknij okno aby kontyunuwac", "");

            //foreach (var n in NValues)
            //{
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
                    _data.MatrixSize = matrixSizeInput.Value;
                    ReferenceMatrixModalWindow referenceMatrixModalWindow = new ReferenceMatrixModalWindow(_data);
                    referenceMatrixModalWindow.Show();
                    break;

                case Core.Enums.AlgorithmType.UNSUPERVISED:
                    _data.MatrixSize = matrixSizeInput.Value;
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
            _data.MatrixSize = matrixSizeInput.Value;
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
            _narrowingMutationGroupBox.Enabled = false;
            unformBlocksGroupBox.Enabled = false;
            eliteGroupBox.Enabled = false;
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
                _narrowingMutationGroupBox.Enabled = false;
            }
        }

        private void bitSwapingRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.MutationType = MutationType.BIT_SWAPING;
                _narrowingMutationGroupBox.Enabled = false;
            }
        }

        private void randomMutationInput_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                _data.MutationType = MutationType.RANDOM_COORDS;
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
                MessageBox.Show("Brak element�w do podgl�du");
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
