namespace Lab2.UI
{
    partial class PatternPreviewModalWindow
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
            this.patternPreview = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.patternPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // patternPreview
            // 
            this.patternPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.patternPreview.Location = new System.Drawing.Point(12, 12);
            this.patternPreview.Name = "patternPreview";
            this.patternPreview.RowTemplate.Height = 25;
            this.patternPreview.Size = new System.Drawing.Size(424, 385);
            this.patternPreview.TabIndex = 0;
            this.patternPreview.CellFormatting += patternPreview_CellFormatting;
            // 
            // PatternPreviewModalWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 409);
            this.Controls.Add(this.patternPreview);
            this.Name = "PatternPreviewModalWindow";
            this.Text = "Podgląd wzorca";
            ((System.ComponentModel.ISupportInitialize)(this.patternPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView patternPreview;
    }
}