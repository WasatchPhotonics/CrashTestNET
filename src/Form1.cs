using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using System.Data;

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

        SortedDictionary<string, SpectrometerState> states = new SortedDictionary<string, SpectrometerState>();
        BindingSource statusSource;

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
                logger.info("Index {0}: model {1}, serial {2}, detector {3}, pixels {4}, range ({5:f2}, {6:f2}nm)",
                    i, spec.model, spec.serialNumber, spec.eeprom.detectorName, spec.pixels,
                    spec.wavelengths[0], spec.wavelengths[spec.pixels - 1]);

                SpectrometerState state = new SpectrometerState(spec);

                // init graphs
                state.series = new Series(sn);
                state.series.ChartType = SeriesChartType.Line;
                chartAll.Series.Add(state.series);

                // init workers
                state.worker = new BackgroundWorker() { WorkerSupportsCancellation = true };
                state.worker.DoWork += Worker_DoWork;
                state.worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                states[sn] = state;
            }

            var bindingList = new BindingList<SpectrometerStatus>();
            foreach (var pair in states)
                bindingList.Add(pair.Value.status);
            statusSource = new BindingSource(bindingList, null);
            dgvStatus.DataSource = statusSource;

            groupBoxTestControl.Enabled = true;
        }

        ////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////

        void buttonStart_Click(object sender, EventArgs e)
        {
            if (running)
            {
                foreach (var pair in states)
                    pair.Value.worker.CancelAsync();
            }
            else
            {
                buttonStart.Text = "Stop";
                stopTime = DateTime.Now.AddMinutes(testDurationMin);
                updateTimeRemaining();
                foreach (var pair in states)
                {
                    logger.debug($"starting worker for {pair.Key}");
                    pair.Value.worker.RunWorkerAsync(pair.Key);
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
            var state = states[sn];
            state.series.Points.DataBindXY(state.spec.wavelengths, spectrum);

            updateTimeRemaining();
        }

        void updateTimeRemaining()
        {
            lock (labelTimeRemaining)
                labelTimeRemaining.Text = 
                    string.Format("{0:hh\\:mm\\:ss}", stopTime - DateTime.Now);

            statusSource.ResetBindings(false);
        }

        ////////////////////////////////////////////////////////////////////////
        // Background Worker
        ////////////////////////////////////////////////////////////////////////

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sn = e.Argument as string;
            var state = states[sn];
            var worker = sender as BackgroundWorker;
            var status = state.status;
            var spec = state.spec;

            status.reset();
            status.running = true;

            Thread.CurrentThread.Name = sn;
            logger.info("worker started");

            // enter event loop
            while (true)
            {
                if (worker.CancellationPending)
                    break;

                DateTime now = DateTime.Now;
                if (now >= stopTime)
                    break;

                // randomize integration time
                spec.integrationTimeMS = (uint)r.Next(integTimeMin, integTimeMax);

                // take acquisition
                var spectrum = spec.getSpectrum();
                if (spectrum is null)
                {
                    status.readFailures++;
                    status.consecutiveFailures++;
                    if (status.consecutiveFailures > MAX_CONSECUTIVE_FAILURES)
                    {
                        logger.error($"{sn}: giving up after {status.consecutiveFailures} consecutive failures");
                        break;
                    }

                    // try again
                    Thread.Sleep(1000);
                    continue;
                }

                status.acquisitions++;
                status.consecutiveFailures = 0;

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
            var state = states[sn];
            var status = state.status;

            status.running = false;
            logger.debug($"{sn} worker complete");

            var allComplete = true;
            foreach (var pair in states)
            {
                if (pair.Value.status.running)
                {
                    logger.debug($"waiting on {pair.Key} to close");
                    allComplete = false;
                }
            }

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
