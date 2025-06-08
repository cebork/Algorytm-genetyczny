namespace Lab2.UI
{
    partial class ReferenceMatrixModalWindow
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
            this.referenceMatrixInput = new System.Windows.Forms.DataGridView();
            this.setButton = new System.Windows.Forms.Button();
            this.saveReferenceTable = new System.Windows.Forms.Button();
            this.referenceTableName = new System.Windows.Forms.TextBox();
            this.referenceTableLabel = new System.Windows.Forms.Label();
            this.referenceMatrixList = new System.Windows.Forms.ListView();
            this.generateMatrix = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.referenceMatrixInput)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceMatrixInput
            // 
            this.referenceMatrixInput.AllowUserToResizeColumns = false;
            this.referenceMatrixInput.AllowUserToResizeRows = false;
            this.referenceMatrixInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.referenceMatrixInput.Location = new System.Drawing.Point(12, 12);
            this.referenceMatrixInput.Name = "referenceMatrixInput";
            this.referenceMatrixInput.RowTemplate.Height = 25;
            this.referenceMatrixInput.Size = new System.Drawing.Size(631, 565);
            this.referenceMatrixInput.TabIndex = 0;
            this.referenceMatrixInput.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.referenceMatrixInput_CellClick);
            this.referenceMatrixInput.CellFormatting += referenceMatrixInput_CellFormatting;
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(12, 583);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 61);
            this.setButton.TabIndex = 1;
            this.setButton.Text = "Ustaw macierz";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // saveReferenceTable
            // 
            this.saveReferenceTable.Location = new System.Drawing.Point(922, 602);
            this.saveReferenceTable.Name = "saveReferenceTable";
            this.saveReferenceTable.Size = new System.Drawing.Size(174, 23);
            this.saveReferenceTable.TabIndex = 2;
            this.saveReferenceTable.Text = "Zapisz tablice refernacyjną";
            this.saveReferenceTable.UseVisualStyleBackColor = true;
            this.saveReferenceTable.Click += new System.EventHandler(this.saveReferenceTable_Click);
            // 
            // referenceTableName
            // 
            this.referenceTableName.Location = new System.Drawing.Point(649, 602);
            this.referenceTableName.Name = "referenceTableName";
            this.referenceTableName.Size = new System.Drawing.Size(267, 23);
            this.referenceTableName.TabIndex = 3;
            // 
            // referenceTableLabel
            // 
            this.referenceTableLabel.AutoSize = true;
            this.referenceTableLabel.Location = new System.Drawing.Point(492, 606);
            this.referenceTableLabel.Name = "referenceTableLabel";
            this.referenceTableLabel.Size = new System.Drawing.Size(151, 15);
            this.referenceTableLabel.TabIndex = 4;
            this.referenceTableLabel.Text = "Nazwa tablicy referencyjnej";
            // 
            // referenceMatrixList
            // 
            this.referenceMatrixList.Location = new System.Drawing.Point(662, 12);
            this.referenceMatrixList.Name = "referenceMatrixList";
            this.referenceMatrixList.Size = new System.Drawing.Size(421, 565);
            this.referenceMatrixList.TabIndex = 5;
            this.referenceMatrixList.UseCompatibleStateImageBehavior = false;
            // 
            // generateMatrix
            // 
            this.generateMatrix.Location = new System.Drawing.Point(93, 583);
            this.generateMatrix.Name = "generateMatrix";
            this.generateMatrix.Size = new System.Drawing.Size(113, 61);
            this.generateMatrix.TabIndex = 6;
            this.generateMatrix.Text = "Generuj macierz";
            this.generateMatrix.UseVisualStyleBackColor = true;
            this.generateMatrix.Click += new System.EventHandler(this.generateMatrix_Click);
            // 
            // ReferenceMatrixModalWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 656);
            this.Controls.Add(this.generateMatrix);
            this.Controls.Add(this.referenceMatrixList);
            this.Controls.Add(this.referenceTableLabel);
            this.Controls.Add(this.referenceTableName);
            this.Controls.Add(this.saveReferenceTable);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.referenceMatrixInput);
            this.Name = "ReferenceMatrixModalWindow";
            this.Text = "Wybór macierzy referencyjnej";
            ((System.ComponentModel.ISupportInitialize)(this.referenceMatrixInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView referenceMatrixInput;
        private Button setButton;
        private Button saveReferenceTable;
        private TextBox referenceTableName;
        private Label referenceTableLabel;
        private ListView referenceMatrixList;
        private Button generateMatrix;
    }
}