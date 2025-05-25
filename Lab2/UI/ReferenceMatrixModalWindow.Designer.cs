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
            this.referenceMatrixInput.Size = new System.Drawing.Size(1084, 565);
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
            // ReferenceMatrixModalWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 656);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.referenceMatrixInput);
            this.Name = "ReferenceMatrixModalWindow";
            this.Text = "Wybór macierzy referencyjnej";
            ((System.ComponentModel.ISupportInitialize)(this.referenceMatrixInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView referenceMatrixInput;
        private Button setButton;
    }
}