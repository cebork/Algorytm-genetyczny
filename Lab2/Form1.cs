using Lab2.GEO.domain;
using Lab2.objects;
using Lab2.Utils;
using System.Collections.Concurrent;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab2
{
    public partial class Form1 : Form
    {
        List<FinalOsobnik> finalOsobniks = new List<FinalOsobnik>();
        List<FinalOsobnik> bests = new List<FinalOsobnik>();
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
            if (dInput.SelectedItem == null)
            {
                MessageBox.Show("Nie wybrano dokładności");
            }
            else
            {
                osobniki.DataSource = new List<string>();
                InitialData initialData = new InitialData(
                    (int)aInput.Value,
                    (int)bInput.Value,
                    (double)dInput.SelectedItem,
                    (int)tValue.Value,
                    (double)tetaValue.Value,
                    GeneralUtils.GetChromosomeLength((int)aInput.Value, (int)bInput.Value, (double)dInput.SelectedItem),
                    GeneralUtils.GetPrecision((double)dInput.SelectedItem)
                    );

                finalOsobniks.Clear();
                bests.Clear();
                finalOsobniks.Add(GEOUtils.GenerateInitialOsobnik(initialData));
                bests.Add(GEOUtils.GenerateInitialOsobnik(initialData));
                FinalOsobnik objToCompare = null;
                for (int i = 0; i < initialData.T; i++)
                {
                    objToCompare = GEOUtils.Mutate(initialData, finalOsobniks.Last());
                    if(objToCompare.Mark > bests.Last().Mark)
                    {
                        bests.Add(objToCompare);
                    }
                    else
                    {
                        bests.Add(bests.Last());
                    }
                    finalOsobniks.Add(objToCompare);
                }

                bestXBin.Text = bests.Last().XBin;
                bestxReal.Text = bests.Last().XReal.ToString();
                bestMark.Text = bests.Last().Mark.ToString();

                osobniki.DataSource = finalOsobniks;


                Dictionary<int, double> allValues = new Dictionary<int, double>();
                for (int i = 0; i < finalOsobniks.Count; i++)
                {
                    allValues.Add(i, finalOsobniks[i].Mark);
                }

                Dictionary<int, double> bestsLocal = new Dictionary<int, double>();
                for (int i = 0; i < bests.Count; i++)
                {
                    bestsLocal.Add(i, bests[i].Mark);
                }


                chart1.Series.Clear();


                var series1 = new Series
                {
                    Name = "Każdy osobnik",
                    Color = System.Drawing.Color.Blue,
                    ChartType = SeriesChartType.Line
                };
                foreach (var point in allValues)
                {
                    series1.Points.AddXY(point.Key, point.Value);
                }
                chart1.Series.Add(series1);


                var series2 = new Series
                {
                    Name = "Najlepsi osobnicy",
                    Color = System.Drawing.Color.Red,
                    ChartType = SeriesChartType.Line
                };
                foreach (var point in bestsLocal)
                {
                    series2.Points.AddXY(point.Key, point.Value);
                }
                chart1.Series.Add(series2);


                var chartArea = new ChartArea();
                chartArea.AxisX.Title = "Iteracje";
                chartArea.AxisY.Title = "Oceny";
                chart1.ChartAreas.Clear();
                chart1.ChartAreas.Add(chartArea);

                chart1.ChartAreas[0].BackColor = System.Drawing.Color.LightYellow;
            }
        }




        private async void testyStart_Click(object sender, EventArgs e)
        {
            List<FinalOsobnik> testBest = new List<FinalOsobnik>();
            List<FinalOsobnik> testFinalOsobniks = new List<FinalOsobnik>();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<GeoTestObject> list = new List<GeoTestObject>();
            var listLock = new object();

            var tValues = new List<int> { 2000, 3000, 4000, 5000 };
            var tetaValues = Enumerable.Range(0, (int)((5 - 0.5) / 0.1) + 1)
                           .Select(pkIndex => 0.5 + pkIndex * 0.1)
                           .ToList();
            int innerLoop = 100;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = tValues.Count * tetaValues.Count * innerLoop;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            MessageBox.Show("Zamknij okno aby kontyunuwac", "");

            int progresBarIter = 1;
            foreach (var teta in tetaValues)
            {
                foreach (var t in tValues)
                {

                    List<FinalOsobnik> localHistory = new List<FinalOsobnik>();

                    for (int test = 0; test < innerLoop; test++)
                    {

                        InitialData initialData = new InitialData(
                            -4,
                            12,
                            0.001,
                            t,
                            teta,
                            GeneralUtils.GetChromosomeLength(-4, 12, 0.001),
                            GeneralUtils.GetPrecision(0.001)
                        );

                        testFinalOsobniks.Clear();
                        testBest.Clear();
                        testFinalOsobniks.Add(GEOUtils.GenerateInitialOsobnik(initialData));
                        testBest.Add(GEOUtils.GenerateInitialOsobnik(initialData));
                        for (int i = 0; i < initialData.T; i++)
                        {
                            FinalOsobnik objToCompare = GEOUtils.Mutate(initialData, testFinalOsobniks.Last());
                            if (objToCompare.Mark > testBest.Last().Mark)
                            {
                                testBest.Add(objToCompare);
                            }
                            else
                            {
                                testBest.Add(testBest.Last());
                            }
                            testFinalOsobniks.Add(objToCompare);
                        }
                        localHistory.Add(testBest.Last());
                        Invoke(new Action(() => progressBar1.Value = progresBarIter++));
                    }

                    var avgMark = localHistory.Average(o => o.Mark);
                    var testObject = new GeoTestObject { T = t, teta = teta, Mark = Math.Round(avgMark, 3) };

                    list.Add(testObject);
                }
            }

            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            TimeSpan elapsed = TimeSpan.FromMilliseconds(elapsedMs);

            string elapsedFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                    elapsed.Hours,
                                                    elapsed.Minutes,
                                                    elapsed.Seconds,
                                                    elapsed.Milliseconds);
            MessageBox.Show("Liczba wyników: " + list.Count().ToString() + "\nPotrzebny czas: " + elapsedFormatted , "Sukces");
            testy.DataSource = list.OrderByDescending(x => x.Mark).ToList();

            testMark.Text = list.OrderByDescending(x => x.Mark).ToList().First().Mark.ToString();
            testT.Text = list.OrderByDescending(x => x.Mark).ToList().First().T.ToString();
            testTeta.Text = list.OrderByDescending(x => x.Mark).ToList().First().teta.ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void testT_Click(object sender, EventArgs e)
        {

        }
    }
}