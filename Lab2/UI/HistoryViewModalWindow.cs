using Lab2.Core.Domain;
using System.Data;


namespace Lab2.UI
{
    public partial class HistoryViewModalWindow : Form
    {
        private readonly List<List<Individual>> history = new();

        public HistoryViewModalWindow()
        {
            InitializeComponent();
        }

        public HistoryViewModalWindow(List<List<Individual>> history)
        {
            InitializeComponent();

            this.history = history ?? new List<List<Individual>>();

            if (this.history.Count == 0)
            {
                hisotryIndex.Enabled = false;
                previousIndividual.Enabled = false;
                nextIndividual.Enabled = false;
                MessageBox.Show("Historia jest pusta. Uruchom algorytm przed podglądem historii.");
                return;
            }

            hisotryIndex.Minimum = 1;
            hisotryIndex.Maximum = this.history.Count;
            hisotryIndex.Value = 1;
            ShowGeneration(0);
        }

        private void previousIndividual_Click(object sender, EventArgs e)
        {
            if (hisotryIndex.Value == 1)
            {
                MessageBox.Show("Wartość wykracza poza ilość iteracji");
            }
            else
            {
                hisotryIndex.Value--;
            }
                
        }

        private void nextIndividual_Click(object sender, EventArgs e)
        {
            if (hisotryIndex.Value == history.Count)
            {
                MessageBox.Show("Wartość wykracza poza ilość iteracji");
            }
            else
            {
                hisotryIndex.Value++;
            }
        }

        private void DisplayMatrix(bool[,] matrix)
        {

            historyDisplay.Rows.Clear();
            historyDisplay.Columns.Clear();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);


            historyDisplay.AllowUserToAddRows = false;


            historyDisplay.ColumnHeadersVisible = false;
            historyDisplay.RowHeadersVisible = false;
            historyDisplay.ColumnCount = cols;


            foreach (DataGridViewColumn column in historyDisplay.Columns)
            {
                column.Width = 25;
            }


            for (int row = 0; row < rows; row++)
            {
                historyDisplay.Rows.Add();
                historyDisplay.Rows[row].Height = 25;
            }


            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var cell = historyDisplay.Rows[row].Cells[col];
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

            historyDisplay.ReadOnly = true;
            historyDisplay.Enabled = true;
            historyDisplay.ClearSelection();
        }

        private void hisotryIndex_ValueChanged(object sender, EventArgs e)
        {
            ShowGeneration((int)hisotryIndex.Value - 1);
        }

        private void ShowGeneration(int generationIndex)
        {
            if (generationIndex < 0 || generationIndex >= history.Count)
                return;

            Individual bestIndividual = history[generationIndex]
                .OrderByDescending(ind => ind.Fitness)
                .FirstOrDefault();

            if (bestIndividual == null)
                return;

            DisplayMatrix(bestIndividual.Genotype);
        }
    }
}
