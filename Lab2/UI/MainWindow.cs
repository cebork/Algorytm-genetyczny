using Lab2.objects;
using Lab2.Utils;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Lab2.Core.Enums;
using Lab2.UI;
using Lab2.Services;
using Lab2.Core.Domain;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;

namespace Lab2
{
    public partial class MainWindow : Form
    {
        InitialData InitialData;

        List<List<Individual>> historyOfIndividuals = new List<List<Individual>>();

        public MainWindow()
        {
            InitialData = new InitialData();
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitialData.MatrixSize = matrixSizeInput.Value;

            precisionInput.Items.Add(0.1m);
            precisionInput.Items.Add(0.01m);
            precisionInput.Items.Add(0.001m);
            precisionInput.Items.Add(0.0001m);

            precisionInput.SelectedItem = 0.001m;
            supervisedTypedRadioButton.Checked = true;

            setupDefaultAlgorithmOtpions();
            seed.Enabled = false;
        }


        private async void startButton_Click(object sender, EventArgs e)
        {
            if (!useSeed.Checked)
                RandomSingleton.Reset();
            else
            {
                if (int.TryParse(seed.Text, out int parsedSeed))
                {
                    RandomSingleton.SetSeed(parsedSeed);
                }
                else
                {
                    MessageBox.Show("Invalid seed value. Please enter a valid integer.");
                }
            }
            InitialData.MatrixSize = matrixSizeInput.Value;
            InitialData.Precision = (decimal)precisionInput.SelectedItem;
            InitialData.NumberOfIndividuals = individualNumberInput.Value;
            InitialData.CrossProbability = crossProbabilityInput.Value;
            InitialData.MutationProbability = mutationProbabilityInput.Value;
            InitialData.NumberOfIterations = iterationNumberInput.Value;
            InitialData.NumberOfExperiments = experimentNumber.Value;
            InitialData.CrossCount = crossPoints.Value;
            InitialData.ProbGen1 = propGen1.Value;
            InitialData.TournamentSelectionSize = tournamentSizeInput.Value;
            InitialData.TournamentSoftSelectionTreshold = tournamentTresholdInput.Value;
            historyOfIndividuals.Clear();

            try
            {
                new ValidationService(InitialData);
                for (int i = 0; i < InitialData.NumberOfExperiments; i++)
                {
                    historyOfIndividuals.Clear();
                    runProgressBar.Visible = true;
                    runProgressBar.Value = 0;
                    runProgressBar.Maximum = (int)InitialData.NumberOfIterations;
                    var progress = new Progress<int>(value =>
                    {
                        runProgressBar.Value = value;
                    });
                    startButton.Enabled = false;

                    try
                    {
                        decimal precision = 0.001m;
                        this.Invoke((MethodInvoker)(() => precision = (decimal)precisionInput.SelectedItem));
                        await Task.Run(() => AlgorithmRun(progress, precision));
                    }
                    finally
                    {
                        runProgressBar.Visible = false;
                        startButton.Enabled = true;
                        if (i == 0)
                            FileUtils.SaveMaxFCCorr(historyOfIndividuals, InitialData, false);
                        else
                            FileUtils.SaveMaxFCCorr(historyOfIndividuals, InitialData, true);
                    }
                }


                List<Individual> lastGeneration = historyOfIndividuals.Last();
                int totalCount = lastGeneration.Count;
                int xe = 1;
                List<SumUp> sumUps = lastGeneration
                .OrderByDescending(o => o.MarkAfterMutation)
                .GroupBy(individual => individual.MarkAfterMutation)
                .Select(group => new SumUp
                {
                    lp = xe++,
                    Matrix = group.First().MatrixAfterMutation,
                    f_C_Corr = group.First().MarkAfterMutation,
                    Percentage = (decimal)group.Count() / totalCount * 100
                })
                .ToList();
                osobniki.DataSource = sumUps;

                DisplayMatrix(lastGeneration.OrderByDescending(o => o.MarkAfterMutation).First().MatrixAfterMutation);
                FileUtils.SaveResultsGa(historyOfIndividuals, InitialData);

                Dictionary<int, decimal> maxValues = new Dictionary<int, decimal>
                        {
                            { 0, historyOfIndividuals.First().Max(osb => osb.Mark) }
                        };
                for (int i = 0; i < historyOfIndividuals.Count; i++)
                {
                    decimal max = historyOfIndividuals[i].Max(osobnik => osobnik.MarkAfterMutation);
                    maxValues.Add(i + 1, max);
                }



                Dictionary<int, decimal> avgValues = new Dictionary<int, decimal>
                        {
                            { 0, historyOfIndividuals.First().Average(osb => osb.Mark) }
                        };
                for (int i = 0; i < historyOfIndividuals.Count; i++)
                {
                    decimal avg = historyOfIndividuals[i].Average(osobnik => osobnik.MarkAfterMutation);
                    avgValues.Add(i + 1, avg);
                }

                Dictionary<int, decimal> minValues = new Dictionary<int, decimal>
                        {
                            { 0, historyOfIndividuals.First().Min(osb => osb.Mark) }
                        };
                for (int i = 0; i < historyOfIndividuals.Count; i++)
                {
                    decimal min = historyOfIndividuals[i].Min(osobnik => osobnik.MarkAfterMutation);
                    minValues.Add(i + 1, min);
                }


                chart1.Series.Clear();


                var series1 = new Series
                {
                    Name = "Maximum",
                    Color = Color.Blue,
                    ChartType = SeriesChartType.Line
                };
                foreach (var point in maxValues)
                {
                    series1.Points.AddXY(point.Key, point.Value);
                }
                chart1.Series.Add(series1);


                var series2 = new Series
                {
                    Name = "Œrednia",
                    Color = Color.Red,
                    ChartType = SeriesChartType.Line
                };
                foreach (var point in avgValues)
                {
                    series2.Points.AddXY(point.Key, point.Value);
                }
                chart1.Series.Add(series2);


                var series3 = new Series
                {
                    Name = "Minimum",
                    Color = Color.Green,
                    ChartType = SeriesChartType.Line
                };
                foreach (var point in minValues)
                {
                    series3.Points.AddXY(point.Key, point.Value);
                }
                chart1.Series.Add(series3);

                var chartArea = new ChartArea();
                chartArea.AxisX.Title = "Pokolenia";
                chartArea.AxisY.Title = "Oceny";
                chart1.ChartAreas.Clear();
                chart1.ChartAreas.Add(chartArea);

                chart1.ChartAreas[0].BackColor = Color.LightYellow;

                seed.Text = RandomSingleton.GetUsedSeed().ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "B³¹d");
            }






        }




