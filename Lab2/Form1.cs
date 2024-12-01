using Lab2.objects;
using Lab2.Utils;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab2
{
    public partial class Form1 : Form
    {

        Random random = new Random();

        List<List<Osobnik>> historyOfOsobniks = new List<List<Osobnik>>();    

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dInput.Items.Add(0.1);
            dInput.Items.Add(0.01);
            dInput.Items.Add(0.001);
            dInput.Items.Add(0.0001);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void bInput_ValueChanged(object sender, EventArgs e)
        {

        }

        private void aInput_ValueChanged(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            historyOfOsobniks.Clear();
            if (aInput.Value == null || dInput.SelectedItem == null || bInput.Value == null || nInput.Value == null || pk.Value == null || pm.Value == null || TInput.Value == null)
            {
                MessageBox.Show("Nie wszystkie pola zosta³y uzupe³nione", "B³¹d");
            } 
            else
            {

                if ((int)aInput.Value <= 0 || (int)bInput.Value <= 0)
                {
                    MessageBox.Show("Macierze nie mog¹ mieæ ujemnych wymiarów", "B³¹d");
                } 
                else
                {
                    if ((pk.Value > 1 || pk.Value < 0) || (pm.Value > 1 || pm.Value < 0))
                    {
                        MessageBox.Show("Niepoprawny przedzia³ dla pm lub pk", "B³¹d");
                    } else
                    {
                        List<Osobnik> osobniks = new List<Osobnik>();
                        for (int i = 1; i <= nInput.Value; i++)
                        {
                            osobniks.Add(new Osobnik(i, (int) aInput.Value, (int) bInput.Value, (double) dInput.SelectedItem, (double) pk.Value, (double) pm.Value));
                            
                        }
                        for (int t = 0; t < TInput.Value; t++)
                        {
                            
                            SelectionUtils.SetUpFitValue(osobniks);
                            SelectionUtils.SetUpDistribuator(osobniks);
                            SelectionUtils.SetUpNewOsobnikAfterSelection(osobniks);
                            foreach (var item in osobniks)
                            {
                                item.SetParent();
                            }
                            CrossUtils.SetCutPoint(osobniks);
                            CrossUtils.CrossOsobniks(osobniks);
                            CrossUtils.CreatePopulationAfterCrossing(osobniks);
                            foreach (var item in osobniks)
                            {
                                item.Mutate();
                                //item.XRealAfterMutation = item.BinaryToReal(item.xBinAfterMutation);
                                item.MarkAfterMutation = item.SetOcena(item.MatrixAfterMutation);
                            }


                          
                            historyOfOsobniks.Add(osobniks);
                            List<Osobnik> coppiedOsobniks = osobniks.ToList();
                            osobniks = new List<Osobnik>();
                            int idx = 1;
                            foreach (Osobnik osobnik in coppiedOsobniks)
                            {
                                osobniks.Add(new Osobnik(idx, (int)aInput.Value, (int)bInput.Value, (double)dInput.SelectedItem, (double)pk.Value, (double)pm.Value, osobnik.MatrixAfterMutation));
                                idx++;
                            }
                            
                            DisplayMatrix(osobniks.First().Matrix);
                        }

                        List<Osobnik> lastGeneration = historyOfOsobniks.Last();
                        int totalCount = lastGeneration.Count;
                        int xe = 1;
                        List<SumUp> sumUps = lastGeneration
                        .GroupBy(osobik => osobik.XRealAfterMutation)
                        .Select(group => new SumUp
                        {
                            lp = xe++,
                            xReal = group.Key,
                            Matrix = group.First().MatrixAfterMutation,
                            Mark = group.First().MarkAfterMutation,
                            Percentage = (double)group.Count() / totalCount * 100
                        })
                        .ToList();
                        osobniki.DataSource = sumUps;

                        bool[,] test = sumUps.First().Matrix;

                        

                        Dictionary<int, double> maxValues = new Dictionary<int, double>
                        {
                            { 0, historyOfOsobniks.First().Max(osb => osb.Mark) }
                        };
                        for (int i = 0; i < historyOfOsobniks.Count; i++)
                        {
                            double max = historyOfOsobniks[i].Max(osobnik => osobnik.MarkAfterMutation);
                            maxValues.Add(i + 1, max);
                        }



                        Dictionary<int, double> avgValues = new Dictionary<int, double>
                        {
                            { 0, historyOfOsobniks.First().Average(osb => osb.Mark) }
                        };
                        for (int i = 0; i < historyOfOsobniks.Count; i++)
                        {
                            double avg = historyOfOsobniks[i].Average(osobnik => osobnik.MarkAfterMutation);
                            avgValues.Add(i + 1, avg);
                        }

                        Dictionary<int, double> minValues = new Dictionary<int, double>
                        {
                            { 0, historyOfOsobniks.First().Min(osb => osb.Mark) }
                        };
                        for (int i = 0; i < historyOfOsobniks.Count; i++)
                        {
                            double min = historyOfOsobniks[i].Min(osobnik => osobnik.MarkAfterMutation);
                            minValues.Add(i + 1, min);
                        }


                        chart1.Series.Clear();


                        var series1 = new Series
                        {
                            Name = "Maximum",
                            Color = System.Drawing.Color.Blue,
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
                            Color = System.Drawing.Color.Red,
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
                            Color = System.Drawing.Color.Green,
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

                        chart1.ChartAreas[0].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
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
                column.Width = 30;
            }

  
            for (int row = 0; row < rows; row++)
            {
                display.Rows.Add();
                display.Rows[row].Height = 30;
            }


            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var cell = display.Rows[row].Cells[col];
                    if (matrix[row, col])
                    {
                        cell.Style.BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        cell.Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }

            display.ReadOnly = true;
            display.Enabled = false;

            Application.DoEvents();


            Thread.Sleep(50);

            display.ClearSelection();

        }

    }
}