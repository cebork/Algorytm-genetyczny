namespace Lab2.UI
{
    partial class PatternChoosingModalWindow
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
            this.patternDataGroupBox = new System.Windows.Forms.GroupBox();
            this.patternAddButtom = new System.Windows.Forms.Button();
            this.patternSizeInput = new System.Windows.Forms.NumericUpDown();
            this.patternNameInput = new System.Windows.Forms.TextBox();
            this.patternMatrixInput = new System.Windows.Forms.DataGridView();
            this.patternNameLabel = new System.Windows.Forms.Label();
            this.patternSizeLabel = new System.Windows.Forms.Label();
            this.patternListGroupBox = new System.Windows.Forms.GroupBox();
            this.patternList = new System.Windows.Forms.ListView();
            this.setPatternsButton = new System.Windows.Forms.Button();
            this.patternDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.patternSizeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternMatrixInput)).BeginInit();
            this.patternListGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // patternDataGroupBox
            // 
            this.patternDataGroupBox.Controls.Add(this.patternAddButtom);
            this.patternDataGroupBox.Controls.Add(this.patternSizeInput);
            this.patternDataGroupBox.Controls.Add(this.patternNameInput);
            this.patternDataGroupBox.Controls.Add(this.patternMatrixInput);
            this.patternDataGroupBox.Controls.Add(this.patternNameLabel);
            this.patternDataGroupBox.Controls.Add(this.patternSizeLabel);
            this.patternDataGroupBox.Location = new System.Drawing.Point(12, 12);
            this.patternDataGroupBox.Name = "patternDataGroupBox";
            this.patternDataGroupBox.Size = new System.Drawing.Size(328, 372);
            this.patternDataGroupBox.TabIndex = 0;
            this.patternDataGroupBox.TabStop = false;
            this.patternDataGroupBox.Text = "Dane wzorca";
            // 
            // patternAddButtom
            // 
            this.patternAddButtom.Location = new System.Drawing.Point(189, 334);
            this.patternAddButtom.Name = "patternAddButtom";
            this.patternAddButtom.Size = new System.Drawing.Size(123, 23);
            this.patternAddButtom.TabIndex = 5;
            this.patternAddButtom.Text = "Dodaj wzorzec";
            this.patternAddButtom.UseVisualStyleBackColor = true;
            this.patternAddButtom.Click += new System.EventHandler(this.patternAddButtom_Click);
            // 
            // patternSizeInput
            // 
            this.patternSizeInput.Location = new System.Drawing.Point(113, 17);
            this.patternSizeInput.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.patternSizeInput.Name = "patternSizeInput";
            this.patternSizeInput.Size = new System.Drawing.Size(199, 23);
            this.patternSizeInput.TabIndex = 4;
            this.patternSizeInput.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.patternSizeInput.ValueChanged += new System.EventHandler(this.patternSizeInput_ValueChanged);
            // 
            // patternNameInput
            // 
            this.patternNameInput.Location = new System.Drawing.Point(113, 49);
            this.patternNameInput.Name = "patternNameInput";
            this.patternNameInput.Size = new System.Drawing.Size(199, 23);
            this.patternNameInput.TabIndex = 3;
            // 
            // patternMatrixInput
            // 
            this.patternMatrixInput.AllowUserToResizeColumns = false;
            this.patternMatrixInput.AllowUserToResizeRows = false;
            this.patternMatrixInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.patternMatrixInput.Location = new System.Drawing.Point(15, 81);
            this.patternMatrixInput.Name = "patternMatrixInput";
            this.patternMatrixInput.RowTemplate.Height = 25;
            this.patternMatrixInput.Size = new System.Drawing.Size(297, 238);
            this.patternMatrixInput.TabIndex = 2;
            this.patternMatrixInput.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.patternMatrixInput_CellClick);
            this.patternMatrixInput.CellFormatting += patternMatrixInput_CellFormatting;
            // 
            // patternNameLabel
            // 
            this.patternNameLabel.AutoSize = true;
            this.patternNameLabel.Location = new System.Drawing.Point(15, 53);
            this.patternNameLabel.Name = "patternNameLabel";
            this.patternNameLabel.Size = new System.Drawing.Size(82, 15);
            this.patternNameLabel.TabIndex = 1;
            this.patternNameLabel.Text = "Nazwa wzorca";
            // 
            // patternSizeLabel
            // 
            this.patternSizeLabel.AutoSize = true;
            this.patternSizeLabel.Location = new System.Drawing.Point(15, 21);
            this.patternSizeLabel.Name = "patternSizeLabel";
            this.patternSizeLabel.Size = new System.Drawing.Size(90, 15);
            this.patternSizeLabel.TabIndex = 0;
            this.patternSizeLabel.Text = "Rozmiar wzorca";
            // 
            // patternListGroupBox
            // 
            this.patternListGroupBox.Controls.Add(this.patternList);
            this.patternListGroupBox.Location = new System.Drawing.Point(346, 12);
            this.patternListGroupBox.Name = "patternListGroupBox";
            this.patternListGroupBox.Size = new System.Drawing.Size(442, 372);
            this.patternListGroupBox.TabIndex = 1;
            this.patternListGroupBox.TabStop = false;
            this.patternListGroupBox.Text = "Lista wzorców";
            // 
            // patternList
            // 
            this.patternList.Location = new System.Drawing.Point(6, 22);
            this.patternList.Name = "patternList";
            this.patternList.Size = new System.Drawing.Size(430, 344);
            this.patternList.TabIndex = 0;
            this.patternList.UseCompatibleStateImageBehavior = false;
            this.patternList.DoubleClick += patternListView_DoubleClick;
            // 
            // setPatternsButton
            // 
            this.setPatternsButton.Location = new System.Drawing.Point(604, 404);
            this.setPatternsButton.Name = "setPatternsButton";
            this.setPatternsButton.Size = new System.Drawing.Size(178, 23);
            this.setPatternsButton.TabIndex = 2;
            this.setPatternsButton.Text = "Ustaw wzorce";
            this.setPatternsButton.UseVisualStyleBackColor = true;
            this.setPatternsButton.Click += new System.EventHandler(this.setPatternsButton_Click);
            // 
            // PatternChoosingModalWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.setPatternsButton);
            this.Controls.Add(this.patternListGroupBox);
            this.Controls.Add(this.patternDataGroupBox);
            this.Name = "PatternChoosingModalWindow";
            this.Text = "Wybór wzorców";
            this.patternDataGroupBox.ResumeLayout(false);
            this.patternDataGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.patternSizeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternMatrixInput)).EndInit();
            this.patternListGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox patternDataGroupBox;
        private NumericUpDown patternSizeInput;
        private TextBox patternNameInput;
        private DataGridView patternMatrixInput;
        private Label patternNameLabel;
        private Label patternSizeLabel;
        private GroupBox patternListGroupBox;
        private ListView patternList;
        private Button patternAddButtom;
        private Button setPatternsButton;
    }
}