using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2.UI
{
    public partial class PatternPreviewModalWindow : Form
    {
        private bool[,] pattern;

        public PatternPreviewModalWindow()
        {
            InitializeComponent();
        }

        public PatternPreviewModalWindow(bool[,] pattern)
        {
            InitializeComponent();
            this.pattern = pattern;

            InitPatternGrid();
        }


        private void InitPatternGrid()
        {
            int rows = pattern.GetLength(0);
            int cols = pattern.GetLength(1);

            patternPreview.Rows.Clear();
            patternPreview.Columns.Clear();

            patternPreview.RowHeadersVisible = false;
            patternPreview.ColumnHeadersVisible = false;
            patternPreview.ReadOnly = true;
            patternPreview.AllowUserToAddRows = false;
            patternPreview.AllowUserToResizeRows = false;
            patternPreview.AllowUserToResizeColumns = false;

            // Add columns
            for (int col = 0; col < cols; col++)
            {
                var column = new DataGridViewTextBoxColumn();
                column.Width = 30;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                patternPreview.Columns.Add(column);
            }

            // Add rows
            patternPreview.RowCount = rows;
            for (int row = 0; row < rows; row++)
            {
                patternPreview.Rows[row].Height = 30;
            }

            // Populate cells
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    bool value = pattern[row, col];
                    patternPreview[col, row].Value = value;
                    patternPreview[col, row].Style.BackColor = value ? Color.Red : Color.White;
                }
            }

            patternPreview.ClearSelection();
        }

        private void patternPreview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = patternPreview.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value is bool)
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