        private void testyStart_Click(object sender, EventArgs e)
        {
            if (!useSeed.Checked)
                RandomSingleton.Reset();
            else
            {
                if (int.TryParse(seed.Text, out int parsedSeed))
                {
                    RandomSingleton.SetSeed(parsedSeed);
                }
                else
                {
                    MessageBox.Show("Invalid seed value. Please enter a valid integer.");
                }
            }
            InitialData.MatrixSize = matrixSizeInput.Value;
            InitialData.Precision = (decimal)precisionInput.SelectedItem;
            InitialData.NumberOfIndividuals = individualNumberInput.Value;
            InitialData.CrossProbability = crossProbabilityInput.Value;
            InitialData.MutationProbability = mutationProbabilityInput.Value;
            InitialData.NumberOfIterations = iterationNumberInput.Value;
            InitialData.NumberOfExperiments = experimentNumber.Value;
            InitialData.CrossCount = crossPoints.Value;
            InitialData.ProbGen1 = propGen1.Value;
            InitialData.TournamentSelectionSize = tournamentSizeInput.Value;
            InitialData.TournamentSoftSelectionTreshold = tournamentTresholdInput.Value;
            historyOfIndividuals.Clear();
            if (testExperimentCount.Value <= 0)
            {
                MessageBox.Show("Liczba eksperymentów musi byæ wiêksza od 0", "");
                return;
            }

            if (NaInput.Value <= 0 || TaInput.Value <= 0)
            {
                MessageBox.Show("Iloœæ osobników oraz iloœæ iteracji musi byæ wiêksza od 0", "");
                return;
            }
            if (NaInput.Value >= NbInput.Value || pkaInput.Value >= PkbbInput.Value || pmaInput.Value >= PmbInput.Value || TaInput.Value >= TbInput.Value || Ps_a.Value >= Ps_b_Input.Value || Rt_a_Input.Value >= rt_b_input.Value || ipk_input.Value >= ipk_b_input.Value)
            {
                MessageBox.Show("Wartoœæ przedzia³ów testów nie mo¿e byæ odwrotna lub zerowa", "");
                return;
            }
            if (NstepInput.Value == 0 || PkstepInput.Value == 0 || PmstepInput.Value == 0 || TstepInput.Value == 0 || rt_step_input.Value == 0 || Ps_step.Value == 0 || ipk_step_input.Value == 0)
            {
                MessageBox.Show("Wartoœæ kroku nie mo¿e byæ zerowa", "");
                return;
            }

            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED && (InitialData.SupervisedReferenceMatrix == null || InitialData.SupervisedReferenceMatrix.Length == 0) ||
                InitialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED && (InitialData.UnsupervisedPatternMatrixes == null || InitialData.UnsupervisedPatternMatrixes.Length == 0))
            {
                MessageBox.Show("Nie wybrano macierzy wzorców lub macierzy referencyjnej", "");
                return;
            }

