﻿namespace CrashTestNET
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainerTopVsBottom = new System.Windows.Forms.SplitContainer();
            this.splitContainerControlsVsGraph = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxInit = new System.Windows.Forms.GroupBox();
            this.checkBoxVerbose = new System.Windows.Forms.CheckBox();
            this.buttonInit = new System.Windows.Forms.Button();
            this.groupBoxTestControl = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelTimeRemaining = new System.Windows.Forms.Label();
            this.numericUpDownTestSeconds = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxMonteCarlo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownIntegTimeMin = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownIntegTimeMax = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownIterDelayMin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownReadDelayMin = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownReadDelayMax = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownIterDelayMax = new System.Windows.Forms.NumericUpDown();
            this.checkBoxIntegThrowaways = new System.Windows.Forms.CheckBox();
            this.checkBoxSerializeSpecs = new System.Windows.Forms.CheckBox();
            this.checkBoxSerializeReads = new System.Windows.Forms.CheckBox();
            this.checkBoxHasMarker = new System.Windows.Forms.CheckBox();
            this.numericUpDownExtraReads = new System.Windows.Forms.NumericUpDown();
            this.checkBoxTrackMetrics = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSpectra = new System.Windows.Forms.TabPage();
            this.splitContainerGraphVsStatus = new System.Windows.Forms.SplitContainer();
            this.chartAll = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvStatus = new System.Windows.Forms.DataGridView();
            this.tabPageTime = new System.Windows.Forms.TabPage();
            this.chartTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxEventLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTopVsBottom)).BeginInit();
            this.splitContainerTopVsBottom.Panel1.SuspendLayout();
            this.splitContainerTopVsBottom.Panel2.SuspendLayout();
            this.splitContainerTopVsBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlsVsGraph)).BeginInit();
            this.splitContainerControlsVsGraph.Panel1.SuspendLayout();
            this.splitContainerControlsVsGraph.Panel2.SuspendLayout();
            this.splitContainerControlsVsGraph.SuspendLayout();
            this.flowLayoutPanelControls.SuspendLayout();
            this.groupBoxInit.SuspendLayout();
            this.groupBoxTestControl.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTestSeconds)).BeginInit();
            this.groupBoxMonteCarlo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntegTimeMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntegTimeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterDelayMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadDelayMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadDelayMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterDelayMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExtraReads)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageSpectra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGraphVsStatus)).BeginInit();
            this.splitContainerGraphVsStatus.Panel1.SuspendLayout();
            this.splitContainerGraphVsStatus.Panel2.SuspendLayout();
            this.splitContainerGraphVsStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).BeginInit();
            this.tabPageTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTime)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerTopVsBottom
            // 
            this.splitContainerTopVsBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTopVsBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTopVsBottom.Name = "splitContainerTopVsBottom";
            this.splitContainerTopVsBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTopVsBottom.Panel1
            // 
            this.splitContainerTopVsBottom.Panel1.Controls.Add(this.splitContainerControlsVsGraph);
            // 
            // splitContainerTopVsBottom.Panel2
            // 
            this.splitContainerTopVsBottom.Panel2.Controls.Add(this.groupBox1);
            this.splitContainerTopVsBottom.Size = new System.Drawing.Size(800, 450);
            this.splitContainerTopVsBottom.SplitterDistance = 344;
            this.splitContainerTopVsBottom.TabIndex = 0;
            // 
            // splitContainerControlsVsGraph
            // 
            this.splitContainerControlsVsGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlsVsGraph.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerControlsVsGraph.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControlsVsGraph.Name = "splitContainerControlsVsGraph";
            // 
            // splitContainerControlsVsGraph.Panel1
            // 
            this.splitContainerControlsVsGraph.Panel1.Controls.Add(this.flowLayoutPanelControls);
            // 
            // splitContainerControlsVsGraph.Panel2
            // 
            this.splitContainerControlsVsGraph.Panel2.Controls.Add(this.tabControl1);
            this.splitContainerControlsVsGraph.Size = new System.Drawing.Size(800, 344);
            this.splitContainerControlsVsGraph.SplitterDistance = 209;
            this.splitContainerControlsVsGraph.TabIndex = 0;
            // 
            // flowLayoutPanelControls
            // 
            this.flowLayoutPanelControls.AutoScroll = true;
            this.flowLayoutPanelControls.Controls.Add(this.groupBoxInit);
            this.flowLayoutPanelControls.Controls.Add(this.groupBoxTestControl);
            this.flowLayoutPanelControls.Controls.Add(this.groupBoxMonteCarlo);
            this.flowLayoutPanelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelControls.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelControls.Name = "flowLayoutPanelControls";
            this.flowLayoutPanelControls.Size = new System.Drawing.Size(209, 344);
            this.flowLayoutPanelControls.TabIndex = 0;
            // 
            // groupBoxInit
            // 
            this.groupBoxInit.Controls.Add(this.checkBoxVerbose);
            this.groupBoxInit.Controls.Add(this.buttonInit);
            this.groupBoxInit.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInit.Name = "groupBoxInit";
            this.groupBoxInit.Size = new System.Drawing.Size(200, 52);
            this.groupBoxInit.TabIndex = 0;
            this.groupBoxInit.TabStop = false;
            this.groupBoxInit.Text = "Initialization";
            // 
            // checkBoxVerbose
            // 
            this.checkBoxVerbose.AutoSize = true;
            this.checkBoxVerbose.Location = new System.Drawing.Point(92, 22);
            this.checkBoxVerbose.Name = "checkBoxVerbose";
            this.checkBoxVerbose.Size = new System.Drawing.Size(65, 17);
            this.checkBoxVerbose.TabIndex = 1;
            this.checkBoxVerbose.Text = "Verbose";
            this.checkBoxVerbose.UseVisualStyleBackColor = true;
            this.checkBoxVerbose.CheckedChanged += new System.EventHandler(this.checkBoxVerbose_CheckedChanged);
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(10, 19);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(75, 23);
            this.buttonInit.TabIndex = 0;
            this.buttonInit.Text = "Initialize";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // groupBoxTestControl
            // 
            this.groupBoxTestControl.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxTestControl.Enabled = false;
            this.groupBoxTestControl.Location = new System.Drawing.Point(3, 61);
            this.groupBoxTestControl.Name = "groupBoxTestControl";
            this.groupBoxTestControl.Size = new System.Drawing.Size(200, 80);
            this.groupBoxTestControl.TabIndex = 2;
            this.groupBoxTestControl.TabStop = false;
            this.groupBoxTestControl.Text = "Test Control";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.buttonStart, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelTimeRemaining, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDownTestSeconds, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label5, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(194, 61);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(3, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelTimeRemaining
            // 
            this.labelTimeRemaining.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTimeRemaining.AutoSize = true;
            this.labelTimeRemaining.Location = new System.Drawing.Point(84, 8);
            this.labelTimeRemaining.Name = "labelTimeRemaining";
            this.labelTimeRemaining.Size = new System.Drawing.Size(83, 13);
            this.labelTimeRemaining.TabIndex = 1;
            this.labelTimeRemaining.Text = "Time Remaining";
            this.labelTimeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownTestSeconds
            // 
            this.numericUpDownTestSeconds.Location = new System.Drawing.Point(3, 32);
            this.numericUpDownTestSeconds.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.numericUpDownTestSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTestSeconds.Name = "numericUpDownTestSeconds";
            this.numericUpDownTestSeconds.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownTestSeconds.TabIndex = 1;
            this.numericUpDownTestSeconds.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownTestSeconds.ValueChanged += new System.EventHandler(this.numericUpDownTestSeconds_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "seconds";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxMonteCarlo
            // 
            this.groupBoxMonteCarlo.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxMonteCarlo.Enabled = false;
            this.groupBoxMonteCarlo.Location = new System.Drawing.Point(3, 147);
            this.groupBoxMonteCarlo.Name = "groupBoxMonteCarlo";
            this.groupBoxMonteCarlo.Size = new System.Drawing.Size(200, 300);
            this.groupBoxMonteCarlo.TabIndex = 1;
            this.groupBoxMonteCarlo.TabStop = false;
            this.groupBoxMonteCarlo.Text = "Monte Carlo Options";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownIntegTimeMin, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownIntegTimeMax, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownIterDelayMin, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownReadDelayMin, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownReadDelayMax, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownIterDelayMax, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxIntegThrowaways, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxSerializeSpecs, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxSerializeReads, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxHasMarker, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownExtraReads, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxTrackMetrics, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 14);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(194, 281);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numericUpDownIntegTimeMin
            // 
            this.numericUpDownIntegTimeMin.Location = new System.Drawing.Point(3, 29);
            this.numericUpDownIntegTimeMin.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownIntegTimeMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownIntegTimeMin.Name = "numericUpDownIntegTimeMin";
            this.numericUpDownIntegTimeMin.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownIntegTimeMin.TabIndex = 2;
            this.numericUpDownIntegTimeMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownIntegTimeMin.ValueChanged += new System.EventHandler(this.numericUpDownIntegTimeMin_ValueChanged);
            // 
            // numericUpDownIntegTimeMax
            // 
            this.numericUpDownIntegTimeMax.Location = new System.Drawing.Point(94, 29);
            this.numericUpDownIntegTimeMax.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownIntegTimeMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownIntegTimeMax.Name = "numericUpDownIntegTimeMax";
            this.numericUpDownIntegTimeMax.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownIntegTimeMax.TabIndex = 3;
            this.numericUpDownIntegTimeMax.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownIntegTimeMax.ValueChanged += new System.EventHandler(this.numericUpDownIntegTimeMax_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
            this.label3.Location = new System.Drawing.Point(3, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Integration Time (ms)";
            // 
            // numericUpDownIterDelayMin
            // 
            this.numericUpDownIterDelayMin.Location = new System.Drawing.Point(3, 114);
            this.numericUpDownIterDelayMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownIterDelayMin.Name = "numericUpDownIterDelayMin";
            this.numericUpDownIterDelayMin.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownIterDelayMin.TabIndex = 6;
            this.numericUpDownIterDelayMin.ValueChanged += new System.EventHandler(this.numericUpDownIterDelayMin_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max";
            // 
            // numericUpDownReadDelayMin
            // 
            this.numericUpDownReadDelayMin.Location = new System.Drawing.Point(3, 68);
            this.numericUpDownReadDelayMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownReadDelayMin.Name = "numericUpDownReadDelayMin";
            this.numericUpDownReadDelayMin.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownReadDelayMin.TabIndex = 4;
            this.numericUpDownReadDelayMin.ValueChanged += new System.EventHandler(this.numericUpDownReadDelayMin_ValueChanged);
            // 
            // numericUpDownReadDelayMax
            // 
            this.numericUpDownReadDelayMax.Location = new System.Drawing.Point(94, 68);
            this.numericUpDownReadDelayMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownReadDelayMax.Name = "numericUpDownReadDelayMax";
            this.numericUpDownReadDelayMax.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownReadDelayMax.TabIndex = 5;
            this.numericUpDownReadDelayMax.ValueChanged += new System.EventHandler(this.numericUpDownReadDelayMax_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Read Delay (ms)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label6, 2);
            this.label6.Location = new System.Drawing.Point(3, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Iteration Delay (ms)";
            // 
            // numericUpDownIterDelayMax
            // 
            this.numericUpDownIterDelayMax.Location = new System.Drawing.Point(94, 114);
            this.numericUpDownIterDelayMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownIterDelayMax.Name = "numericUpDownIterDelayMax";
            this.numericUpDownIterDelayMax.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownIterDelayMax.TabIndex = 7;
            this.numericUpDownIterDelayMax.ValueChanged += new System.EventHandler(this.numericUpDownIterDelayMax_ValueChanged);
            // 
            // checkBoxIntegThrowaways
            // 
            this.checkBoxIntegThrowaways.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxIntegThrowaways, 2);
            this.checkBoxIntegThrowaways.Location = new System.Drawing.Point(3, 140);
            this.checkBoxIntegThrowaways.Name = "checkBoxIntegThrowaways";
            this.checkBoxIntegThrowaways.Size = new System.Drawing.Size(86, 17);
            this.checkBoxIntegThrowaways.TabIndex = 8;
            this.checkBoxIntegThrowaways.Text = "Throwaways";
            this.checkBoxIntegThrowaways.UseVisualStyleBackColor = true;
            this.checkBoxIntegThrowaways.CheckedChanged += new System.EventHandler(this.checkBoxIntegThrowaways_CheckedChanged);
            // 
            // checkBoxSerializeSpecs
            // 
            this.checkBoxSerializeSpecs.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxSerializeSpecs, 2);
            this.checkBoxSerializeSpecs.Location = new System.Drawing.Point(3, 163);
            this.checkBoxSerializeSpecs.Name = "checkBoxSerializeSpecs";
            this.checkBoxSerializeSpecs.Size = new System.Drawing.Size(136, 17);
            this.checkBoxSerializeSpecs.TabIndex = 9;
            this.checkBoxSerializeSpecs.Text = "Serialize Spectrometers";
            this.checkBoxSerializeSpecs.UseVisualStyleBackColor = true;
            this.checkBoxSerializeSpecs.CheckedChanged += new System.EventHandler(this.checkBoxSerializeSpecs_CheckedChanged);
            // 
            // checkBoxSerializeReads
            // 
            this.checkBoxSerializeReads.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxSerializeReads, 2);
            this.checkBoxSerializeReads.Location = new System.Drawing.Point(3, 186);
            this.checkBoxSerializeReads.Name = "checkBoxSerializeReads";
            this.checkBoxSerializeReads.Size = new System.Drawing.Size(99, 17);
            this.checkBoxSerializeReads.TabIndex = 10;
            this.checkBoxSerializeReads.Text = "Serialize Reads";
            this.checkBoxSerializeReads.UseVisualStyleBackColor = true;
            this.checkBoxSerializeReads.CheckedChanged += new System.EventHandler(this.checkBoxSerializeReads_CheckedChanged);
            // 
            // checkBoxHasMarker
            // 
            this.checkBoxHasMarker.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxHasMarker, 2);
            this.checkBoxHasMarker.Location = new System.Drawing.Point(3, 209);
            this.checkBoxHasMarker.Name = "checkBoxHasMarker";
            this.checkBoxHasMarker.Size = new System.Drawing.Size(81, 17);
            this.checkBoxHasMarker.TabIndex = 11;
            this.checkBoxHasMarker.Text = "Has Marker";
            this.checkBoxHasMarker.UseVisualStyleBackColor = true;
            this.checkBoxHasMarker.CheckedChanged += new System.EventHandler(this.checkBoxHasMarker_CheckedChanged);
            // 
            // numericUpDownExtraReads
            // 
            this.numericUpDownExtraReads.Location = new System.Drawing.Point(3, 255);
            this.numericUpDownExtraReads.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownExtraReads.Name = "numericUpDownExtraReads";
            this.numericUpDownExtraReads.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownExtraReads.TabIndex = 12;
            this.numericUpDownExtraReads.ValueChanged += new System.EventHandler(this.numericUpDownExtraReads_ValueChanged);
            // 
            // checkBoxTrackMetrics
            // 
            this.checkBoxTrackMetrics.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxTrackMetrics, 2);
            this.checkBoxTrackMetrics.Location = new System.Drawing.Point(3, 232);
            this.checkBoxTrackMetrics.Name = "checkBoxTrackMetrics";
            this.checkBoxTrackMetrics.Size = new System.Drawing.Size(87, 17);
            this.checkBoxTrackMetrics.TabIndex = 14;
            this.checkBoxTrackMetrics.Text = "Track Metrics";
            this.checkBoxTrackMetrics.UseVisualStyleBackColor = true;
            this.checkBoxTrackMetrics.CheckedChanged += new System.EventHandler(this.checkBoxTrackMetrics_CheckedChanged);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(94, 260);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Extra Reads";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSpectra);
            this.tabControl1.Controls.Add(this.tabPageTime);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(587, 344);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageSpectra
            // 
            this.tabPageSpectra.Controls.Add(this.splitContainerGraphVsStatus);
            this.tabPageSpectra.Location = new System.Drawing.Point(4, 22);
            this.tabPageSpectra.Name = "tabPageSpectra";
            this.tabPageSpectra.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSpectra.Size = new System.Drawing.Size(579, 318);
            this.tabPageSpectra.TabIndex = 0;
            this.tabPageSpectra.Text = "Spectra";
            this.tabPageSpectra.UseVisualStyleBackColor = true;
            // 
            // splitContainerGraphVsStatus
            // 
            this.splitContainerGraphVsStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerGraphVsStatus.Location = new System.Drawing.Point(3, 3);
            this.splitContainerGraphVsStatus.Name = "splitContainerGraphVsStatus";
            this.splitContainerGraphVsStatus.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerGraphVsStatus.Panel1
            // 
            this.splitContainerGraphVsStatus.Panel1.Controls.Add(this.chartAll);
            // 
            // splitContainerGraphVsStatus.Panel2
            // 
            this.splitContainerGraphVsStatus.Panel2.Controls.Add(this.dgvStatus);
            this.splitContainerGraphVsStatus.Size = new System.Drawing.Size(573, 312);
            this.splitContainerGraphVsStatus.SplitterDistance = 199;
            this.splitContainerGraphVsStatus.TabIndex = 1;
            // 
            // chartAll
            // 
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LabelStyle.Format = "f2";
            chartArea1.AxisX.Title = "Wavelength";
            chartArea1.AxisY.Title = "Intensity (Counts)";
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            this.chartAll.ChartAreas.Add(chartArea1);
            this.chartAll.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartAll.Legends.Add(legend1);
            this.chartAll.Location = new System.Drawing.Point(0, 0);
            this.chartAll.Name = "chartAll";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartAll.Series.Add(series1);
            this.chartAll.Size = new System.Drawing.Size(573, 199);
            this.chartAll.TabIndex = 0;
            this.chartAll.Text = "chart1";
            // 
            // dgvStatus
            // 
            this.dgvStatus.AllowUserToAddRows = false;
            this.dgvStatus.AllowUserToDeleteRows = false;
            this.dgvStatus.AllowUserToOrderColumns = true;
            this.dgvStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvStatus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatus.Location = new System.Drawing.Point(0, 0);
            this.dgvStatus.Name = "dgvStatus";
            this.dgvStatus.ReadOnly = true;
            this.dgvStatus.Size = new System.Drawing.Size(573, 109);
            this.dgvStatus.TabIndex = 0;
            // 
            // tabPageTime
            // 
            this.tabPageTime.Controls.Add(this.chartTime);
            this.tabPageTime.Location = new System.Drawing.Point(4, 22);
            this.tabPageTime.Name = "tabPageTime";
            this.tabPageTime.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTime.Size = new System.Drawing.Size(579, 318);
            this.tabPageTime.TabIndex = 1;
            this.tabPageTime.Text = "Time";
            this.tabPageTime.UseVisualStyleBackColor = true;
            // 
            // chartTime
            // 
            chartArea2.Name = "ChartArea1";
            this.chartTime.ChartAreas.Add(chartArea2);
            this.chartTime.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartTime.Legends.Add(legend2);
            this.chartTime.Location = new System.Drawing.Point(3, 3);
            this.chartTime.Name = "chartTime";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartTime.Series.Add(series2);
            this.chartTime.Size = new System.Drawing.Size(573, 312);
            this.chartTime.TabIndex = 0;
            this.chartTime.Text = "chart1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxEventLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Event Log";
            // 
            // textBoxEventLog
            // 
            this.textBoxEventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventLog.Location = new System.Drawing.Point(3, 16);
            this.textBoxEventLog.Multiline = true;
            this.textBoxEventLog.Name = "textBoxEventLog";
            this.textBoxEventLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxEventLog.Size = new System.Drawing.Size(794, 83);
            this.textBoxEventLog.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainerTopVsBottom);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainerTopVsBottom.Panel1.ResumeLayout(false);
            this.splitContainerTopVsBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTopVsBottom)).EndInit();
            this.splitContainerTopVsBottom.ResumeLayout(false);
            this.splitContainerControlsVsGraph.Panel1.ResumeLayout(false);
            this.splitContainerControlsVsGraph.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlsVsGraph)).EndInit();
            this.splitContainerControlsVsGraph.ResumeLayout(false);
            this.flowLayoutPanelControls.ResumeLayout(false);
            this.groupBoxInit.ResumeLayout(false);
            this.groupBoxInit.PerformLayout();
            this.groupBoxTestControl.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTestSeconds)).EndInit();
            this.groupBoxMonteCarlo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntegTimeMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntegTimeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterDelayMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadDelayMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReadDelayMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIterDelayMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExtraReads)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSpectra.ResumeLayout(false);
            this.splitContainerGraphVsStatus.Panel1.ResumeLayout(false);
            this.splitContainerGraphVsStatus.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGraphVsStatus)).EndInit();
            this.splitContainerGraphVsStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).EndInit();
            this.tabPageTime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTime)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerTopVsBottom;
        private System.Windows.Forms.SplitContainer splitContainerControlsVsGraph;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelControls;
        private System.Windows.Forms.GroupBox groupBoxInit;
        private System.Windows.Forms.CheckBox checkBoxVerbose;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.GroupBox groupBoxTestControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelTimeRemaining;
        private System.Windows.Forms.GroupBox groupBoxMonteCarlo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numericUpDownIntegTimeMin;
        private System.Windows.Forms.NumericUpDown numericUpDownIntegTimeMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownReadDelayMin;
        private System.Windows.Forms.NumericUpDown numericUpDownReadDelayMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSpectra;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAll;
        private System.Windows.Forms.TabPage tabPageTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxEventLog;
        private System.Windows.Forms.NumericUpDown numericUpDownTestSeconds;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTime;
        private System.Windows.Forms.NumericUpDown numericUpDownIterDelayMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownIterDelayMax;
        private System.Windows.Forms.DataGridView dgvStatus;
        private System.Windows.Forms.CheckBox checkBoxIntegThrowaways;
        private System.Windows.Forms.NumericUpDown numericUpDownExtraReads;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxSerializeSpecs;
        private System.Windows.Forms.CheckBox checkBoxSerializeReads;
        private System.Windows.Forms.SplitContainer splitContainerGraphVsStatus;
        private System.Windows.Forms.CheckBox checkBoxHasMarker;
        private System.Windows.Forms.CheckBox checkBoxTrackMetrics;
    }
}

