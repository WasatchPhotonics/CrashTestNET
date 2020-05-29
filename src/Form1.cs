using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;

namespace CrashTestNET
{
    public partial class Form1 : Form
    {
        const int MAX_CONSECUTIVE_FAILURES = 3;

        int testDurationMin = 1;
        int integTimeMin = 0;
        int integTimeMax = 0;
        int readDelayMin = 0;
        int readDelayMax = 0;
        int iterDelayMin = 0;
        int iterDelayMax = 0;

        Random r = new Random();
        DateTime stopTime = DateTime.Now;

        bool running = false;

        Dictionary<string, WasatchNET.Spectrometer> specs = new Dictionary<string, WasatchNET.Spectrometer>();
        Dictionary<string, Series> serieses = new Dictionary<string, Series>();
        Dictionary<string, BackgroundWorker> workers = new Dictionary<string, BackgroundWorker>();
        Dictionary<string, bool> runnings = new Dictionary<string, bool>();

        WasatchNET.Logger logger = WasatchNET.Logger.getInstance();

        public Form1()
        {
            InitializeComponent();

            logger.setTextBox(textBoxEventLog);

            testDurationMin = (int)numericUpDownTestMinutes.Value;
            iterDelayMin = (int)numericUpDownIterDelayMin.Value;
            iterDelayMax = (int)numericUpDownIterDelayMax.Value;
            integTimeMin = (int)numericUpDownIntegTimeMin.Value;
            integTimeMax = (int)numericUpDownIntegTimeMax.Value;
            readDelayMin = (int)numericUpDownReadDelayMin.Value;
            readDelayMax = (int)numericUpDownReadDelayMax.Value;

            Text = string.Format("CrashTestNET {0}", 
                Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        protected override void OnFormClosing(FormClosingEventArgs e) => logger.setTextBox(null);

        void buttonInit_Click(object sender, EventArgs e)
        {
            chartAll.Series.Clear();

            var driver = WasatchNET.Driver.getInstance();
            Text = string.Format("CrashTestNET {0} (WasatchNET {1})", 
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                driver.version);

            var specCount = driver.openAllSpectrometers();
            if (specCount < 1)
            {
                logger.error("No spectrometers found");
                return;
            }

            for (int i = 0; i < specCount; i++)
            {
                var spec = driver.getSpectrometer(i);
                var sn = spec.serialNumber;
                specs[sn] = spec;

                // init graphs
                var s = new Series(sn);
                s.ChartType = SeriesChartType.Line;
                serieses[sn] = s;
                chartAll.Series.Add(s);

                // init workers
                var worker = new BackgroundWorker() { WorkerSupportsCancellation = true };
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                workers[sn] = worker;
            }
            groupBoxTestControl.Enabled = true;
        }

        ////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////

        void buttonStart_Click(object sender, EventArgs e)
        {
            if (running)
            {
                foreach (var pair in workers)
                    pair.Value.CancelAsync();
            }
            else
            {
                buttonStart.Text = "Stop";
                stopTime = DateTime.Now.AddMinutes(testDurationMin);
                updateTimeRemaining();
                foreach (var pair in workers)
                {
                    logger.debug($"starting working for {pair.Key}");
                    pair.Value.RunWorkerAsync(pair.Key);
                }
            }
        }

        void numericUpDownTestMinutes_ValueChanged(object sender, EventArgs e)
        {
            testDurationMin = (int)(sender as NumericUpDown).Value;
            stopTime = DateTime.Now.AddMinutes(testDurationMin);
        }

        void numericUpDownIntegTimeMin_ValueChanged(object sender, EventArgs e) =>
            integTimeMin = (int)(sender as NumericUpDown).Value;

        void numericUpDownIntegTimeMax_ValueChanged(object sender, EventArgs e) =>
            integTimeMax = (int)(sender as NumericUpDown).Value;

        void numericUpDownReadDelayMin_ValueChanged(object sender, EventArgs e) =>
            readDelayMin = (int)(sender as NumericUpDown).Value;

        void numericUpDownReadDelayMax_ValueChanged(object sender, EventArgs e) =>
            readDelayMax = (int)(sender as NumericUpDown).Value;

        void checkBoxVerbose_CheckedChanged(object sender, EventArgs e) =>
            logger.level = (sender as CheckBox).Checked 
                         ? WasatchNET.LogLevel.DEBUG 
                         : WasatchNET.LogLevel.INFO;

        ////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////

        void processSpectrum(string sn, double[] spectrum)
        {
            var spec = specs[sn];
            var s = serieses[sn];
            s.Points.DataBindXY(spec.wavelengths, spectrum);

            updateTimeRemaining();
        }

        void updateTimeRemaining()
        {
            lock (labelTimeRemaining)
                labelTimeRemaining.Text = 
                    string.Format("{0:hh\\:mm\\:ss}", stopTime - DateTime.Now);
        }

        ////////////////////////////////////////////////////////////////////////
        // Background Worker
        ////////////////////////////////////////////////////////////////////////

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var sn = e.Argument as string;
            var spec = specs[sn];

            runnings[sn] = true;
            Thread.CurrentThread.Name = sn;
            logger.info("worker started");

            int consecutiveFailures = 0;

            // enter event loop
            while (true)
            {
                if (worker.CancellationPending)
                    break;

                DateTime now = DateTime.Now;
                if (now >= stopTime)
                    break;

                // randomize integration time
                var ms = (uint)r.Next(integTimeMin, integTimeMax);
                logger.debug($"integTime -> {ms}");
                spec.integrationTimeMS = ms;

                // take acquisition
                var spectrum = spec.getSpectrum();
                if (spectrum is null)
                {
                    consecutiveFailures++;
                    if (consecutiveFailures > MAX_CONSECUTIVE_FAILURES)
                    {
                        logger.error($"{sn}: giving up after {consecutiveFailures} consecutive failures");
                        break;
                    }
                }

                // process measurement
                chartAll.BeginInvoke((MethodInvoker)delegate { processSpectrum(sn, spectrum); });

                if (worker.CancellationPending)
                    break;

                var delayMS = r.Next(iterDelayMin, iterDelayMax);
                Thread.Sleep(delayMS);
            }

            logger.info("worker closing");
            e.Result = sn;
        }

        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sn = e.Result as string;
            runnings[sn] = false;
            logger.debug($"{sn} worker complete");

            var allComplete = true;
            foreach (var pair in specs)
                if (runnings[pair.Key])
                    allComplete = false;

            if (allComplete)
            {
                running = false;
                buttonStart.Text = "Start";
            }
        }

        private void numericUpDownIterDelayMin_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