            if (ipk_b_input.Value <= matrixSizeInput.Value - 2)
            {
                MessageBox.Show("Maksymalna iloœæ ciêæ to rozmiar macierzy - 2", "");
                return;
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            ////Dictionary<InitialData, List<List<Individual>>> globalHistory = new();
            List<TestObject> list = new();

            var NValues = GenerateDecimalRange(NaInput.Value, NbInput.Value, NstepInput.Value);
            var pkValues = GenerateDecimalRange(pkaInput.Value, PkbbInput.Value, PkstepInput.Value);
            var pmValues = GenerateDecimalRange(pmaInput.Value, PmbInput.Value, PmstepInput.Value);
            var tValues = GenerateDecimalRange(TaInput.Value, TbInput.Value, TstepInput.Value);
            var rtValues = GenerateDecimalRange(Rt_a_Input.Value, rt_b_input.Value, rt_step_input.Value);
            var psValues = GenerateDecimalRange(Ps_b_Input.Value, Ps_b_Input.Value, Ps_step.Value);
            var ipkValues = GenerateDecimalRange(ipk_input.Value, ipk_b_input.Value, ipk_step_input.Value);
            int maxCount = NValues.Count * pkValues.Count * pmValues.Count * tValues.Count;
            int currentCount = 0;
            decimal iter = 1;

            MessageBox.Show("Zamknij okno aby kontyunuwac", "");

            foreach (var n in NValues)
            {
                foreach (var pk in pkValues)
                {
                    foreach (var pm in pmValues)
                    {
                        foreach (var t in tValues)
                        {
                            foreach (var rt in rtValues)
                            {
                                foreach(var ps in psValues)
                                {
                                    foreach (var ipk in ipkValues)
                                    {
                                        InitialData.TournamentSelectionSize = rt;
                                        InitialData.TournamentSoftSelectionTreshold = ps;
                                        InitialData.CrossCount = ipk;

                                        testCounter.Text = $"Test {++currentCount} / {maxCount}";
                                        individualCount.Text = $"Iloœæ osobników {n}";
                                        mutationProb.Text = $"Prawdopodobieñstwo mutacji {pm}";
                                        crossProb.Text = $"Prawdopodobieñstwo krzy¿owania {pk}";
                                        iterationCount.Text = $"Iloœæ iteracji {t}";
                                        tournamentSizeLabelTesty.Text = $"Rozmiar turnieju {rt}";
                                        selectionTresholLabelTesty.Text = $"Próg selekcji {ps}";
                                        ipkLabelTesty.Text = $"Iloœæ punktów krzy¿owañ {ipk}";
                                        Application.DoEvents();

                                        List<List<Individual>> localHistory = new();

                                        for (int x = 0; x < testExperimentCount.Value; x++)
                                        {
                                            var individuals = Enumerable.Range(1, (int)n)
                                                .Select(i => new Individual(i, matrixSizeInput.Value, 0.001m, pk, pm, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes, InitialData.ProbGen1))
                                                .ToList();

                                            for (int t2 = 0; t2 < t; t2++)
                                            {
                                                if (InitialData.SelectionType == SelectionType.ROULETTE)
                                                {
                                                    SelectionUtils.SetUpFitValue(individuals);
                                                    SelectionUtils.SetUpDistribuator(individuals);
                                                    SelectionUtils.SetUpNewOsobnikAfterSelection(individuals);
                                                }
                                                else if (InitialData.SelectionType == SelectionType.TOURNAMENT_HARD)
                                                {
                                                    SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentHard(individuals, InitialData.TournamentSelectionSize);
                                                }
                                                else if (InitialData.SelectionType == SelectionType.TOURNAMENT_SOFT)
                                                {
                                                    SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentSoft(individuals, InitialData.TournamentSelectionSize, InitialData.TournamentSoftSelectionTreshold);
                                                }

                                                for (int i = 0; i < individuals.Count; i++)
                                                    individuals[i].SetParent();

                                                if (InitialData.CrossType == CrossType.SINGLE_POINT)
                                                {
                                                    CrossUtils.SetCutPoint(individuals);
                                                    CrossUtils.CrossOsobniks(individuals);
                                                    CrossUtils.CreatePopulationAfterCrossing(individuals);
                                                }
                                                else
                                                {
                                                    CrossUtils.CreatePopulationAfterCrossingNPoints(individuals, InitialData.CrossCount);
                                                }

                                                for (int i = 0; i < individuals.Count; i++)
                                                {
                                                    if (InitialData.UniformBlock)
                                                    {
                                                        individuals[i].MutateUniformBlock();
                                                    }
                                                    switch (InitialData.MutationType)
                                                    {
                                                        case MutationType.EQUALY:
                                                            individuals[i].Mutate();
                                                            break;
                                                        case MutationType.BIT_SWAPING:
                                                            individuals[i].BitSwapMutation();
                                                            break;
                                                        case MutationType.RANDOM_COORDS:
                                                            individuals[i].MutateByNarrowing(i, (int)t);
                                                            break;
                                                    }

                                                    individuals[i].MarkAfterMutation = individuals[i].SetOcena(individuals[i].MatrixAfterMutation);
                                                }

                                                //historyOfIndividuals.Add(individuals);
                                                var copiedIndividuals = individuals.ToList();
                                                individuals = new();

                                                int idx = 1;
                                                for (int i = 0; i < copiedIndividuals.Count; i++)
                                                {
                                                    individuals.Add(new Individual(idx++, matrixSizeInput.Value, 0.003m, crossProbabilityInput.Value, mutationProbabilityInput.Value, copiedIndividuals[i].MatrixAfterMutation, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes));
                                                }
                                            }

                                            localHistory.Add(individuals.ToList());
                                        }

                                        var avgMark = localHistory.SelectMany(os => os).Average(o => o.Mark);
                                        var minMark = localHistory.SelectMany(os => os).Min(o => o.Mark);
                                        var maxMark = localHistory.SelectMany(os => os).Max(o => o.Mark);

                                        var testObject = new TestObject
                                        {
                                            Iter = iter++,
                                            N = n,
                                            pk = pk,
                                            pm = pm,
                                            T = t,
                                            AvgMark = avgMark,
                                            MaxMark = maxMark,
                                            MinMark = minMark
                                        };

                                        var initial = new InitialData()
                                        {
                                            MatrixSize = matrixSizeInput.Value,
                                            NumberOfIndividuals = n,
                                            NumberOfIterations = t,
                                            MutationProbability = pm,
                                            CrossProbability = pk,
                                            SupervisedReferenceMatrix = InitialData.SupervisedReferenceMatrix,
                                            UnsupervisedPatternMatrixes = InitialData.UnsupervisedPatternMatrixes
                                        };

                                        //globalHistory.Add(initial, localHistory);
                                        list.Add(testObject);

                                        localHistory.Clear();
                                        GC.Collect();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            watch.Stop();
            var elapsed = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
            string elapsedFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);

            MessageBox.Show("Liczba wyników: " + list.Count + "\nPotrzebny czas: " + elapsedFormatted, "Sukces");

            InitialData.MatrixSize = matrixSizeInput.Value;
            InitialData.Precision = (decimal)precisionInput.SelectedItem;
            FileUtils.saveGaTunningResults(list, InitialData);
            //FileUtils.SaveMResultsGa(globalHistory);

            //historyOfIndividuals.Clear();
            //globalHistory.Clear();
            list.Clear();
            GC.Collect();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            switch (InitialData.AlgorithmType)
            {
                case Core.Enums.AlgorithmType.SUPERVISED:
                    ReferenceMatrixModalWindow referenceMatrixModalWindow = new ReferenceMatrixModalWindow(InitialData);
                    referenceMatrixModalWindow.Show();
                    break;

                case Core.Enums.AlgorithmType.UNSUPERVISED:
                    PatternChoosingModalWindow patternChoosingModalWindow = new PatternChoosingModalWindow(InitialData);
                    patternChoosingModalWindow.Show();
                    break;

                default:
                    Console.WriteLine("Unknown algorithm type");
                    break;
            }

        }

        private void matrixSizeInput_ValueChanged(object sender, EventArgs e)
        {
            InitialData.MatrixSize = matrixSizeInput.Value;
        }

        private void supervisedTypedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.AlgorithmType = AlgorithmType.SUPERVISED;
                additioanlDataButton.Text = "Wybór macierzy referencyjnej";
            }
        }

        private void unsupervisedTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.AlgorithmType = AlgorithmType.UNSUPERVISED;
                additioanlDataButton.Text = "Wybór macierzy wzorców";
            }
        }

        private void AlgorithmRun(IProgress<int> progress, decimal precision)
        {
            List<Individual> individuals = new List<Individual>();
            for (int i = 1; i <= individualNumberInput.Value; i++)
            {
                individuals.Add(new Individual(i, matrixSizeInput.Value, precision, crossProbabilityInput.Value, mutationProbabilityInput.Value, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes, InitialData.ProbGen1));
            }
            for (int t = 0; t < iterationNumberInput.Value; t++)
            {

                if (InitialData.SelectionType.Equals(SelectionType.ROULETTE))
                {
                    SelectionUtils.SetUpFitValue(individuals);
                    SelectionUtils.SetUpDistribuator(individuals);
                    SelectionUtils.SetUpNewOsobnikAfterSelection(individuals);
                }
                else if (InitialData.SelectionType.Equals(SelectionType.TOURNAMENT_HARD))
                {
                    SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentHard(individuals, InitialData.TournamentSelectionSize);
                }
                else if (InitialData.SelectionType.Equals(SelectionType.TOURNAMENT_SOFT))
                {
                    SelectionUtils.SetUpNewOsobnikAfterSelectionTournamentSoft(individuals, InitialData.TournamentSelectionSize, InitialData.TournamentSoftSelectionTreshold);
                }

                for (int i = 0; i < individuals.Count; i++)
                {
                    individuals[i].SetParent();
                }
                if (InitialData.CrossType.Equals(CrossType.SINGLE_POINT))
                {
                    CrossUtils.SetCutPoint(individuals);
                    CrossUtils.CrossOsobniks(individuals);
                    CrossUtils.CreatePopulationAfterCrossing(individuals);
                }
                else
                {
                    CrossUtils.CreatePopulationAfterCrossingNPoints(individuals, InitialData.CrossCount);
                }

                for (int i = 0; i < individuals.Count; i++)
                {
                    if (InitialData.UniformBlock)
                    {
                        individuals[i].MutateUniformBlock();
                    }

                    if (InitialData.MutationType.Equals(MutationType.EQUALY))
                    {
                        individuals[i].Mutate();
                    }
                    else if (InitialData.MutationType.Equals(MutationType.EQUALY))
                    {
                        individuals[i].BitSwapMutation();
                    }
                    else if (InitialData.MutationType.Equals(MutationType.RANDOM_COORDS))
                    {
                        individuals[i].MutateByNarrowing(i, (int)InitialData.NumberOfIterations);
                    }
                    individuals[i].MarkAfterMutation = individuals[i].SetOcena(individuals[i].MatrixAfterMutation);
                }




                historyOfIndividuals.Add(individuals);
                List<Individual> coppiedIndividuals = individuals.ToList();
                individuals = new List<Individual>();
                int idx = 1;
                for (int i = 0; i < coppiedIndividuals.Count; i++)
                {
                    individuals.Add(new Individual(idx, matrixSizeInput.Value, precision, crossProbabilityInput.Value, mutationProbabilityInput.Value, coppiedIndividuals[i].MatrixAfterMutation, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes));
                    idx++;
                }

                progress?.Report(t + 1);


            }


        }

        private List<decimal> GenerateDecimalRange(decimal start, decimal end, decimal step)
        {
            if (step <= 0)
                throw new ArgumentException("Step must be positive and non-zero.");

            var result = new List<decimal>();
            for (decimal value = start; value <= end; value += step)
            {
                result.Add(decimal.Round(value, 10));
            }
            return result;
        }

        private void classicalGA_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.AlgorithmOption = AlgorithmOption.CLASSICAL;
                InitialData.SelectionType = SelectionType.ROULETTE;
                InitialData.CrossType = CrossType.SINGLE_POINT;
                InitialData.MutationType = MutationType.EQUALY;
                setupDefaultAlgorithmOtpions();
            }
        }

