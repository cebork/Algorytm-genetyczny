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
                    // Run your long operation asynchronously
                    await Task.Run(() => AlgorithmRun(progress));
                }
                finally
                {
                    // Hide loading
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

                bool[,] test = sumUps.First().Matrix;



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
            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //List<TestObject> list = new List<TestObject>();
            //var listLock = new object();

            //var nValues = Enumerable.Range(30, 51).Where(n => (n - 30) % 5 == 0).ToList();
            //var pkValues = Enumerable.Range(0, 9).Select(pkIndex => 0.5 + pkIndex * 0.05).ToList();
            //var pmValues = new List<double> { 0.0001, 0.0005, 0.001, 0.002, 0.003, 0.004, 0.005, 0.006, 0.007, 0.008, 0.009, 0.01 };
            //var tValues = Enumerable.Range(50, 101).Where(t => (t - 50) % 10 == 0).ToList();
            //MessageBox.Show("Zamknij okno aby kontyunuwac", "");

            //var tasks = new List<Task>();

            //foreach (var n in nValues)
            //{
            //    foreach (var pk in pkValues)
            //    {
            //        foreach (var pm in pmValues)
            //        {
            //            foreach (var t in tValues)
            //            {
            //                tasks.Add(Task.Run(() =>
            //                {
            //                    List<List<Osobnik>> localHistory = new List<List<Osobnik>>();

            //                    Parallel.For(0, 10, x =>
            //                    {
            //                        var osobniks = Enumerable.Range(1, n).Select(i => new Osobnik(i, -4, 12, 0.001, pk, pm)).ToList();

            //                        for (int t2 = 0; t2 < t; t2++)
            //                        {
            //                            SelectionUtils.SetUpFitValue(osobniks);
            //                            SelectionUtils.SetUpDistribuator(osobniks);
            //                            SelectionUtils.SetUpNewOsobnikAfterSelection(osobniks);

            //                            Parallel.ForEach(osobniks, item =>
            //                            {
            //                                item.RealToBin(item.XRealAfterSelection);
            //                                item.SetParent();
            //                            });

            //                            CrossUtils.SetCutPoint(osobniks);
            //                            CrossUtils.CrossOsobniks(osobniks);
            //                            CrossUtils.CreatePopulationAfterCrossing(osobniks);

            //                            Parallel.ForEach(osobniks, item =>
            //                            {
            //                                item.Mutate();
            //                                item.XRealAfterMutation = item.BinaryToReal(item.xBinAfterMutation);
            //                                item.MarkAfterMutation = item.SetOcena(item.XRealAfterMutation);
            //                            });

            //                            List<Osobnik> coppiedOsobniks = osobniks.ToList();
            //                            osobniks = new List<Osobnik>();
            //                            int idx = 1;
            //                            foreach (Osobnik osobnik in coppiedOsobniks)
            //                            {
            //                                osobniks.Add(new Osobnik(idx, -4, 12, 0.001, pk, pm, osobnik.XRealAfterMutation));
            //                                idx++;
            //                            }
                                        
            //                        }
            //                        localHistory.Add(osobniks.ToList());
            //                    });

            //                    var avgMark = localHistory.SelectMany(os => os).Average(o => o.Mark);
            //                    var testObject = new TestObject { N = n, pk = Math.Round(pk, 3), pm = Math.Round(pm, 4), T = t, AvgMark = Math.Round(avgMark, 3) };

            //                    lock (localHistory)
            //                    {
            //                        list.Add(testObject);
            //                    }
            //                }));
            //            }
            //        }
            //    }
            //}

            //await Task.WhenAll(tasks);

            
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            //TimeSpan elapsed = TimeSpan.FromMilliseconds(elapsedMs);

            //string elapsedFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
            //                                        elapsed.Hours,
            //                                        elapsed.Minutes,
            //                                        elapsed.Seconds,
            //                                        elapsed.Milliseconds);
            //MessageBox.Show("Liczba wyników: " + list.Count().ToString() + "\nPotrzebny czas: " + elapsedFormatted , "Sukces");
            //testy.DataSource = list.OrderByDescending(x => x.AvgMark).ToList();
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
            DisplayMatrix(individuals.OrderByDescending(o => o.Mark).First().IndividualMatrix);
        }
    }
}