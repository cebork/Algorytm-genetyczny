using Lab2.objects;
using Lab2.Utils;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
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
        }


        private async void startButton_Click(object sender, EventArgs e)
        {

            InitialData.MatrixSize = matrixSizeInput.Value;
            InitialData.Precision = (decimal) precisionInput.SelectedItem;
            InitialData.NumberOfIndividuals = individualNumberInput.Value;
            InitialData.CrossProbability = crossProbabilityInput.Value;
            InitialData.MutationProbability = mutationProbabilityInput.Value;
            InitialData.NumberOfIterations = iterationNumberInput.Value;

            historyOfIndividuals.Clear();

            try
            {
                new ValidationService(InitialData);

                runProgressBar.Visible = true;
                runProgressBar.Value = 0;
                runProgressBar.Maximum = (int) InitialData.NumberOfIterations;
                var progress = new Progress<int>(value =>
                {
                    runProgressBar.Value = value;
                });
                startButton.Enabled = false;

                try
                {
                    await Task.Run(() => AlgorithmRun(progress));
                }
                finally
                {
                    runProgressBar.Visible = false;
                    startButton.Enabled = true;
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
                    Mark = group.First().MarkAfterMutation,
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



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "B³¹d");
            }






        }




        private async void testyStart_Click(object sender, EventArgs e)
        {

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
            if (NaInput.Value >= NbInput.Value || pkaInput.Value >= PkbbInput.Value || pmaInput.Value >= PmbInput.Value || TaInput.Value >= TbInput.Value)
            {
                MessageBox.Show("Wartoœæ przedzia³ów testów nie mo¿e byæ odwrotna lub zerowa", "");
                return;
            }
            if (NstepInput.Value == 0 || PkstepInput.Value == 0 || PmstepInput.Value == 0 || TstepInput.Value == 0)
            {
                MessageBox.Show("Wartoœæ kroku nie mo¿e byæ zerowa", "");
                return;
            }

            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED && (InitialData.SupervisedReferenceMatrix == null || InitialData.SupervisedReferenceMatrix.Length == 0) || InitialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED && (InitialData.UnsupervisedPatternMatrixes == null || InitialData.UnsupervisedPatternMatrixes.Length == 0))
            {
                MessageBox.Show("Nie wybrano macierzy wzorców lub macierzy referencyjnej", "");
                return;
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Dictionary<InitialData, List<List<Individual>>>  globalHistory = new Dictionary<InitialData, List<List<Individual>>>(); 
            List<TestObject> list = new List<TestObject>();
            var listLock = new object();

            var NValues = GenerateDecimalRange(NaInput.Value, NbInput.Value, NstepInput.Value);
            var pkValues = GenerateDecimalRange(pkaInput.Value, PkbbInput.Value, PkstepInput.Value);
            var pmValues = GenerateDecimalRange(pmaInput.Value, PmbInput.Value, PmstepInput.Value);
            var tValues = GenerateDecimalRange(TaInput.Value, TbInput.Value, TstepInput.Value);

            

            MessageBox.Show("Zamknij okno aby kontyunuwac", "");
            decimal iter = 1;
            var tasks = new List<Task>();

            foreach (var n in NValues)
            {
                foreach (var pk in pkValues)
                {
                    foreach (var pm in pmValues)
                    {
                        foreach (var t in tValues)
                        {
                            tasks.Add(Task.Run(() =>
                            {
                                List<List<Individual>> localHistory = new List<List<Individual>>();

                                Parallel.For(0, (int)testExperimentCount.Value, x =>
                                {
                                    var individuals = Enumerable.Range(1, (int)n).Select(i => new Individual(i, matrixSizeInput.Value, 0.001m, pk, pm, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes)).ToList();

                                    for (int t2 = 0; t2 < t; t2++)
                                    {
                                        SelectionUtils.SetUpFitValue(individuals);
                                        SelectionUtils.SetUpDistribuator(individuals);
                                        SelectionUtils.SetUpNewOsobnikAfterSelection(individuals);
                                        Parallel.ForEach(individuals, item => {
                                            item.SetParent();
                                        });
                                        CrossUtils.SetCutPoint(individuals);
                                        CrossUtils.CrossOsobniks(individuals);
                                        CrossUtils.CreatePopulationAfterCrossing(individuals);
                                        Parallel.ForEach(individuals, item => {
                                            item.Mutate();
                                            item.MarkAfterMutation = item.SetOcena(item.MatrixAfterMutation);
                                        });




                                        historyOfIndividuals.Add(individuals);
                                        List<Individual> coppiedIndividuals = individuals.ToList();
                                        individuals = new List<Individual>();
                                        int idx = 1;
                                        foreach (Individual individual in coppiedIndividuals)
                                        {
                                            individuals.Add(new Individual(idx, matrixSizeInput.Value, 0.003m, crossProbabilityInput.Value, mutationProbabilityInput.Value, individual.MatrixAfterMutation, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes));
                                            idx++;
                                        }

                                    }
                                    localHistory.Add(individuals.ToList());
                                });

                                var avgMark = localHistory.SelectMany(os => os).Average(o => o.Mark);
                                var minMark = localHistory.SelectMany(os => os).Min(o => o.Mark);
                                var maxMark = localHistory.SelectMany(os => os).Max(o => o.Mark);
                                var testObject = new TestObject { Iter = iter, N = n, pk = pk, pm = pm, T = t, AvgMark = avgMark, MaxMark = maxMark, MinMark = minMark };
                                iter++;
                                lock (localHistory)
                                {
                                    InitialData initial = new InitialData()
                                    {
                                        MatrixSize = matrixSizeInput.Value,
                                        NumberOfIndividuals = n,
                                        NumberOfIterations = t,
                                        MutationProbability = pm,
                                        CrossProbability = pk,
                                        SupervisedReferenceMatrix = InitialData.SupervisedReferenceMatrix,
                                        UnsupervisedPatternMatrixes = InitialData.UnsupervisedPatternMatrixes
                                    };
                                    globalHistory.Add(initial, localHistory);
                                    list.Add(testObject);
                                }
                            }));
                        }
                    }
                }
            }

            await Task.WhenAll(tasks);



            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            TimeSpan elapsed = TimeSpan.FromMilliseconds(elapsedMs);

            string elapsedFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                    elapsed.Hours,
                                                    elapsed.Minutes,
                                                    elapsed.Seconds,
                                                    elapsed.Milliseconds);
            MessageBox.Show("Liczba wyników: " + list.Count().ToString() + "\nPotrzebny czas: " + elapsedFormatted, "Sukces");

            InitialData.MatrixSize = matrixSizeInput.Value;
            InitialData.Precision = (decimal) precisionInput.SelectedItem;
            FileUtils.saveGaTunningResults(list, InitialData);
            FileUtils.SaveMResultsGa(globalHistory);
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
                InitialData.AlgorithmType = Core.Enums.AlgorithmType.SUPERVISED;
                additioanlDataButton.Text = "Wybór macierzy referencyjnej";
            }
        }

        private void unsupervisedTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selected = sender as RadioButton;
            if (selected != null && selected.Checked)
            {
                InitialData.AlgorithmType = Core.Enums.AlgorithmType.UNSUPERVISED;
                additioanlDataButton.Text = "Wybór macierzy wzorców";
            }
        }

        private void AlgorithmRun(IProgress<int> progress)
        {
            List<Individual> individuals = new List<Individual>();
            for (int i = 1; i <= individualNumberInput.Value; i++)
            {
                individuals.Add(new Individual(i, matrixSizeInput.Value, (decimal)precisionInput.SelectedItem, crossProbabilityInput.Value, mutationProbabilityInput.Value, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes));
            }
            for (int t = 0; t < iterationNumberInput.Value; t++)
            {

                
                SelectionUtils.SetUpFitValue(individuals);
                SelectionUtils.SetUpDistribuator(individuals);
                SelectionUtils.SetUpNewOsobnikAfterSelection(individuals);
                Parallel.ForEach(individuals, item => {
                        item.SetParent();
                });
                CrossUtils.SetCutPoint(individuals);
                CrossUtils.CrossOsobniks(individuals);
                CrossUtils.CreatePopulationAfterCrossing(individuals);
                Parallel.ForEach(individuals, item => {
                    item.Mutate();
                    item.MarkAfterMutation = item.SetOcena(item.MatrixAfterMutation);
                });




                historyOfIndividuals.Add(individuals);
                List<Individual> coppiedIndividuals = individuals.ToList();
                individuals = new List<Individual>();
                int idx = 1;
                foreach (Individual individual in coppiedIndividuals)
                {
                    individuals.Add(new Individual(idx, matrixSizeInput.Value, (decimal)precisionInput.SelectedItem, crossProbabilityInput.Value, mutationProbabilityInput.Value, individual.MatrixAfterMutation, InitialData.SupervisedReferenceMatrix, InitialData.AlgorithmType, InitialData.UnsupervisedPatternMatrixes));
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
    }
}