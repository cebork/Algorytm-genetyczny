namespace Lab2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.bInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nInput = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.dInput = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.aInput = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.osobniki = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.pk = new System.Windows.Forms.NumericUpDown();
            this.pm = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osobniki)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pm)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "a = ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // bInput
            // 
            this.bInput.Location = new System.Drawing.Point(252, 23);
            this.bInput.Name = "bInput";
            this.bInput.Size = new System.Drawing.Size(120, 23);
            this.bInput.TabIndex = 4;
            this.bInput.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.bInput.ValueChanged += new System.EventHandler(this.bInput_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "b = ";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // nInput
            // 
            this.nInput.Location = new System.Drawing.Point(625, 23);
            this.nInput.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nInput.Name = "nInput";
            this.nInput.Size = new System.Drawing.Size(120, 23);
            this.nInput.TabIndex = 6;
            this.nInput.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(589, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "N = ";
            // 
            // dInput
            // 
            this.dInput.FormattingEnabled = true;
            this.dInput.Location = new System.Drawing.Point(436, 23);
            this.dInput.Name = "dInput";
            this.dInput.Size = new System.Drawing.Size(121, 23);
            this.dInput.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(402, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "d = ";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // aInput
            // 
            this.aInput.Location = new System.Drawing.Point(65, 23);
            this.aInput.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.aInput.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.aInput.Name = "aInput";
            this.aInput.Size = new System.Drawing.Size(120, 23);
            this.aInput.TabIndex = 9;
            this.aInput.Value = new decimal(new int[] {
            4,
            0,
            0,
            -2147483648});
            this.aInput.ValueChanged += new System.EventHandler(this.aInput_ValueChanged);
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.startButton.Location = new System.Drawing.Point(1546, 27);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 10;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // osobniki
            // 
            this.osobniki.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.osobniki.Location = new System.Drawing.Point(32, 90);
            this.osobniki.Name = "osobniki";
            this.osobniki.RowTemplate.Height = 25;
            this.osobniki.Size = new System.Drawing.Size(1512, 372);
            this.osobniki.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(799, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "pk=";
            // 
            // pk
            // 
            this.pk.DecimalPlaces = 2;
            this.pk.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.pk.Location = new System.Drawing.Point(833, 24);
            this.pk.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pk.Name = "pk";
            this.pk.Size = new System.Drawing.Size(120, 23);
            this.pk.TabIndex = 13;
            this.pk.Value = new decimal(new int[] {
            75,
            0,
            0,
            131072});
            // 
            // pm
            // 
            this.pm.DecimalPlaces = 3;
            this.pm.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.pm.Location = new System.Drawing.Point(1024, 23);
            this.pm.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pm.Name = "pm";
            this.pm.Size = new System.Drawing.Size(120, 23);
            this.pm.TabIndex = 15;
            this.pm.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(990, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "pm=";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 569);
            this.Controls.Add(this.pm);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.osobniki);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.aInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dInput);
            this.Controls.Add(this.nInput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Lab 2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osobniki)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label1;
        private NumericUpDown bInput;
        private Label label2;
        private NumericUpDown nInput;
        private Label label3;
        private ComboBox dInput;
        private Label label4;
        private NumericUpDown aInput;
        private Button startButton;
        private DataGridView osobniki;
        private Label label5;
        private NumericUpDown pk;
        private NumericUpDown pm;
        private Label label6;
    }
}