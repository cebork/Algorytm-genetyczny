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
                MessageBox.Show("Nie wybrano dok³adnoœci");
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


                finalOsobniks.Add(GEOUtils.GenerateInitialOsobnik(initialData));
                bests.Add(GEOUtils.GenerateInitialOsobnik(initialData));
                FinalOsobnik objToCompare = null;
                for (int i = 0; i < initialData.T; i++)
                {
                    objToCompare = GEOUtils.Mutate(initialData, finalOsobniks.Last());
                    if(objToCompare.Mark > finalOsobniks.Last().Mark)
                    {
                        bests.Add(objToCompare);
                    }
                    else
                    {
                        bests.Add(finalOsobniks.Last());
                    }
                    finalOsobniks.Add(objToCompare);
                }

                osobniki.DataSource = finalOsobniks;
            }
        }




        private async void testyStart_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<TestObject> list = new List<TestObject>();
            var listLock = new object();

            var nValues = Enumerable.Range(30, 51).Where(n => (n - 30) % 5 == 0).ToList();
            var pkValues = Enumerable.Range(0, 9).Select(pkIndex => 0.5 + pkIndex * 0.05).ToList();
            var pmValues = new List<double> { 0.0001, 0.0005, 0.001, 0.002, 0.003, 0.004, 0.005, 0.006, 0.007, 0.008, 0.009, 0.01 };
            var tValues = Enumerable.Range(50, 101).Where(t => (t - 50) % 10 == 0).ToList();
            MessageBox.Show("Zamknij okno aby kontyunuwac", "");

            var tasks = new List<Task>();

            foreach (var n in nValues)
            {
                foreach (var pk in pkValues)
                {
                    foreach (var pm in pmValues)
                    {
                        foreach (var t in tValues)
                        {
                            tasks.Add(Task.Run(() =>
                            {
                                List<List<Osobnik>> localHistory = new List<List<Osobnik>>();

                                Parallel.For(0, 10, x =>
                                {
                                    var osobniks = Enumerable.Range(1, n).Select(i => new Osobnik(i, -4, 12, 0.001, pk, pm)).ToList();

                                    for (int t2 = 0; t2 < t; t2++)
                                    {
                                        SelectionUtils.SetUpFitValue(osobniks);
                                        SelectionUtils.SetUpDistribuator(osobniks);
                                        SelectionUtils.SetUpNewOsobnikAfterSelection(osobniks);

                                        Parallel.ForEach(osobniks, item =>
                                        {
                                            item.RealToBin(item.XRealAfterSelection);
                                            item.SetParent();
                                        });

                                        CrossUtils.SetCutPoint(osobniks);
                                        CrossUtils.CrossOsobniks(osobniks);
                                        CrossUtils.CreatePopulationAfterCrossing(osobniks);

                                        Parallel.ForEach(osobniks, item =>
                                        {
                                            item.Mutate();
                                            item.XRealAfterMutation = item.BinaryToReal(item.xBinAfterMutation);
                                            item.MarkAfterMutation = item.SetOcena(item.XRealAfterMutation);
                                        });

                                        List<Osobnik> coppiedOsobniks = osobniks.ToList();
                                        osobniks = new List<Osobnik>();
                                        int idx = 1;
                                        foreach (Osobnik osobnik in coppiedOsobniks)
                                        {
                                            osobniks.Add(new Osobnik(idx, -4, 12, 0.001, pk, pm, osobnik.XRealAfterMutation));
                                            idx++;
                                        }
                                        
                                    }
                                    localHistory.Add(osobniks.ToList());
                                });

                                var avgMark = localHistory.SelectMany(os => os).Average(o => o.Mark);
                                var testObject = new TestObject { N = n, pk = Math.Round(pk, 3), pm = Math.Round(pm, 4), T = t, AvgMark = Math.Round(avgMark, 3) };

                                lock (localHistory)
                                {
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
            MessageBox.Show("Liczba wyników: " + list.Count().ToString() + "\nPotrzebny czas: " + elapsedFormatted , "Sukces");
            testy.DataSource = list.OrderByDescending(x => x.AvgMark).ToList();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}