using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab2.Core.Domain;
using Lab2.Infrastructure;
using Lab2.Services;
using Lab2.UI.Domain;

namespace Lab2.UI
{
    public partial class ReferenceMatrixModalWindow : Form
    {

        public InitialData InitialData { get; set; }

        List<ReferenceMatrixDisplayColumns> referenceMatrixDisplayColumns1 = new List<ReferenceMatrixDisplayColumns>();

        public ReferenceMatrixModalWindow()
        {
            InitializeComponent();
        }

        public ReferenceMatrixModalWindow(InitialData initialData)
        {
            InitialData = initialData;
            InitializeComponent();

            referenceMatrixInput.DefaultCellStyle.SelectionBackColor = referenceMatrixInput.DefaultCellStyle.BackColor;
            referenceMatrixInput.DefaultCellStyle.SelectionForeColor = referenceMatrixInput.DefaultCellStyle.ForeColor;

            int size = GetDisplayedMatrixSize();

            referenceMatrixInput.ColumnCount = size;
            referenceMatrixInput.RowCount = size;

            referenceMatrixInput.RowHeadersVisible = false;
            referenceMatrixInput.ColumnHeadersVisible = false;

            for (int i = 0; i < size; i++)
            {
                referenceMatrixInput.Columns[i].Width = 30;
                referenceMatrixInput.Rows[i].Height = 30;
            }

            // Fill from matrix if provided
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    bool value = false;

                    if (initialData.SupervisedReferenceMatrix != null &&
                        initialData.SupervisedReferenceMatrix.GetLength(0) == size &&
                        initialData.SupervisedReferenceMatrix.GetLength(1) == size)
                    {
                        value = initialData.SupervisedReferenceMatrix[row, col];
                    }

                    var cell = referenceMatrixInput.Rows[row].Cells[col];
                    cell.Value = value;
                    cell.Style.BackColor = value ? Color.Red : Color.White;
                }
            }

            referenceMatrixInput.ReadOnly = true;
            referenceMatrixInput.CurrentCell = null;
            referenceMatrixInput.ClearSelection();

            var allReferenceMatrixies = FileUtils.LoadAllReferenceMatrixes(GetDisplayedMatrixSize());
            referenceMatrixDisplayColumns1 = allReferenceMatrixies;
            setupReferenceMatrixListView();
        }

        private int GetDisplayedMatrixSize()
        {
            return InitialData.UseEvenMatrixPadding
                ? (int)InitialData.RequestedMatrixSize
                : (int)InitialData.MatrixSize;
        }


        private void referenceMatrixListView_DoubleClick(object sender, EventArgs e)
        {
            if (referenceMatrixList.SelectedItems.Count > 0)
            {
                var selectedItem = referenceMatrixList.SelectedItems[0];
                var data = selectedItem.Tag as bool[,];

                int size = GetDisplayedMatrixSize();
                InitialData.SupervisedReferenceMatrix = data;

                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        bool value = false;

                        if (InitialData.SupervisedReferenceMatrix != null &&
                            InitialData.SupervisedReferenceMatrix.GetLength(0) == size &&
                            InitialData.SupervisedReferenceMatrix.GetLength(1) == size)
                        {
                            value = InitialData.SupervisedReferenceMatrix[row, col];
                        }

                        var cell = referenceMatrixInput.Rows[row].Cells[col];
                        cell.Value = value;
                        cell.Style.BackColor = value ? Color.Red : Color.White;
                    }
                }

            }
        }

        private void setupReferenceMatrixListView()
        {


            referenceMatrixList.Items.Clear();
            referenceMatrixList.View = View.Details;
            referenceMatrixList.CheckBoxes = false;

            if (referenceMatrixList.Columns.Count == 0)
            {
                referenceMatrixList.Columns.Add("Nazwa macierzry referencyjnej", 250);
                referenceMatrixList.Columns.Add("Rozmiar", 50);
            }

            foreach (var referenceMatrix in referenceMatrixDisplayColumns1)
            {
                var item = new ListViewItem(referenceMatrix.ReferenceMatrixName);
                item.SubItems.Add(referenceMatrix.MatrixSize.ToString());
                item.Checked = false;
                item.Tag = referenceMatrix.ReferenceMatrix;
                referenceMatrixList.Items.Add(item);
            }
        }


        private void referenceMatrixInput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int lastRow = referenceMatrixInput.RowCount - 1;
            int lastCol = referenceMatrixInput.ColumnCount - 1;


            if (e.RowIndex == 0 || e.RowIndex == lastRow || e.ColumnIndex == 0 || e.ColumnIndex == lastCol)
                return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = referenceMatrixInput.Rows[e.RowIndex].Cells[e.ColumnIndex];

                bool current = Convert.ToBoolean(cell.Value ?? false);
                bool next = !current;
                cell.Value = next;

                cell.Style.BackColor = next ? Color.Red : Color.White;
            }

            referenceMatrixInput.CurrentCell = null;
            referenceMatrixInput.ClearSelection();
        }

        private void referenceMatrixInput_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = referenceMatrixInput.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value is bool)
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            int size = referenceMatrixInput.RowCount;
            bool[,] matrix = new bool[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    bool value = Convert.ToBoolean(referenceMatrixInput.Rows[row].Cells[col].Value ?? false);
                    matrix[row, col] = value;
                }
            }

            InitialData.SupervisedReferenceMatrix = matrix;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void saveReferenceTable_Click(object sender, EventArgs e)
        {
            try
            {
                new ValidationService(referenceTableName.Text);
                int size = referenceMatrixInput.RowCount;
                bool[,] matrix = new bool[size, size];

                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        bool value = Convert.ToBoolean(referenceMatrixInput.Rows[row].Cells[col].Value ?? false);
                        matrix[row, col] = value;
                    }
                }

                ReferenceMatrixDisplayColumns referenceMatrixDisplayColumns = new ReferenceMatrixDisplayColumns()
                {
                    ReferenceMatrixName = referenceTableName.Text,
                    MatrixSize = GetDisplayedMatrixSize(),
                    ReferenceMatrix = matrix
                };

                FileUtils.AppendReferenceMatrixToFile(referenceMatrixDisplayColumns);
                referenceMatrixDisplayColumns1.Add(referenceMatrixDisplayColumns);

                var item = new ListViewItem(referenceMatrixDisplayColumns.ReferenceMatrixName);
                item.SubItems.Add(referenceMatrixDisplayColumns.MatrixSize.ToString());
                item.Tag = referenceMatrixDisplayColumns.ReferenceMatrix;
                item.Checked = false;

                referenceMatrixList.Items.Add(item);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd");
            }



        }

        private void generateMatrix_Click(object sender, EventArgs e)
        {
            int size = GetDisplayedMatrixSize();
            InitialData.SupervisedReferenceMatrix = new bool[size, size];

            for (int row = 1; row < size - 1; row++)
            {
                for (int col = 1; col < size - 1; col++)
                {
                    bool value = (row % 2 == 1) && (col % 2 == 1);
                    InitialData.SupervisedReferenceMatrix[row, col] = value;

                    var cell = referenceMatrixInput.Rows[row].Cells[col];
                    cell.Value = value;
                    cell.Style.BackColor = value ? Color.Red : Color.White;
                }
            }
        }

    }
}
