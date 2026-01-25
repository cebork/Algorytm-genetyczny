using Lab2.Core.Domain;
using System.Data;


namespace Lab2.UI
{
    public partial class HistoryViewModalWindow : Form
    {
        List<List<Individual>> history;
        public HistoryViewModalWindow()
        {
            InitializeComponent();
        }

        public HistoryViewModalWindow(List<List<Individual>> history)
        {
            InitializeComponent();
            this.history = history;
            Individual bestIndividual = history[0].OrderByDescending(ind => ind.Genotype).FirstOrDefault();

            if (bestIndividual != null)
            {
                DisplayMatrix(bestIndividual.Genotype);
            }
            else
            {
                Console.WriteLine("No individuals found.");
            }
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
            historyDisplay.Enabled = false;
        }

        private void hisotryIndex_ValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    Individual bestIndividual = history[(int)hisotryIndex.Value - 1].OrderByDescending(ind => ind.MarkAfterMutation).FirstOrDefault();
            //    if (bestIndividual != null)
            //    {
            //        DisplayMatrix(bestIndividual.MatrixAfterMutation);
            //    }
            //    else
            //    {
            //        Console.WriteLine("No individuals found.");
            //    }
            //} 
            //catch (IndexOutOfRangeException ex)
            //{
            //    MessageBox.Show("Wartość wykracza poza ilość iteracji");
            //}
            //catch (ArgumentOutOfRangeException ex)
            //{
            //    MessageBox.Show("Wartość wykracza poza ilość iteracji");
            //}



        }
    }
}
