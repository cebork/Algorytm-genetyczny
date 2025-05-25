namespace Lab2
{
    partial class MainWindow
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
            this.matrixSizeLabel = new System.Windows.Forms.Label();
            this.matrixSizeInput = new System.Windows.Forms.NumericUpDown();
            this.individualNumberInput = new System.Windows.Forms.NumericUpDown();
            this.individualNumberLabel = new System.Windows.Forms.Label();
            this.precisionInput = new System.Windows.Forms.ComboBox();
            this.precisionLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.corssProbabilityLabel = new System.Windows.Forms.Label();
            this.crossProbabilityInput = new System.Windows.Forms.NumericUpDown();
            this.mutationProbabilityInput = new System.Windows.Forms.NumericUpDown();
            this.mutationProbabilityLabel = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.osobniki = new System.Windows.Forms.DataGridView();
            this.display = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.testy = new System.Windows.Forms.DataGridView();
            this.testyStart = new System.Windows.Forms.Button();
            this.iterationNumberInput = new System.Windows.Forms.NumericUpDown();
            this.iterationNumberLabel = new System.Windows.Forms.Label();
            this.additioanlDataButton = new System.Windows.Forms.Button();
            this.algorithmTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.unsupervisedTypeRadioButton = new System.Windows.Forms.RadioButton();
            this.supervisedTypedRadioButton = new System.Windows.Forms.RadioButton();
            this.inputDataGroupBox = new System.Windows.Forms.GroupBox();
            this.runProgressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.matrixSizeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.individualNumberInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.crossProbabilityInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutationProbabilityInput)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.osobniki)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.display)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationNumberInput)).BeginInit();
            this.algorithmTypeGroupBox.SuspendLayout();
            this.inputDataGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // matrixSizeLabel
            // 
            this.matrixSizeLabel.AutoSize = true;
            this.matrixSizeLabel.Location = new System.Drawing.Point(7, 33);
            this.matrixSizeLabel.Name = "matrixSizeLabel";
            this.matrixSizeLabel.Size = new System.Drawing.Size(121, 15);
            this.matrixSizeLabel.TabIndex = 1;
            this.matrixSizeLabel.Text = "Rozmiar macierzy (n) ";
            // 
            // matrixSizeInput
            // 
            this.matrixSizeInput.Location = new System.Drawing.Point(227, 29);
            this.matrixSizeInput.Name = "matrixSizeInput";
            this.matrixSizeInput.Size = new System.Drawing.Size(120, 23);
            this.matrixSizeInput.TabIndex = 4;
            this.matrixSizeInput.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.matrixSizeInput.ValueChanged += new System.EventHandler(this.matrixSizeInput_ValueChanged);
            // 
            // individualNumberInput
            // 
            this.individualNumberInput.Location = new System.Drawing.Point(491, 29);
            this.individualNumberInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.individualNumberInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.individualNumberInput.Name = "individualNumberInput";
            this.individualNumberInput.Size = new System.Drawing.Size(120, 23);
            this.individualNumberInput.TabIndex = 6;
            this.individualNumberInput.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // individualNumberLabel
            // 
            this.individualNumberLabel.AutoSize = true;
            this.individualNumberLabel.Location = new System.Drawing.Point(373, 33);
            this.individualNumberLabel.Name = "individualNumberLabel";
            this.individualNumberLabel.Size = new System.Drawing.Size(112, 15);
            this.individualNumberLabel.TabIndex = 5;
            this.individualNumberLabel.Text = "Ilość osobników (N)";
            // 
            // precisionInput
            // 
            this.precisionInput.FormattingEnabled = true;
            this.precisionInput.Location = new System.Drawing.Point(491, 87);
            this.precisionInput.Name = "precisionInput";
            this.precisionInput.Size = new System.Drawing.Size(121, 23);
            this.precisionInput.TabIndex = 7;
            // 
            // precisionLabel
            // 
            this.precisionLabel.AutoSize = true;
            this.precisionLabel.Location = new System.Drawing.Point(373, 91);
            this.precisionLabel.Name = "precisionLabel";
            this.precisionLabel.Size = new System.Drawing.Size(87, 15);
            this.precisionLabel.TabIndex = 8;
            this.precisionLabel.Text = "Dokładność (d)";
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.startButton.Location = new System.Drawing.Point(971, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(210, 55);
            this.startButton.TabIndex = 10;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // corssProbabilityLabel
            // 
            this.corssProbabilityLabel.AutoSize = true;
            this.corssProbabilityLabel.Location = new System.Drawing.Point(7, 62);
            this.corssProbabilityLabel.Name = "corssProbabilityLabel";
            this.corssProbabilityLabel.Size = new System.Drawing.Size(214, 15);
            this.corssProbabilityLabel.TabIndex = 12;
            this.corssProbabilityLabel.Text = "Prawdopodobieństwo krzyżowania (pk)";
            // 
            // crossProbabilityInput
            // 
            this.crossProbabilityInput.DecimalPlaces = 2;
            this.crossProbabilityInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.crossProbabilityInput.Location = new System.Drawing.Point(227, 58);
            this.crossProbabilityInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.crossProbabilityInput.Name = "crossProbabilityInput";
            this.crossProbabilityInput.Size = new System.Drawing.Size(120, 23);
            this.crossProbabilityInput.TabIndex = 13;
            this.crossProbabilityInput.Value = new decimal(new int[] {
            75,
            0,
            0,
            131072});
            // 
            // mutationProbabilityInput
            // 
            this.mutationProbabilityInput.DecimalPlaces = 4;
            this.mutationProbabilityInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.mutationProbabilityInput.Location = new System.Drawing.Point(227, 87);
            this.mutationProbabilityInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mutationProbabilityInput.Name = "mutationProbabilityInput";
            this.mutationProbabilityInput.Size = new System.Drawing.Size(120, 23);
            this.mutationProbabilityInput.TabIndex = 15;
            this.mutationProbabilityInput.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            // 
            // mutationProbabilityLabel
            // 
            this.mutationProbabilityLabel.AutoSize = true;
            this.mutationProbabilityLabel.Location = new System.Drawing.Point(7, 91);
            this.mutationProbabilityLabel.Name = "mutationProbabilityLabel";
            this.mutationProbabilityLabel.Size = new System.Drawing.Size(195, 15);
            this.mutationProbabilityLabel.TabIndex = 14;
            this.mutationProbabilityLabel.Text = "Prawdopodobieństwo mutacji (pm)";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Controls.Add(this.tabPage2);
            this.tabs.Controls.Add(this.tabPage3);
            this.tabs.Location = new System.Drawing.Point(51, 230);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1400, 848);
            this.tabs.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.osobniki);
            this.tabPage1.Controls.Add(this.display);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1392, 820);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Podsumowanie";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // osobniki
            // 
            this.osobniki.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.osobniki.Location = new System.Drawing.Point(3, 3);
            this.osobniki.Name = "osobniki";
            this.osobniki.RowTemplate.Height = 25;
            this.osobniki.Size = new System.Drawing.Size(486, 811);
            this.osobniki.TabIndex = 11;
            // 
            // display
            // 
            this.display.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.display.Location = new System.Drawing.Point(495, 3);
            this.display.Name = "display";
            this.display.RowTemplate.Height = 25;
            this.display.Size = new System.Drawing.Size(891, 811);
            this.display.TabIndex = 19;
            this.display.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1392, 820);
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
            this.chart1.Size = new System.Drawing.Size(1102, 594);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.testy);
            this.tabPage3.Controls.Add(this.testyStart);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1392, 820);
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
            this.testy.Size = new System.Drawing.Size(1120, 600);
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
            // iterationNumberInput
            // 
            this.iterationNumberInput.Location = new System.Drawing.Point(491, 58);
            this.iterationNumberInput.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.iterationNumberInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.iterationNumberInput.Name = "iterationNumberInput";
            this.iterationNumberInput.Size = new System.Drawing.Size(120, 23);
            this.iterationNumberInput.TabIndex = 18;
            this.iterationNumberInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // iterationNumberLabel
            // 
            this.iterationNumberLabel.AutoSize = true;
            this.iterationNumberLabel.Location = new System.Drawing.Point(373, 62);
            this.iterationNumberLabel.Name = "iterationNumberLabel";
            this.iterationNumberLabel.Size = new System.Drawing.Size(86, 15);
            this.iterationNumberLabel.TabIndex = 17;
            this.iterationNumberLabel.Text = "Ilość iteracji (T)";
            // 
            // additioanlDataButton
            // 
            this.additioanlDataButton.Location = new System.Drawing.Point(636, 29);
            this.additioanlDataButton.Name = "additioanlDataButton";
            this.additioanlDataButton.Size = new System.Drawing.Size(93, 81);
            this.additioanlDataButton.TabIndex = 20;
            this.additioanlDataButton.Text = "Wybór macierzy referencyjnej";
            this.additioanlDataButton.UseVisualStyleBackColor = true;
            this.additioanlDataButton.Click += new System.EventHandler(this.additioanlDataButton_Click);
            // 
            // algorithmTypeGroupBox
            // 
            this.algorithmTypeGroupBox.Controls.Add(this.unsupervisedTypeRadioButton);
            this.algorithmTypeGroupBox.Controls.Add(this.supervisedTypedRadioButton);
            this.algorithmTypeGroupBox.Location = new System.Drawing.Point(51, 12);
            this.algorithmTypeGroupBox.Name = "algorithmTypeGroupBox";
            this.algorithmTypeGroupBox.Size = new System.Drawing.Size(237, 48);
            this.algorithmTypeGroupBox.TabIndex = 22;
            this.algorithmTypeGroupBox.TabStop = false;
            this.algorithmTypeGroupBox.Text = "Typ algorytmu";
            // 
            // unsupervisedTypeRadioButton
            // 
            this.unsupervisedTypeRadioButton.AutoSize = true;
            this.unsupervisedTypeRadioButton.Location = new System.Drawing.Point(113, 19);
            this.unsupervisedTypeRadioButton.Name = "unsupervisedTypeRadioButton";
            this.unsupervisedTypeRadioButton.Size = new System.Drawing.Size(112, 19);
            this.unsupervisedTypeRadioButton.TabIndex = 1;
            this.unsupervisedTypeRadioButton.TabStop = true;
            this.unsupervisedTypeRadioButton.Text = "nienadzorowany";
            this.unsupervisedTypeRadioButton.UseVisualStyleBackColor = true;
            this.unsupervisedTypeRadioButton.CheckedChanged += new System.EventHandler(this.unsupervisedTypeRadioButton_CheckedChanged);
            // 
            // supervisedTypedRadioButton
            // 
            this.supervisedTypedRadioButton.AutoSize = true;
            this.supervisedTypedRadioButton.Location = new System.Drawing.Point(11, 19);
            this.supervisedTypedRadioButton.Name = "supervisedTypedRadioButton";
            this.supervisedTypedRadioButton.Size = new System.Drawing.Size(96, 19);
            this.supervisedTypedRadioButton.TabIndex = 0;
            this.supervisedTypedRadioButton.TabStop = true;
            this.supervisedTypedRadioButton.Text = "nazdorowany";
            this.supervisedTypedRadioButton.UseVisualStyleBackColor = true;
            this.supervisedTypedRadioButton.CheckedChanged += new System.EventHandler(this.supervisedTypedRadioButton_CheckedChanged);
            // 
            // inputDataGroupBox
            // 
            this.inputDataGroupBox.Controls.Add(this.matrixSizeLabel);
            this.inputDataGroupBox.Controls.Add(this.matrixSizeInput);
            this.inputDataGroupBox.Controls.Add(this.additioanlDataButton);
            this.inputDataGroupBox.Controls.Add(this.individualNumberLabel);
            this.inputDataGroupBox.Controls.Add(this.individualNumberInput);
            this.inputDataGroupBox.Controls.Add(this.iterationNumberInput);
            this.inputDataGroupBox.Controls.Add(this.precisionInput);
            this.inputDataGroupBox.Controls.Add(this.iterationNumberLabel);
            this.inputDataGroupBox.Controls.Add(this.precisionLabel);
            this.inputDataGroupBox.Controls.Add(this.corssProbabilityLabel);
            this.inputDataGroupBox.Controls.Add(this.mutationProbabilityInput);
            this.inputDataGroupBox.Controls.Add(this.crossProbabilityInput);
            this.inputDataGroupBox.Controls.Add(this.mutationProbabilityLabel);
            this.inputDataGroupBox.Location = new System.Drawing.Point(51, 79);
            this.inputDataGroupBox.Name = "inputDataGroupBox";
            this.inputDataGroupBox.Size = new System.Drawing.Size(741, 126);
            this.inputDataGroupBox.TabIndex = 23;
            this.inputDataGroupBox.TabStop = false;
            this.inputDataGroupBox.Text = "Dane wejściowe";
            // 
            // runProgressBar
            // 
            this.runProgressBar.Location = new System.Drawing.Point(971, 79);
            this.runProgressBar.Name = "runProgressBar";
            this.runProgressBar.Size = new System.Drawing.Size(210, 23);
            this.runProgressBar.TabIndex = 24;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1464, 1094);
            this.Controls.Add(this.runProgressBar);
            this.Controls.Add(this.inputDataGroupBox);
            this.Controls.Add(this.algorithmTypeGroupBox);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.startButton);
            this.Name = "MainWindow";
            this.Text = "Lab 2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.matrixSizeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.individualNumberInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.crossProbabilityInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutationProbabilityInput)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.osobniki)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.display)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationNumberInput)).EndInit();
            this.algorithmTypeGroupBox.ResumeLayout(false);
            this.algorithmTypeGroupBox.PerformLayout();
            this.inputDataGroupBox.ResumeLayout(false);
            this.inputDataGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Label matrixSizeLabel;
        private NumericUpDown matrixSizeInput;
        private NumericUpDown individualNumberInput;
        private Label individualNumberLabel;
        private ComboBox precisionInput;
        private Label precisionLabel;
        private Button startButton;
        private Label corssProbabilityLabel;
        private NumericUpDown crossProbabilityInput;
        private NumericUpDown mutationProbabilityInput;
        private Label mutationProbabilityLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TabControl tabs;
        private TabPage tabPage2;
        private NumericUpDown iterationNumberInput;
        private Label iterationNumberLabel;
        private TabPage tabPage3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private DataGridView testy;
        private Button testyStart;
        private Button additioanlDataButton;
        private GroupBox algorithmTypeGroupBox;
        private RadioButton unsupervisedTypeRadioButton;
        private RadioButton supervisedTypedRadioButton;
        private GroupBox inputDataGroupBox;
        private TabPage tabPage1;
        private DataGridView osobniki;
        private DataGridView display;
        private ProgressBar runProgressBar;
    }
}