using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab2.Core.Domain;

namespace Lab2.UI
{
    public partial class ReferenceMatrixModalWindow : Form
    {

        public InitialData InitialData { get; set; }

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

            int size = (int)initialData.MatrixSize;

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

    }
}
