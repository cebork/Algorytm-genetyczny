namespace Lab2.UI
{
    partial class HistoryViewModalWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            historyDisplay = new DataGridView();
            previousIndividual = new Button();
            nextIndividual = new Button();
            hisotryIndex = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)historyDisplay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hisotryIndex).BeginInit();
            SuspendLayout();
            // 
            // historyDisplay
            // 
            historyDisplay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            historyDisplay.Location = new Point(12, 59);
            historyDisplay.Name = "historyDisplay";
            historyDisplay.RowTemplate.Height = 25;
            historyDisplay.Size = new Size(1234, 899);
            historyDisplay.TabIndex = 20;
            // 
            // previousIndividual
            // 
            previousIndividual.BackColor = SystemColors.ActiveCaption;
            previousIndividual.Location = new Point(16, 12);
            previousIndividual.Name = "previousIndividual";
            previousIndividual.Size = new Size(25, 23);
            previousIndividual.TabIndex = 21;
            previousIndividual.Text = "<";
            previousIndividual.UseVisualStyleBackColor = false;
            previousIndividual.Click += previousIndividual_Click;
            // 
            // nextIndividual
            // 
            nextIndividual.BackColor = SystemColors.ActiveCaption;
            nextIndividual.Location = new Point(112, 12);
            nextIndividual.Name = "nextIndividual";
            nextIndividual.Size = new Size(25, 23);
            nextIndividual.TabIndex = 22;
            nextIndividual.Text = ">";
            nextIndividual.UseVisualStyleBackColor = false;
            nextIndividual.Click += nextIndividual_Click;
            // 
            // hisotryIndex
            // 
            hisotryIndex.Location = new Point(47, 12);
            hisotryIndex.Maximum = new decimal(new int[] { 1215752191, 23, 0, 0 });
            hisotryIndex.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            hisotryIndex.Name = "hisotryIndex";
            hisotryIndex.Size = new Size(59, 23);
            hisotryIndex.TabIndex = 24;
            hisotryIndex.Value = new decimal(new int[] { 1, 0, 0, 0 });
            hisotryIndex.ValueChanged += hisotryIndex_ValueChanged;
            // 
            // HistoryViewModalWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1258, 970);
            Controls.Add(hisotryIndex);
            Controls.Add(nextIndividual);
            Controls.Add(previousIndividual);
            Controls.Add(historyDisplay);
            Name = "HistoryViewModalWindow";
            Text = "Podgląd historii";
            ((System.ComponentModel.ISupportInitialize)historyDisplay).EndInit();
            ((System.ComponentModel.ISupportInitialize)hisotryIndex).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView historyDisplay;
        private Button previousIndividual;
        private Button nextIndividual;
        private NumericUpDown hisotryIndex;
    }
}