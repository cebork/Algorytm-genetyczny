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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.bInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.dInput = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.aInput = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.osobniki = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bestMark = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.bestxReal = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.bestXBin = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.testMark = new System.Windows.Forms.Label();
            this.testMark2 = new System.Windows.Forms.Label();
            this.testTeta = new System.Windows.Forms.Label();
            this.testT = new System.Windows.Forms.Label();
            this.testTeta2 = new System.Windows.Forms.Label();
            this.testT2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.testy = new System.Windows.Forms.DataGridView();
            this.testyStart = new System.Windows.Forms.Button();
            this.tValue = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tetaValue = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osobniki)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tetaValue)).BeginInit();
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
            this.osobniki.Size = new System.Drawing.Size(470, 717);
            this.osobniki.TabIndex = 11;
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
            this.tabPage1.Controls.Add(this.bestMark);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.bestxReal);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.bestXBin);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.osobniki);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1428, 720);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Podsumowanie";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bestMark
            // 
            this.bestMark.AutoSize = true;
            this.bestMark.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bestMark.Location = new System.Drawing.Point(678, 156);
            this.bestMark.Name = "bestMark";
            this.bestMark.Size = new System.Drawing.Size(227, 28);
            this.bestMark.TabIndex = 17;
            this.bestMark.Text = "[wartość pojawi się tutaj]";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(563, 156);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 28);
            this.label10.TabIndex = 16;
            this.label10.Text = "best Mark =";
            // 
            // bestxReal
            // 
            this.bestxReal.AutoSize = true;
            this.bestxReal.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bestxReal.Location = new System.Drawing.Point(678, 105);
            this.bestxReal.Name = "bestxReal";
            this.bestxReal.Size = new System.Drawing.Size(227, 28);
            this.bestxReal.TabIndex = 15;
            this.bestxReal.Text = "[wartość pojawi się tutaj]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(563, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 28);
            this.label8.TabIndex = 14;
            this.label8.Text = "best xReal =";
            // 
            // bestXBin
            // 
            this.bestXBin.AutoSize = true;
            this.bestXBin.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bestXBin.Location = new System.Drawing.Point(678, 60);
            this.bestXBin.Name = "bestXBin";
            this.bestXBin.Size = new System.Drawing.Size(227, 28);
            this.bestXBin.TabIndex = 13;
            this.bestXBin.Text = "[wartość pojawi się tutaj]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(563, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 28);
            this.label6.TabIndex = 12;
            this.label6.Text = "best xBin =";
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
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(6, 6);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1419, 711);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.testMark);
            this.tabPage3.Controls.Add(this.testMark2);
            this.tabPage3.Controls.Add(this.testTeta);
            this.tabPage3.Controls.Add(this.testT);
            this.tabPage3.Controls.Add(this.testTeta2);
            this.tabPage3.Controls.Add(this.testT2);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.progressBar1);
            this.tabPage3.Controls.Add(this.testy);
            this.tabPage3.Controls.Add(this.testyStart);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1428, 720);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Testy";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // testMark
            // 
            this.testMark.AutoSize = true;
            this.testMark.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.testMark.Location = new System.Drawing.Point(709, 331);
            this.testMark.Name = "testMark";
            this.testMark.Size = new System.Drawing.Size(213, 28);
            this.testMark.TabIndex = 32;
            this.testMark.Text = "[Tutaj pojawi się wynik]";
            // 
            // testMark2
            // 
            this.testMark2.AutoSize = true;
            this.testMark2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.testMark2.Location = new System.Drawing.Point(546, 331);
            this.testMark2.Name = "testMark2";
            this.testMark2.Size = new System.Drawing.Size(170, 28);
            this.testMark2.TabIndex = 31;
            this.testMark2.Text = "Najlepsze Mark = ";
            // 
            // testTeta
            // 
            this.testTeta.AutoSize = true;
            this.testTeta.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.testTeta.Location = new System.Drawing.Point(709, 280);
            this.testTeta.Name = "testTeta";
            this.testTeta.Size = new System.Drawing.Size(213, 28);
            this.testTeta.TabIndex = 30;
            this.testTeta.Text = "[Tutaj pojawi się wynik]";
            // 
            // testT
            // 
            this.testT.AutoSize = true;
            this.testT.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.testT.Location = new System.Drawing.Point(682, 229);
            this.testT.Name = "testT";
            this.testT.Size = new System.Drawing.Size(213, 28);
            this.testT.TabIndex = 29;
            this.testT.Text = "[Tutaj pojawi się wynik]";
            // 
            // testTeta2
            // 
            this.testTeta2.AutoSize = true;
            this.testTeta2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.testTeta2.Location = new System.Drawing.Point(546, 280);
            this.testTeta2.Name = "testTeta2";
            this.testTeta2.Size = new System.Drawing.Size(159, 28);
            this.testTeta2.TabIndex = 28;
            this.testTeta2.Text = "Najlepsze teta = ";
            // 
            // testT2
            // 
            this.testT2.AutoSize = true;
            this.testT2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.testT2.Location = new System.Drawing.Point(546, 229);
            this.testT2.Name = "testT2";
            this.testT2.Size = new System.Drawing.Size(130, 28);
            this.testT2.TabIndex = 27;
            this.testT2.Text = "Najlepsze T =";
            this.testT2.Click += new System.EventHandler(this.testT_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(546, 183);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 28);
            this.label9.TabIndex = 26;
            this.label9.Text = "Stałe d = 0.001";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(546, 132);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(117, 28);
            this.label12.TabIndex = 24;
            this.label12.Text = "Stałe b = 12";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(546, 87);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(112, 28);
            this.label14.TabIndex = 22;
            this.label14.Text = "Stałe a = -4";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(546, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(672, 46);
            this.progressBar1.TabIndex = 21;
            // 
            // testy
            // 
            this.testy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testy.Location = new System.Drawing.Point(3, 3);
            this.testy.Name = "testy";
            this.testy.RowTemplate.Height = 25;
            this.testy.Size = new System.Drawing.Size(374, 615);
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
            // tValue
            // 
            this.tValue.Location = new System.Drawing.Point(608, 23);
            this.tValue.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.tValue.Name = "tValue";
            this.tValue.Size = new System.Drawing.Size(120, 23);
            this.tValue.TabIndex = 18;
            this.tValue.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(574, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "T = ";
            // 
            // tetaValue
            // 
            this.tetaValue.DecimalPlaces = 3;
            this.tetaValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.tetaValue.Location = new System.Drawing.Point(796, 23);
            this.tetaValue.Name = "tetaValue";
            this.tetaValue.Size = new System.Drawing.Size(120, 23);
            this.tetaValue.TabIndex = 20;
            this.tetaValue.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(749, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "teta = ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 958);
            this.Controls.Add(this.tetaValue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.aInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dInput);
            this.Controls.Add(this.bInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Lab 2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osobniki)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tetaValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label1;
        private NumericUpDown bInput;
        private Label label2;
        private ComboBox dInput;
        private Label label4;
        private NumericUpDown aInput;
        private Button startButton;
        private DataGridView osobniki;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TabControl tabs;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private DataGridView testy;
        private Button testyStart;
        private NumericUpDown tValue;
        private Label label3;
        private NumericUpDown tetaValue;
        private Label label5;
        private Label bestXBin;
        private Label label6;
        private Label bestMark;
        private Label label10;
        private Label bestxReal;
        private Label label8;
        private ProgressBar progressBar1;
        private Label testMark;
        private Label testMark2;
        private Label testTeta;
        private Label testT;
        private Label testTeta2;
        private Label testT2;
        private Label label9;
        private Label label12;
        private Label label14;
    }
}