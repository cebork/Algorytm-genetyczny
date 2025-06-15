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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TstepILabel = new System.Windows.Forms.Label();
            this.TstepInput = new System.Windows.Forms.NumericUpDown();
            this.TbLabel = new System.Windows.Forms.Label();
            this.TbInput = new System.Windows.Forms.NumericUpDown();
            this.TaLabel = new System.Windows.Forms.Label();
            this.TaInput = new System.Windows.Forms.NumericUpDown();
            this.pmgroupbox = new System.Windows.Forms.GroupBox();
            this.pmbsizeLabel = new System.Windows.Forms.Label();
            this.PmstepInput = new System.Windows.Forms.NumericUpDown();
            this.pmblabel = new System.Windows.Forms.Label();
            this.PmbInput = new System.Windows.Forms.NumericUpDown();
            this.PmaLabel = new System.Windows.Forms.Label();
            this.pmaInput = new System.Windows.Forms.NumericUpDown();
            this.pkgroupbox = new System.Windows.Forms.GroupBox();
            this.PkSizeLabel = new System.Windows.Forms.Label();
            this.PkstepInput = new System.Windows.Forms.NumericUpDown();
            this.pkbLabel = new System.Windows.Forms.Label();
            this.PkbbInput = new System.Windows.Forms.NumericUpDown();
            this.pka = new System.Windows.Forms.Label();
            this.pkaInput = new System.Windows.Forms.NumericUpDown();
            this.individiualsCountGroupBox = new System.Windows.Forms.GroupBox();
            this.NstepLabel = new System.Windows.Forms.Label();
            this.NstepInput = new System.Windows.Forms.NumericUpDown();
            this.NbLabel = new System.Windows.Forms.Label();
            this.NbInput = new System.Windows.Forms.NumericUpDown();
            this.NaLabel = new System.Windows.Forms.Label();
            this.NaInput = new System.Windows.Forms.NumericUpDown();
            this.testExperimentCountLabel = new System.Windows.Forms.Label();
            this.testExperimentCount = new System.Windows.Forms.NumericUpDown();
            this.testyStart = new System.Windows.Forms.Button();
            this.iterationNumberInput = new System.Windows.Forms.NumericUpDown();
            this.iterationNumberLabel = new System.Windows.Forms.Label();
            this.additioanlDataButton = new System.Windows.Forms.Button();
            this.algorithmTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.unsupervisedTypeRadioButton = new System.Windows.Forms.RadioButton();
            this.supervisedTypedRadioButton = new System.Windows.Forms.RadioButton();
            this.inputDataGroupBox = new System.Windows.Forms.GroupBox();
            this.seed = new System.Windows.Forms.TextBox();
            this.useSeed = new System.Windows.Forms.CheckBox();
            this.experimentNumberLabel = new System.Windows.Forms.Label();
            this.experimentNumber = new System.Windows.Forms.NumericUpDown();
            this.runProgressBar = new System.Windows.Forms.ProgressBar();
            this.GAmodification = new System.Windows.Forms.GroupBox();
            this.modifiedGARadio = new System.Windows.Forms.RadioButton();
            this.classicalGARadio = new System.Windows.Forms.RadioButton();
            this.selectionGroup = new System.Windows.Forms.GroupBox();
            this.tournamentRadio = new System.Windows.Forms.RadioButton();
            this.ruletteRadio = new System.Windows.Forms.RadioButton();
            this.crossGroup = new System.Windows.Forms.GroupBox();
            this.crossPoints = new System.Windows.Forms.NumericUpDown();
            this.multiPointRadio = new System.Windows.Forms.RadioButton();
            this.singlePointRadio = new System.Windows.Forms.RadioButton();
            this.mutationGroup = new System.Windows.Forms.GroupBox();
            this.bitSwapingRadio = new System.Windows.Forms.RadioButton();
            this.evenlyRadio = new System.Windows.Forms.RadioButton();
            this.probGen1Group = new System.Windows.Forms.GroupBox();
            this.propGen1 = new System.Windows.Forms.NumericUpDown();
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
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TstepInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaInput)).BeginInit();
            this.pmgroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PmstepInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PmbInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmaInput)).BeginInit();
            this.pkgroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PkstepInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PkbbInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pkaInput)).BeginInit();
            this.individiualsCountGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NstepInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NbInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NaInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testExperimentCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationNumberInput)).BeginInit();
            this.algorithmTypeGroupBox.SuspendLayout();
            this.inputDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.experimentNumber)).BeginInit();
            this.GAmodification.SuspendLayout();
            this.selectionGroup.SuspendLayout();
            this.crossGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.crossPoints)).BeginInit();
            this.mutationGroup.SuspendLayout();
            this.probGen1Group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propGen1)).BeginInit();
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
            this.matrixSizeInput.Size = new System.Drawing.Size(70, 23);
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
            this.individualNumberInput.Location = new System.Drawing.Point(435, 29);
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
            this.individualNumberInput.Size = new System.Drawing.Size(58, 23);
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
            this.individualNumberLabel.Location = new System.Drawing.Point(317, 33);
            this.individualNumberLabel.Name = "individualNumberLabel";
            this.individualNumberLabel.Size = new System.Drawing.Size(112, 15);
            this.individualNumberLabel.TabIndex = 5;
            this.individualNumberLabel.Text = "Ilość osobników (N)";
            // 
            // precisionInput
            // 
            this.precisionInput.FormattingEnabled = true;
            this.precisionInput.Location = new System.Drawing.Point(435, 87);
            this.precisionInput.Name = "precisionInput";
            this.precisionInput.Size = new System.Drawing.Size(58, 23);
            this.precisionInput.TabIndex = 7;
            // 
            // precisionLabel
            // 
            this.precisionLabel.AutoSize = true;
            this.precisionLabel.Location = new System.Drawing.Point(317, 91);
            this.precisionLabel.Name = "precisionLabel";
            this.precisionLabel.Size = new System.Drawing.Size(87, 15);
            this.precisionLabel.TabIndex = 8;
            this.precisionLabel.Text = "Dokładność (d)";
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.startButton.Location = new System.Drawing.Point(991, 12);
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
            this.crossProbabilityInput.Size = new System.Drawing.Size(70, 23);
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
            this.mutationProbabilityInput.Size = new System.Drawing.Size(70, 23);
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
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.pmgroupbox);
            this.tabPage3.Controls.Add(this.pkgroupbox);
            this.tabPage3.Controls.Add(this.individiualsCountGroupBox);
            this.tabPage3.Controls.Add(this.testExperimentCountLabel);
            this.tabPage3.Controls.Add(this.testExperimentCount);
            this.tabPage3.Controls.Add(this.testyStart);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1392, 820);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Testy";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TstepILabel);
            this.groupBox1.Controls.Add(this.TstepInput);
            this.groupBox1.Controls.Add(this.TbLabel);
            this.groupBox1.Controls.Add(this.TbInput);
            this.groupBox1.Controls.Add(this.TaLabel);
            this.groupBox1.Controls.Add(this.TaInput);
            this.groupBox1.Location = new System.Drawing.Point(320, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 64);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zakresy iteracji (T)";
            // 
            // TstepILabel
            // 
            this.TstepILabel.AutoSize = true;
            this.TstepILabel.Location = new System.Drawing.Point(349, 24);
            this.TstepILabel.Name = "TstepILabel";
            this.TstepILabel.Size = new System.Drawing.Size(41, 15);
            this.TstepILabel.TabIndex = 28;
            this.TstepILabel.Text = "T_step";
            // 
            // TstepInput
            // 
            this.TstepInput.AccessibleDescription = "PmstepInput";
            this.TstepInput.Location = new System.Drawing.Point(402, 22);
            this.TstepInput.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.TstepInput.Name = "TstepInput";
            this.TstepInput.Size = new System.Drawing.Size(120, 23);
            this.TstepInput.TabIndex = 27;
            // 
            // TbLabel
            // 
            this.TbLabel.AutoSize = true;
            this.TbLabel.Location = new System.Drawing.Point(174, 26);
            this.TbLabel.Name = "TbLabel";
            this.TbLabel.Size = new System.Drawing.Size(26, 15);
            this.TbLabel.TabIndex = 26;
            this.TbLabel.Text = "T_b";
            // 
            // TbInput
            // 
            this.TbInput.AccessibleDescription = "TbInput";
            this.TbInput.Location = new System.Drawing.Point(224, 22);
            this.TbInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.TbInput.Name = "TbInput";
            this.TbInput.Size = new System.Drawing.Size(120, 23);
            this.TbInput.TabIndex = 25;
            // 
            // TaLabel
            // 
            this.TaLabel.AutoSize = true;
            this.TaLabel.Location = new System.Drawing.Point(7, 26);
            this.TaLabel.Name = "TaLabel";
            this.TaLabel.Size = new System.Drawing.Size(25, 15);
            this.TaLabel.TabIndex = 24;
            this.TaLabel.Text = "T_a";
            // 
            // TaInput
            // 
            this.TaInput.AccessibleDescription = "TaInput";
            this.TaInput.Location = new System.Drawing.Point(48, 22);
            this.TaInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.TaInput.Name = "TaInput";
            this.TaInput.Size = new System.Drawing.Size(120, 23);
            this.TaInput.TabIndex = 23;
            // 
            // pmgroupbox
            // 
            this.pmgroupbox.Controls.Add(this.pmbsizeLabel);
            this.pmgroupbox.Controls.Add(this.PmstepInput);
            this.pmgroupbox.Controls.Add(this.pmblabel);
            this.pmgroupbox.Controls.Add(this.PmbInput);
            this.pmgroupbox.Controls.Add(this.PmaLabel);
            this.pmgroupbox.Controls.Add(this.pmaInput);
            this.pmgroupbox.Location = new System.Drawing.Point(320, 159);
            this.pmgroupbox.Name = "pmgroupbox";
            this.pmgroupbox.Size = new System.Drawing.Size(528, 64);
            this.pmgroupbox.TabIndex = 30;
            this.pmgroupbox.TabStop = false;
            this.pmgroupbox.Text = "Zakresy prawdopodobieństwa mutacji";
            // 
            // pmbsizeLabel
            // 
            this.pmbsizeLabel.AutoSize = true;
            this.pmbsizeLabel.Location = new System.Drawing.Point(349, 24);
            this.pmbsizeLabel.Name = "pmbsizeLabel";
            this.pmbsizeLabel.Size = new System.Drawing.Size(52, 15);
            this.pmbsizeLabel.TabIndex = 28;
            this.pmbsizeLabel.Text = "Pm_step";
            // 
            // PmstepInput
            // 
            this.PmstepInput.AccessibleDescription = "PmstepInput";
            this.PmstepInput.DecimalPlaces = 4;
            this.PmstepInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.PmstepInput.Location = new System.Drawing.Point(402, 22);
            this.PmstepInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PmstepInput.Name = "PmstepInput";
            this.PmstepInput.Size = new System.Drawing.Size(120, 23);
            this.PmstepInput.TabIndex = 27;
            // 
            // pmblabel
            // 
            this.pmblabel.AutoSize = true;
            this.pmblabel.Location = new System.Drawing.Point(174, 26);
            this.pmblabel.Name = "pmblabel";
            this.pmblabel.Size = new System.Drawing.Size(44, 15);
            this.pmblabel.TabIndex = 26;
            this.pmblabel.Text = "Pmb_b";
            // 
            // PmbInput
            // 
            this.PmbInput.AccessibleDescription = "PmbInput";
            this.PmbInput.DecimalPlaces = 4;
            this.PmbInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.PmbInput.Location = new System.Drawing.Point(224, 22);
            this.PmbInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PmbInput.Name = "PmbInput";
            this.PmbInput.Size = new System.Drawing.Size(120, 23);
            this.PmbInput.TabIndex = 25;
            // 
            // PmaLabel
            // 
            this.PmaLabel.AutoSize = true;
            this.PmaLabel.Location = new System.Drawing.Point(7, 26);
            this.PmaLabel.Name = "PmaLabel";
            this.PmaLabel.Size = new System.Drawing.Size(36, 15);
            this.PmaLabel.TabIndex = 24;
            this.PmaLabel.Text = "Pm_a";
            // 
            // pmaInput
            // 
            this.pmaInput.AccessibleDescription = "pmaInput";
            this.pmaInput.DecimalPlaces = 4;
            this.pmaInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.pmaInput.Location = new System.Drawing.Point(48, 22);
            this.pmaInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pmaInput.Name = "pmaInput";
            this.pmaInput.Size = new System.Drawing.Size(120, 23);
            this.pmaInput.TabIndex = 23;
            // 
            // pkgroupbox
            // 
            this.pkgroupbox.Controls.Add(this.PkSizeLabel);
            this.pkgroupbox.Controls.Add(this.PkstepInput);
            this.pkgroupbox.Controls.Add(this.pkbLabel);
            this.pkgroupbox.Controls.Add(this.PkbbInput);
            this.pkgroupbox.Controls.Add(this.pka);
            this.pkgroupbox.Controls.Add(this.pkaInput);
            this.pkgroupbox.Location = new System.Drawing.Point(320, 89);
            this.pkgroupbox.Name = "pkgroupbox";
            this.pkgroupbox.Size = new System.Drawing.Size(528, 64);
            this.pkgroupbox.TabIndex = 29;
            this.pkgroupbox.TabStop = false;
            this.pkgroupbox.Text = "Zakresy prawdopodobieństwa krzyżowania";
            // 
            // PkSizeLabel
            // 
            this.PkSizeLabel.AutoSize = true;
            this.PkSizeLabel.Location = new System.Drawing.Point(352, 26);
            this.PkSizeLabel.Name = "PkSizeLabel";
            this.PkSizeLabel.Size = new System.Drawing.Size(47, 15);
            this.PkSizeLabel.TabIndex = 28;
            this.PkSizeLabel.Text = "Pk_step";
            // 
            // PkstepInput
            // 
            this.PkstepInput.AccessibleDescription = "PkstepInput";
            this.PkstepInput.DecimalPlaces = 2;
            this.PkstepInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PkstepInput.Location = new System.Drawing.Point(402, 22);
            this.PkstepInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PkstepInput.Name = "PkstepInput";
            this.PkstepInput.Size = new System.Drawing.Size(120, 23);
            this.PkstepInput.TabIndex = 27;
            // 
            // pkbLabel
            // 
            this.pkbLabel.AutoSize = true;
            this.pkbLabel.Location = new System.Drawing.Point(177, 26);
            this.pkbLabel.Name = "pkbLabel";
            this.pkbLabel.Size = new System.Drawing.Size(39, 15);
            this.pkbLabel.TabIndex = 26;
            this.pkbLabel.Text = "Pkb_b";
            // 
            // PkbbInput
            // 
            this.PkbbInput.AccessibleDescription = "PkbbInput";
            this.PkbbInput.DecimalPlaces = 2;
            this.PkbbInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PkbbInput.Location = new System.Drawing.Point(224, 22);
            this.PkbbInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PkbbInput.Name = "PkbbInput";
            this.PkbbInput.Size = new System.Drawing.Size(120, 23);
            this.PkbbInput.TabIndex = 25;
            // 
            // pka
            // 
            this.pka.AutoSize = true;
            this.pka.Location = new System.Drawing.Point(10, 26);
            this.pka.Name = "pka";
            this.pka.Size = new System.Drawing.Size(31, 15);
            this.pka.TabIndex = 24;
            this.pka.Text = "Pk_a";
            // 
            // pkaInput
            // 
            this.pkaInput.AccessibleDescription = "pkaInput";
            this.pkaInput.DecimalPlaces = 2;
            this.pkaInput.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.pkaInput.Location = new System.Drawing.Point(48, 22);
            this.pkaInput.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pkaInput.Name = "pkaInput";
            this.pkaInput.Size = new System.Drawing.Size(120, 23);
            this.pkaInput.TabIndex = 23;
            // 
            // individiualsCountGroupBox
            // 
            this.individiualsCountGroupBox.Controls.Add(this.NstepLabel);
            this.individiualsCountGroupBox.Controls.Add(this.NstepInput);
            this.individiualsCountGroupBox.Controls.Add(this.NbLabel);
            this.individiualsCountGroupBox.Controls.Add(this.NbInput);
            this.individiualsCountGroupBox.Controls.Add(this.NaLabel);
            this.individiualsCountGroupBox.Controls.Add(this.NaInput);
            this.individiualsCountGroupBox.Location = new System.Drawing.Point(320, 19);
            this.individiualsCountGroupBox.Name = "individiualsCountGroupBox";
            this.individiualsCountGroupBox.Size = new System.Drawing.Size(528, 64);
            this.individiualsCountGroupBox.TabIndex = 22;
            this.individiualsCountGroupBox.TabStop = false;
            this.individiualsCountGroupBox.Text = "Zakresy ilości osobników";
            // 
            // NstepLabel
            // 
            this.NstepLabel.AutoSize = true;
            this.NstepLabel.Location = new System.Drawing.Point(354, 26);
            this.NstepLabel.Name = "NstepLabel";
            this.NstepLabel.Size = new System.Drawing.Size(43, 15);
            this.NstepLabel.TabIndex = 28;
            this.NstepLabel.Text = "N_step";
            // 
            // NstepInput
            // 
            this.NstepInput.AccessibleDescription = "NstepInput";
            this.NstepInput.Location = new System.Drawing.Point(402, 22);
            this.NstepInput.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NstepInput.Name = "NstepInput";
            this.NstepInput.Size = new System.Drawing.Size(120, 23);
            this.NstepInput.TabIndex = 27;
            // 
            // NbLabel
            // 
            this.NbLabel.AutoSize = true;
            this.NbLabel.Location = new System.Drawing.Point(182, 26);
            this.NbLabel.Name = "NbLabel";
            this.NbLabel.Size = new System.Drawing.Size(28, 15);
            this.NbLabel.TabIndex = 26;
            this.NbLabel.Text = "N_b";
            // 
            // NbInput
            // 
            this.NbInput.AccessibleDescription = "NbInput";
            this.NbInput.Location = new System.Drawing.Point(224, 22);
            this.NbInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.NbInput.Name = "NbInput";
            this.NbInput.Size = new System.Drawing.Size(120, 23);
            this.NbInput.TabIndex = 25;
            // 
            // NaLabel
            // 
            this.NaLabel.AutoSize = true;
            this.NaLabel.Location = new System.Drawing.Point(12, 26);
            this.NaLabel.Name = "NaLabel";
            this.NaLabel.Size = new System.Drawing.Size(27, 15);
            this.NaLabel.TabIndex = 24;
            this.NaLabel.Text = "N_a";
            // 
            // NaInput
            // 
            this.NaInput.AccessibleDescription = "NaInput";
            this.NaInput.Location = new System.Drawing.Point(48, 22);
            this.NaInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.NaInput.Name = "NaInput";
            this.NaInput.Size = new System.Drawing.Size(120, 23);
            this.NaInput.TabIndex = 23;
            // 
            // testExperimentCountLabel
            // 
            this.testExperimentCountLabel.AutoSize = true;
            this.testExperimentCountLabel.Location = new System.Drawing.Point(24, 23);
            this.testExperimentCountLabel.Name = "testExperimentCountLabel";
            this.testExperimentCountLabel.Size = new System.Drawing.Size(127, 15);
            this.testExperimentCountLabel.TabIndex = 21;
            this.testExperimentCountLabel.Text = "Liczba eksperymentów";
            // 
            // testExperimentCount
            // 
            this.testExperimentCount.Location = new System.Drawing.Point(157, 19);
            this.testExperimentCount.Name = "testExperimentCount";
            this.testExperimentCount.Size = new System.Drawing.Size(120, 23);
            this.testExperimentCount.TabIndex = 20;
            // 
            // testyStart
            // 
            this.testyStart.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.testyStart.Location = new System.Drawing.Point(33, 60);
            this.testyStart.Name = "testyStart";
            this.testyStart.Size = new System.Drawing.Size(244, 163);
            this.testyStart.TabIndex = 19;
            this.testyStart.Text = "START TESTY";
            this.testyStart.UseVisualStyleBackColor = false;
            this.testyStart.Click += new System.EventHandler(this.testyStart_Click);
            // 
            // iterationNumberInput
            // 
            this.iterationNumberInput.Location = new System.Drawing.Point(435, 58);
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
            this.iterationNumberInput.Size = new System.Drawing.Size(58, 23);
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
            this.iterationNumberLabel.Location = new System.Drawing.Point(317, 62);
            this.iterationNumberLabel.Name = "iterationNumberLabel";
            this.iterationNumberLabel.Size = new System.Drawing.Size(87, 15);
            this.iterationNumberLabel.TabIndex = 17;
            this.iterationNumberLabel.Text = "Ilość iteracji (T)";
            // 
            // additioanlDataButton
            // 
            this.additioanlDataButton.Location = new System.Drawing.Point(499, 27);
            this.additioanlDataButton.Name = "additioanlDataButton";
            this.additioanlDataButton.Size = new System.Drawing.Size(94, 81);
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
            this.inputDataGroupBox.Controls.Add(this.seed);
            this.inputDataGroupBox.Controls.Add(this.useSeed);
            this.inputDataGroupBox.Controls.Add(this.experimentNumberLabel);
            this.inputDataGroupBox.Controls.Add(this.experimentNumber);
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
            this.inputDataGroupBox.Size = new System.Drawing.Size(783, 126);
            this.inputDataGroupBox.TabIndex = 23;
            this.inputDataGroupBox.TabStop = false;
            this.inputDataGroupBox.Text = "Dane wejściowe";
            // 
            // seed
            // 
            this.seed.Location = new System.Drawing.Point(604, 83);
            this.seed.Name = "seed";
            this.seed.Size = new System.Drawing.Size(171, 23);
            this.seed.TabIndex = 26;
            // 
            // useSeed
            // 
            this.useSeed.AutoSize = true;
            this.useSeed.Location = new System.Drawing.Point(604, 60);
            this.useSeed.Name = "useSeed";
            this.useSeed.Size = new System.Drawing.Size(82, 19);
            this.useSeed.TabIndex = 25;
            this.useSeed.Text = "Użyj ziarna";
            this.useSeed.UseVisualStyleBackColor = true;
            this.useSeed.CheckedChanged += new System.EventHandler(this.useSeed_CheckedChanged);
            // 
            // experimentNumberLabel
            // 
            this.experimentNumberLabel.AutoSize = true;
            this.experimentNumberLabel.Location = new System.Drawing.Point(599, 33);
            this.experimentNumberLabel.Name = "experimentNumberLabel";
            this.experimentNumberLabel.Size = new System.Drawing.Size(118, 15);
            this.experimentNumberLabel.TabIndex = 21;
            this.experimentNumberLabel.Text = "Ilość eksperymentów";
            // 
            // experimentNumber
            // 
            this.experimentNumber.Location = new System.Drawing.Point(717, 29);
            this.experimentNumber.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.experimentNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.experimentNumber.Name = "experimentNumber";
            this.experimentNumber.Size = new System.Drawing.Size(58, 23);
            this.experimentNumber.TabIndex = 22;
            this.experimentNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // runProgressBar
            // 
            this.runProgressBar.Location = new System.Drawing.Point(1220, 27);
            this.runProgressBar.Name = "runProgressBar";
            this.runProgressBar.Size = new System.Drawing.Size(210, 23);
            this.runProgressBar.TabIndex = 24;
            // 
            // GAmodification
            // 
            this.GAmodification.Controls.Add(this.modifiedGARadio);
            this.GAmodification.Controls.Add(this.classicalGARadio);
            this.GAmodification.Location = new System.Drawing.Point(840, 79);
            this.GAmodification.Name = "GAmodification";
            this.GAmodification.Size = new System.Drawing.Size(138, 126);
            this.GAmodification.TabIndex = 23;
            this.GAmodification.TabStop = false;
            this.GAmodification.Text = "Opcje algorytmu";
            // 
            // modifiedGARadio
            // 
            this.modifiedGARadio.AutoSize = true;
            this.modifiedGARadio.Location = new System.Drawing.Point(6, 47);
            this.modifiedGARadio.Name = "modifiedGARadio";
            this.modifiedGARadio.Size = new System.Drawing.Size(130, 19);
            this.modifiedGARadio.TabIndex = 1;
            this.modifiedGARadio.TabStop = true;
            this.modifiedGARadio.Text = "Zmodyfikowany GA";
            this.modifiedGARadio.UseVisualStyleBackColor = true;
            this.modifiedGARadio.CheckedChanged += new System.EventHandler(this.modifiedGARadio_CheckedChanged);
            // 
            // classicalGARadio
            // 
            this.classicalGARadio.AutoSize = true;
            this.classicalGARadio.Location = new System.Drawing.Point(6, 22);
            this.classicalGARadio.Name = "classicalGARadio";
            this.classicalGARadio.Size = new System.Drawing.Size(95, 19);
            this.classicalGARadio.TabIndex = 0;
            this.classicalGARadio.TabStop = true;
            this.classicalGARadio.Text = "Klasyczny GA";
            this.classicalGARadio.UseVisualStyleBackColor = true;
            this.classicalGARadio.CheckedChanged += new System.EventHandler(this.classicalGA_CheckedChanged);
            // 
            // selectionGroup
            // 
            this.selectionGroup.Controls.Add(this.tournamentRadio);
            this.selectionGroup.Controls.Add(this.ruletteRadio);
            this.selectionGroup.Location = new System.Drawing.Point(984, 79);
            this.selectionGroup.Name = "selectionGroup";
            this.selectionGroup.Size = new System.Drawing.Size(80, 126);
            this.selectionGroup.TabIndex = 2;
            this.selectionGroup.TabStop = false;
            this.selectionGroup.Text = "Selekcja";
            // 
            // tournamentRadio
            // 
            this.tournamentRadio.AutoSize = true;
            this.tournamentRadio.Location = new System.Drawing.Point(7, 47);
            this.tournamentRadio.Name = "tournamentRadio";
            this.tournamentRadio.Size = new System.Drawing.Size(59, 19);
            this.tournamentRadio.TabIndex = 1;
            this.tournamentRadio.TabStop = true;
            this.tournamentRadio.Text = "turniej";
            this.tournamentRadio.UseVisualStyleBackColor = true;
            this.tournamentRadio.CheckedChanged += new System.EventHandler(this.tournamentRadio_CheckedChanged);
            // 
            // ruletteRadio
            // 
            this.ruletteRadio.AutoSize = true;
            this.ruletteRadio.Location = new System.Drawing.Point(7, 22);
            this.ruletteRadio.Name = "ruletteRadio";
            this.ruletteRadio.Size = new System.Drawing.Size(61, 19);
            this.ruletteRadio.TabIndex = 0;
            this.ruletteRadio.TabStop = true;
            this.ruletteRadio.Text = "ruletka";
            this.ruletteRadio.UseVisualStyleBackColor = true;
            this.ruletteRadio.CheckedChanged += new System.EventHandler(this.ruletteRadio_CheckedChanged);
            // 
            // crossGroup
            // 
            this.crossGroup.Controls.Add(this.crossPoints);
            this.crossGroup.Controls.Add(this.multiPointRadio);
            this.crossGroup.Controls.Add(this.singlePointRadio);
            this.crossGroup.Location = new System.Drawing.Point(1070, 79);
            this.crossGroup.Name = "crossGroup";
            this.crossGroup.Size = new System.Drawing.Size(110, 126);
            this.crossGroup.TabIndex = 3;
            this.crossGroup.TabStop = false;
            this.crossGroup.Text = "Krzyżowanie";
            // 
            // crossPoints
            // 
            this.crossPoints.Location = new System.Drawing.Point(25, 72);
            this.crossPoints.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.crossPoints.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.crossPoints.Name = "crossPoints";
            this.crossPoints.Size = new System.Drawing.Size(51, 23);
            this.crossPoints.TabIndex = 23;
            this.crossPoints.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // multiPointRadio
            // 
            this.multiPointRadio.AutoSize = true;
            this.multiPointRadio.Location = new System.Drawing.Point(7, 47);
            this.multiPointRadio.Name = "multiPointRadio";
            this.multiPointRadio.Size = new System.Drawing.Size(90, 19);
            this.multiPointRadio.TabIndex = 1;
            this.multiPointRadio.TabStop = true;
            this.multiPointRadio.Text = "n-punktowe";
            this.multiPointRadio.UseVisualStyleBackColor = true;
            this.multiPointRadio.CheckedChanged += new System.EventHandler(this.multiPointRadio_CheckedChanged);
            // 
            // singlePointRadio
            // 
            this.singlePointRadio.AutoSize = true;
            this.singlePointRadio.Location = new System.Drawing.Point(7, 22);
            this.singlePointRadio.Name = "singlePointRadio";
            this.singlePointRadio.Size = new System.Drawing.Size(89, 19);
            this.singlePointRadio.TabIndex = 0;
            this.singlePointRadio.TabStop = true;
            this.singlePointRadio.Text = "1-punktowe";
            this.singlePointRadio.UseVisualStyleBackColor = true;
            this.singlePointRadio.CheckedChanged += new System.EventHandler(this.singlePointRadio_CheckedChanged);
            // 
            // mutationGroup
            // 
            this.mutationGroup.Controls.Add(this.bitSwapingRadio);
            this.mutationGroup.Controls.Add(this.evenlyRadio);
            this.mutationGroup.Location = new System.Drawing.Point(1186, 79);
            this.mutationGroup.Name = "mutationGroup";
            this.mutationGroup.Size = new System.Drawing.Size(123, 126);
            this.mutationGroup.TabIndex = 3;
            this.mutationGroup.TabStop = false;
            this.mutationGroup.Text = "Mutacja";
            // 
            // bitSwapingRadio
            // 
            this.bitSwapingRadio.AutoSize = true;
            this.bitSwapingRadio.Location = new System.Drawing.Point(7, 47);
            this.bitSwapingRadio.Name = "bitSwapingRadio";
            this.bitSwapingRadio.Size = new System.Drawing.Size(110, 19);
            this.bitSwapingRadio.TabIndex = 1;
            this.bitSwapingRadio.TabStop = true;
            this.bitSwapingRadio.Text = "zmieniająca bity";
            this.bitSwapingRadio.UseVisualStyleBackColor = true;
            this.bitSwapingRadio.CheckedChanged += new System.EventHandler(this.bitSwapingRadio_CheckedChanged);
            // 
            // evenlyRadio
            // 
            this.evenlyRadio.AutoSize = true;
            this.evenlyRadio.Location = new System.Drawing.Point(7, 22);
            this.evenlyRadio.Name = "evenlyRadio";
            this.evenlyRadio.Size = new System.Drawing.Size(96, 19);
            this.evenlyRadio.TabIndex = 0;
            this.evenlyRadio.TabStop = true;
            this.evenlyRadio.Text = "równomierna";
            this.evenlyRadio.UseVisualStyleBackColor = true;
            this.evenlyRadio.CheckedChanged += new System.EventHandler(this.evenlyRadio_CheckedChanged);
            // 
            // probGen1Group
            // 
            this.probGen1Group.Controls.Add(this.propGen1);
            this.probGen1Group.Location = new System.Drawing.Point(324, 12);
            this.probGen1Group.Name = "probGen1Group";
            this.probGen1Group.Size = new System.Drawing.Size(88, 48);
            this.probGen1Group.TabIndex = 23;
            this.probGen1Group.TabStop = false;
            this.probGen1Group.Text = "prob_gen_1";
            // 
            // propGen1
            // 
            this.propGen1.DecimalPlaces = 2;
            this.propGen1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.propGen1.Location = new System.Drawing.Point(6, 18);
            this.propGen1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.propGen1.Name = "propGen1";
            this.propGen1.Size = new System.Drawing.Size(70, 23);
            this.propGen1.TabIndex = 14;
            this.propGen1.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1464, 1094);
            this.Controls.Add(this.probGen1Group);
            this.Controls.Add(this.mutationGroup);
            this.Controls.Add(this.crossGroup);
            this.Controls.Add(this.selectionGroup);
            this.Controls.Add(this.GAmodification);
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
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TstepInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TaInput)).EndInit();
            this.pmgroupbox.ResumeLayout(false);
            this.pmgroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PmstepInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PmbInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmaInput)).EndInit();
            this.pkgroupbox.ResumeLayout(false);
            this.pkgroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PkstepInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PkbbInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pkaInput)).EndInit();
            this.individiualsCountGroupBox.ResumeLayout(false);
            this.individiualsCountGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NstepInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NbInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NaInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testExperimentCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iterationNumberInput)).EndInit();
            this.algorithmTypeGroupBox.ResumeLayout(false);
            this.algorithmTypeGroupBox.PerformLayout();
            this.inputDataGroupBox.ResumeLayout(false);
            this.inputDataGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.experimentNumber)).EndInit();
            this.GAmodification.ResumeLayout(false);
            this.GAmodification.PerformLayout();
            this.selectionGroup.ResumeLayout(false);
            this.selectionGroup.PerformLayout();
            this.crossGroup.ResumeLayout(false);
            this.crossGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.crossPoints)).EndInit();
            this.mutationGroup.ResumeLayout(false);
            this.mutationGroup.PerformLayout();
            this.probGen1Group.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propGen1)).EndInit();
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
        private GroupBox pmgroupbox;
        private Label pmbsizeLabel;
        private NumericUpDown PmstepInput;
        private Label pmblabel;
        private NumericUpDown PmbInput;
        private Label PmaLabel;
        private NumericUpDown pmaInput;
        private GroupBox pkgroupbox;
        private Label PkSizeLabel;
        private NumericUpDown PkstepInput;
        private Label pkbLabel;
        private NumericUpDown PkbbInput;
        private Label pka;
        private NumericUpDown pkaInput;
        private GroupBox individiualsCountGroupBox;
        private Label NstepLabel;
        private NumericUpDown NstepInput;
        private Label NbLabel;
        private NumericUpDown NbInput;
        private Label NaLabel;
        private NumericUpDown NaInput;
        private Label testExperimentCountLabel;
        private NumericUpDown testExperimentCount;
        private GroupBox groupBox1;
        private Label TstepILabel;
        private NumericUpDown TstepInput;
        private Label TbLabel;
        private NumericUpDown TbInput;
        private Label TaLabel;
        private NumericUpDown TaInput;
        private Label experimentNumberLabel;
        private NumericUpDown experimentNumber;
        private GroupBox GAmodification;
        private RadioButton modifiedGARadio;
        private RadioButton classicalGARadio;
        private GroupBox selectionGroup;
        private RadioButton tournamentRadio;
        private RadioButton ruletteRadio;
        private GroupBox crossGroup;
        private NumericUpDown crossPoints;
        private RadioButton multiPointRadio;
        private RadioButton singlePointRadio;
        private GroupBox mutationGroup;
        private RadioButton bitSwapingRadio;
        private RadioButton evenlyRadio;
        private CheckBox useSeed;
        private TextBox seed;
        private GroupBox probGen1Group;
        private NumericUpDown propGen1;
    }
}