        private void modifiedGARadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.AlgorithmOption = AlgorithmOption.MODIFIED;
                InitialData.SelectionType = SelectionType.ROULETTE;
                InitialData.CrossType = CrossType.SINGLE_POINT;
                InitialData.MutationType = MutationType.EQUALY;

                selectionGroup.Enabled = true;
                crossGroup.Enabled = true;
                mutationGroup.Enabled = true;
            }
        }

        private void setupDefaultAlgorithmOtpions()
        {

            classicalGARadio.Checked = true;
            ruletteRadio.Checked = true;
            singlePointRadio.Checked = true;
            evenlyRadio.Checked = true;

            selectionGroup.Enabled = false;
            crossGroup.Enabled = false;
            mutationGroup.Enabled = false;
        }

        private void ruletteRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.SelectionType = SelectionType.ROULETTE;
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
                InitialData.SelectionType = SelectionType.TOURNAMENT_HARD;
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
                InitialData.SelectionType = SelectionType.TOURNAMENT_SOFT;
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
                InitialData.CrossType = CrossType.SINGLE_POINT;
                crossPoints.Enabled = false;
                crossCountGroupbox.Enabled = false;
            }
        }

        private void multiPointRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.CrossType = CrossType.MULTI_POINT;
                crossPoints.Enabled = true;
                crossCountGroupbox.Enabled = true;
            }
        }

        private void evenlyRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.MutationType = MutationType.EQUALY;
            }
        }

        private void bitSwapingRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.MutationType = MutationType.BIT_SWAPING;
            }
        }

        private void randomMutationInput_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.MutationType = MutationType.RANDOM_COORDS;
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
            if (historyOfIndividuals.Count != 0)
            {
                HistoryViewModalWindow historyViewModalWindow = new HistoryViewModalWindow(historyOfIndividuals);
                historyViewModalWindow.Show();
            }
            else
            {
                MessageBox.Show("Brak elementów do podgl¹du");
            }


        }

        private void uniformBlock_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null && checkbox.Checked)
            {
                InitialData.UniformBlock = checkbox.Checked;
            }
        }

    }
}