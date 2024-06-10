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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.testy = new System.Windows.Forms.DataGridView();
            this.testyStart = new System.Windows.Forms.Button();
            this.TInput = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osobniki)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pm)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TInput)).BeginInit();
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
            this.nInput.Minimum = new decimal(new int[] {
            1,
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
            this.osobniki.Location = new System.Drawing.Point(0, 0);
            this.osobniki.Name = "osobniki";
            this.osobniki.RowTemplate.Height = 25;
            this.osobniki.Size = new System.Drawing.Size(1422, 717);
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
            this.pm.DecimalPlaces = 4;
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
            // tabs
            // 
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Controls.Add(this.tabPage2);
            this.tabs.Controls.Add(this.tabPage3);
            this.tabs.Location = new System.Drawing.Point(83, 58);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1436, 748);
            this.tabs.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.osobniki);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1428, 720);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Podsumowanie";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1428, 720);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Wykresy";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(6, 6);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1419, 711);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.testy);
            this.tabPage3.Controls.Add(this.testyStart);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1428, 720);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Testy";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // testy
            // 
            this.testy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testy.Location = new System.Drawing.Point(3, 3);
            this.testy.Name = "testy";
            this.testy.RowTemplate.Height = 25;
            this.testy.Size = new System.Drawing.Size(1242, 615);
            this.testy.TabIndex = 20;
            // 
            // testyStart
            // 
            this.testyStart.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.testyStart.Location = new System.Drawing.Point(1304, 12);
            this.testyStart.Name = "testyStart";
            this.testyStart.Size = new System.Drawing.Size(102, 23);
            this.testyStart.TabIndex = 19;
            this.testyStart.Text = "START TESTY";
            this.testyStart.UseVisualStyleBackColor = false;
            this.testyStart.Click += new System.EventHandler(this.testyStart_Click);
            // 
            // TInput
            // 
            this.TInput.Location = new System.Drawing.Point(1199, 23);
            this.TInput.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TInput.Name = "TInput";
            this.TInput.Size = new System.Drawing.Size(120, 23);
            this.TInput.TabIndex = 18;
            this.TInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1165, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "T=";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 958);
            this.Controls.Add(this.TInput);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.pm);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pk);
            this.Controls.Add(this.label5);
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
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TInput)).EndInit();
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
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TabControl tabs;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private NumericUpDown TInput;
        private Label label7;
        private TabPage tabPage3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private DataGridView testy;
        private Button testyStart;
    }
}