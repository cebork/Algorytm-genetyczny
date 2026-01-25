using System;
using System.Drawing;
using System.Windows.Forms;
using Lab2.Core.Domain;
using Lab2.UI.Domain;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Lab2.Services;
using Lab2.Infrastructure;
namespace Lab2.UI
{
    public partial class PatternChoosingModalWindow : Form
    {
        public InitialData InitialData { get; set; }
        private int currentMatrixSize;

        List<PatternChoosingDisplayColumns> patternChoosingDisplayColumns = new List<PatternChoosingDisplayColumns>();

        public PatternChoosingModalWindow()
        {
            InitializeComponent();
        }

        public PatternChoosingModalWindow(InitialData initialData)
        {
            InitializeComponent();
            InitialData = initialData;

            var allPatterns = FileUtils.LoadAllPatterns();
            patternChoosingDisplayColumns = allPatterns;
            setupPatternListView();


            patternMatrixInput.DefaultCellStyle.SelectionBackColor = patternMatrixInput.DefaultCellStyle.BackColor;
            patternMatrixInput.DefaultCellStyle.SelectionForeColor = patternMatrixInput.DefaultCellStyle.ForeColor;

            patternSizeInput.ValueChanged += patternSizeInput_ValueChanged;
            patternSizeInput.Validated += patternSizeInput_Validated;

            currentMatrixSize = (int)patternSizeInput.Value;

            patternMatrixInput.ColumnCount = currentMatrixSize;
            patternMatrixInput.RowCount = currentMatrixSize;

            patternMatrixInput.RowHeadersVisible = false;
            patternMatrixInput.ColumnHeadersVisible = false;

            for (int i = 0; i < currentMatrixSize; i++)
            {
                patternMatrixInput.Columns[i].Width = 30;
                patternMatrixInput.Rows[i].Height = 30;
            }

            createMatrixInput();

            patternMatrixInput.ReadOnly = true;
            patternMatrixInput.CurrentCell = null;
            patternMatrixInput.ClearSelection();
        }

        private void patternMatrixInput_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = patternMatrixInput.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value is bool)
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }

        private void patternSizeInput_ValueChanged(object sender, EventArgs e)
        {
            currentMatrixSize = (int)patternSizeInput.Value;
            createMatrixInput();
        }

        private void patternSizeInput_Validated(object sender, EventArgs e)
        {
            patternSizeInput_ValueChanged(sender, e);
        }

        private void createMatrixInput()
        {
            patternMatrixInput.SuspendLayout();

            patternMatrixInput.Rows.Clear();
            patternMatrixInput.Columns.Clear();

            patternMatrixInput.RowHeadersVisible = false;
            patternMatrixInput.ColumnHeadersVisible = false;

            for (int i = 0; i < currentMatrixSize; i++)
            {
                var col = new DataGridViewTextBoxColumn();
                col.Width = 30;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                patternMatrixInput.Columns.Add(col);
            }

            patternMatrixInput.RowCount = currentMatrixSize;

            for (int i = 0; i < currentMatrixSize; i++)
            {
                patternMatrixInput.Rows[i].Height = 30;
            }

            for (int row = 0; row < currentMatrixSize; row++)
            {
                for (int col = 0; col < currentMatrixSize; col++)
                {
                    bool value = false;

                    if (InitialData.SupervisedReferenceMatrix != null &&
                        InitialData.SupervisedReferenceMatrix.GetLength(0) == currentMatrixSize &&
                        InitialData.SupervisedReferenceMatrix.GetLength(1) == currentMatrixSize)
                    {
                        value = InitialData.SupervisedReferenceMatrix[row, col];
                    }

                    patternMatrixInput[col, row].Value = value;
                    patternMatrixInput[col, row].Style.BackColor = value ? Color.Red : Color.White;
                }
            }

            patternMatrixInput.ReadOnly = true;
            patternMatrixInput.CurrentCell = null;
            patternMatrixInput.ClearSelection();
            patternMatrixInput.ResumeLayout();
            patternMatrixInput.Refresh();
        }

        private void patternAddButtom_Click(object sender, EventArgs e)
        {
            int size = patternMatrixInput.RowCount;
            bool[,] patternMatrix = new bool[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    var cellValue = patternMatrixInput[col, row].Value;

                    if (cellValue is bool b)
                    {
                        patternMatrix[row, col] = b;
                    }
                    else
                    {
                        patternMatrix[row, col] = false;
                    }
                }
            }

            var patternChoosingDisplayColumn = new PatternChoosingDisplayColumns()
            {
                PatternSize = (int)patternSizeInput.Value,
                PatternName = patternNameInput.Text,
                PatternMatrix = patternMatrix
            };


            try
            {
                new ValidationService(patternChoosingDisplayColumn);

                FileUtils.AppendPatternToFile(patternChoosingDisplayColumn);
                patternChoosingDisplayColumns.Add(patternChoosingDisplayColumn);

                var item = new ListViewItem(patternChoosingDisplayColumn.PatternName);
                item.SubItems.Add(patternChoosingDisplayColumn.PatternSize.ToString());
                item.Tag = patternChoosingDisplayColumn.PatternMatrix;
                item.Checked = false;

                patternList.Items.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }

        }


        private void setupPatternListView()
        {


            patternList.Items.Clear();
            patternList.View = View.Details;
            patternList.CheckBoxes = true;

            if (patternList.Columns.Count == 0)
            {
                patternList.Columns.Add("Nazwa wzorca", 150);
                patternList.Columns.Add("Rozmiar wzorca", 150);
            }

            foreach (var pattern in patternChoosingDisplayColumns)
            {
                var item = new ListViewItem(pattern.PatternName);
                item.SubItems.Add(pattern.PatternSize.ToString());
                item.Checked = false;
                item.Tag = pattern.PatternMatrix;
                patternList.Items.Add(item);
            }
        }

        private void patternMatrixInput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int lastRow = patternMatrixInput.RowCount - 1;
            int lastCol = patternMatrixInput.ColumnCount - 1;


            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = patternMatrixInput.Rows[e.RowIndex].Cells[e.ColumnIndex];

                bool current = Convert.ToBoolean(cell.Value ?? false);
                bool next = !current;
                cell.Value = next;

                cell.Style.BackColor = next ? Color.Red : Color.White;
            }

            patternMatrixInput.CurrentCell = null;
            patternMatrixInput.ClearSelection();
        }

        private void patternListView_DoubleClick(object sender, EventArgs e)
        {
            if (patternList.SelectedItems.Count > 0)
            {
                var selectedItem = patternList.SelectedItems[0];
                var data = selectedItem.Tag as bool[,];

                var modal = new PatternPreviewModalWindow(data);
                modal.ShowDialog();
            }
        }

        private void setPatternsButton_Click(object sender, EventArgs e)
        {
            var patternListTemp = new List<bool[,]>();

            foreach (ListViewItem item in patternList.CheckedItems)
            {
                bool[,] data = item.Tag as bool[,];

                if (data != null)
                {
                    patternListTemp.Add(data);
                }
            }

            InitialData.UnsupervisedPatternMatrixes = patternListTemp.ToArray();
            Close();
        }

    }
}
