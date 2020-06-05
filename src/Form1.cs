using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using WasatchNET;

namespace CrashTestNET
{
    public partial class Form1 : Form
    {
        // these have not yet been moved to Args
        const int MAX_CONSECUTIVE_FAILURES = 3;
        bool serializeSpecs = false;
        bool explicitSWTrigger = false;

        Args args;
        Random r = new Random();
        DateTime stopTime = DateTime.Now;

        System.Windows.Forms.Timer startupTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer displayTimer = new System.Windows.Forms.Timer();

        bool initializing;
        bool initialized;
        bool running;
        bool shutdownInProgress;

        SortedDictionary<string, SpectrometerState> states = new SortedDictionary<string, SpectrometerState>();
        BindingSource statusSource;
        BindingList<SpectrometerStatus> bindingList;

        // in case we're trying to reproduce a previous random failure
        int[] forceExtraReadSequence = null; // e.g. { 7, 30, 2, 1, 30 };
        int forceExtraReadIndex = 0;

        Mutex specMut = new Mutex();
        Mutex bindingMut = new Mutex();

        Logger logger = Logger.getInstance();

        ////////////////////////////////////////////////////////////////////////
        // Lifecycle
        ////////////////////////////////////////////////////////////////////////

        public Form1(Args args)
        {
            InitializeComponent();

            this.args = args;

            logger.setTextBox(textBoxEventLog);

            Text = string.Format("CrashTestNET {0}",
                Assembly.GetExecutingAssembly().GetName().Version.ToString());

            // apply command-line arguments
            checkBoxVerbose.Checked = args.verbose;
            checkBoxIntegThrowaways.Checked = args.throwaways;
            numericUpDownExtraReads.Value = args.extraReads;
            numericUpDownTestSeconds.Value = args.durationSec;
            numericUpDownIntegTimeMin.Value = args.integMin;
            numericUpDownIntegTimeMax.Value = args.integMax;

            // haven't exposed these through cmd-line args yet
            checkBoxSerializeSpecs_CheckedChanged(null, null);

            displayTimer.Interval = 100; // 0Hz
            displayTimer.Tick += tickDisplayTimer;

            // if autostarting, click the Initialize button 1sec after launch
            if (args.autoStart)
            {
                startupTimer.Interval = 1000;
                startupTimer.Tick += tickStartupTimer;
                startupTimer.Start();
            }
        }

        // Using a Windows Timer seems cumbersome, but ensures the button
        // clicks are on the GUI thread, and occur entirely outside the Form
        // constructor.  Seems to avoid some rare timing glitches under load?
        private void tickStartupTimer(Object myObject, EventArgs myEventArgs)
        {
            if (!initializing)
            {
                buttonInit_Click(null, null);
            }
            else if (initialized)
            {
                startupTimer.Stop();
                buttonStart_Click(null, null);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            shutdownInProgress = true;

            if (running)
            {
                logger.info("closing all workers");
                foreach (var pair in states)
                    pair.Value.worker.CancelAsync();

                // cancel this Close() event...the final Worker_RunWorkerCompleted
                // call will issue a fresh one
                e.Cancel = true;
                return;
            }

            while (true)
            {
                bool allStopped = true;
                foreach (var pair in states)
                    if (pair.Value.worker.IsBusy)
                        allStopped = false;

                if (allStopped)
                    break;

                logger.info("waiting for workers to close");
                Thread.Sleep(500);
            }

            // force synchronization with any in-progress calls to processSpectrum 
            bindingMut.WaitOne();
            {
                Thread.Sleep(1000);

                dgvStatus.AutoGenerateColumns = false;
                dgvStatus.DataSource = null;
                dgvStatus.Rows.Clear();
                dgvStatus.Refresh();

                chartAll.Series.Clear();
                if (bindingList != null)
                    bindingList.Clear();
            }
            bindingMut.ReleaseMutex();

            states.Clear();

            Driver.getInstance().closeAllSpectrometers();
        }

        ////////////////////////////////////////////////////////////////////////
        // Callbacks
        ////////////////////////////////////////////////////////////////////////

        void buttonInit_Click(object sender, EventArgs e)
        {
            initializing = true;

            if (initialized)
                return;

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

            states = new SortedDictionary<string, SpectrometerState>();
            for (int i = 0; i < specCount; i++)
            {
                var spec = driver.getSpectrometer(i);
                var sn = spec.serialNumber;
                if (sn is null)
                {
                    logger.error($"Index {i}: failed to parse EEPROM");
                    spec.close();
                    continue;
                }

                logger.info("Index {0}: model {1}, serial {2}, detector {3}, pixels {4}, range ({5:f2}, {6:f2}nm)",
                    i, spec.model, spec.serialNumber, spec.eeprom.detectorName, spec.pixels,
                    spec.wavelengths[0], spec.wavelengths[spec.pixels - 1]);

                spec.readTemperatureAfterSpectrum = true;

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

            bindingList = new BindingList<SpectrometerStatus>();
            foreach (var pair in states)
            {
                var state = pair.Value;
                var spec = state.spec;
                bindingList.Add(state.status);
                state.series.Points.DataBindXY(spec.wavelengths, new double[spec.pixels]);
            }
            statusSource = new BindingSource(bindingList, null);
            dgvStatus.DataSource = statusSource;

            // update settings
            updateReadDelays();
            checkBoxIntegThrowaways_CheckedChanged(null, null);

            checkBoxTrackMetrics.Checked = args.trackMetrics;

            groupBoxTestControl.Enabled = true;
            groupBoxMonteCarlo.Enabled = true;

            initialized = true;
            buttonInit.Enabled = false;
        }

        void buttonStart_Click(object sender, EventArgs e)
        {
            if (running)
            {
                foreach (var pair in states)
                    pair.Value.worker.CancelAsync();
            }
            else
            {
                running = true;
                buttonStart.Text = "Stop";
                stopTime = DateTime.Now.AddSeconds(args.durationSec);
                displayTimer.Start();
                foreach (var pair in states)
                {
                    var sn = pair.Key;
                    var state = pair.Value;
                    var worker = state.worker;
                    if (!worker.IsBusy)
                    {
                        logger.debug($"starting worker for {sn}");
                        worker.RunWorkerAsync(sn);
                    }
                    else
                    {
                        logger.error($"worker already running for {sn}");
                    }
                }
            }
        }

        void checkBoxVerbose_CheckedChanged(object sender, EventArgs e) =>
            logger.level = checkBoxVerbose.Checked 
                         ? WasatchNET.LogLevel.DEBUG 
                         : WasatchNET.LogLevel.INFO;

        void numericUpDownTestSeconds_ValueChanged(object sender, EventArgs e)
        {
            args.durationSec = (int)numericUpDownTestSeconds.Value;
            stopTime = DateTime.Now.AddSeconds(args.durationSec);
        }

        void numericUpDownIntegTimeMin_ValueChanged(object sender, EventArgs e) =>
            args.integMin = (int)numericUpDownIntegTimeMin.Value;

        void numericUpDownIntegTimeMax_ValueChanged(object sender, EventArgs e) =>
            args.integMax = (int)numericUpDownIntegTimeMax.Value;

        void numericUpDownIterDelayMin_ValueChanged(object sender, EventArgs e) =>
            args.iterDelayMin = (int)numericUpDownIterDelayMin.Value;

        void numericUpDownIterDelayMax_ValueChanged(object sender, EventArgs e) =>
            args.iterDelayMax = (int)numericUpDownIterDelayMax.Value;

        void numericUpDownReadDelayMin_ValueChanged(object sender, EventArgs e)
        {
            args.readDelayMin = (int)numericUpDownReadDelayMin.Value;
            updateReadDelays();
        }

        void numericUpDownReadDelayMax_ValueChanged(object sender, EventArgs e)
        {
            args.readDelayMax = (int)numericUpDownReadDelayMax.Value;
            updateReadDelays();
        }

        void checkBoxIntegThrowaways_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var pair in states)
                pair.Value.spec.throwawayAfterIntegrationTime = checkBoxIntegThrowaways.Checked;
        }

        void numericUpDownExtraReads_ValueChanged(object sender, EventArgs e) =>
            args.extraReads = (int)numericUpDownExtraReads.Value;

        void checkBoxSerializeSpecs_CheckedChanged(object sender, EventArgs e) =>
            serializeSpecs = checkBoxSerializeSpecs.Checked;

        private void checkBoxSerializeReads_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var pair in states)
                pair.Value.spec.useReadoutMutex = checkBoxSerializeReads.Checked;
        }

        private void checkBoxHasMarker_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var pair in states)
                pair.Value.spec.hasMarker = checkBoxHasMarker.Checked;
        }

        private void checkBoxTrackMetrics_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var pair in states)
                pair.Value.metrics.enabled = checkBoxTrackMetrics.Checked;
        }

        ////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////

        private void tickDisplayTimer(Object myObject, EventArgs myEventArgs)
        {
            if (!running || shutdownInProgress)
            {
                displayTimer.Stop();
                return;
            }

            if (!bindingMut.WaitOne(5))
                return;

            // update time remaining
            labelTimeRemaining.Text = string.Format("{0:hh\\:mm\\:ss}", stopTime - DateTime.Now);

            // process each spectrometer
            foreach (var pair in states)
            {
                var sn = pair.Key;
                var state = pair.Value;
                var spec = state.spec;

                var spectrum = spec.lastSpectrum;
                if (spectrum is null)
                    continue;

                // graph the latest spectrum
                state.series.Points.DataBindXY(spec.wavelengths, spectrum);

                // check for shifts / swaps
                state.metrics.process(spectrum);
            }

            // update dataGridView
            if (statusSource != null)
                statusSource.ResetBindings(true);

            bindingMut.ReleaseMutex();
        }

        void updateReadDelays()
        {
            // if there is any read delay at all, we must explicitly send the SW
            // trigger, wait, then perform the read
            explicitSWTrigger = args.readDelayMin != 0 || args.readDelayMax != 0;

            // if using explicit SW triggers, disable the driver's autoTrigger
            foreach (var pair in states)
                pair.Value.spec.autoTrigger = !explicitSWTrigger;
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

            state.reset();
            status.running = true;

            Thread.CurrentThread.Name = sn;
            logger.info("worker started");

            // give all workers a chance to instantiate before diving in
            Thread.Sleep(r.Next(100, 500));

            // enter event loop
            while (true)
            {
                if (worker.CancellationPending || shutdownInProgress)
                    break;

                if (serializeSpecs)
                    specMut.WaitOne();

                DateTime now = DateTime.Now;
                if (now >= stopTime)
                    break;

                logger.header($"{sn} iteration {state.status.count}");

                // randomize integration time
                var integMS = (uint)r.Next(args.integMin, args.integMax);
                logger.debug($"randomizing integration time to {integMS}");
                spec.integrationTimeMS = integMS;

                ////////////////////////////////////////////////////////////////
                // take acquisition
                ////////////////////////////////////////////////////////////////

                logger.info($"{sn} iteration {state.status.count} integration {integMS} ms");

                logger.debug("taking measurement");
                if (explicitSWTrigger)
                {
                    spec.sendSWTrigger();
                    var delayMS = r.Next(args.readDelayMin, args.readDelayMax);
                    logger.debug($"waiting {delayMS} between trigger and read");
                    Thread.Sleep(delayMS);
                }
                var spectrum = spec.getSpectrum();

                if (spectrum is null)
                {
                    status.readFailures++;
                    status.consecFailures++;
                    if (status.consecFailures >= MAX_CONSECUTIVE_FAILURES)
                    {
                        logger.error($"{sn}: giving up after {status.consecFailures} consecutive failures");
                        break;
                    }

                    // try again
                    Thread.Sleep(1000);
                    continue;
                }

                status.count++;
                status.consecFailures = 0;

                // Processing of the measurement (graphing, stats etc) was moved
                // to the displayTimer so we could collect data from multiple 
                // instruments at higher speeds than a low-end laptop could 
                // process and graph it.  No need to do anything with "spectrum",
                // it's already cached in Spectrometer.lastSpectrum.
                //
                // buttonStart.BeginInvoke((MethodInvoker)delegate { processSpectrum(sn, spectrum); });

                if (worker.CancellationPending)
                    break;

                performExtraReads(spec);

                if (serializeSpecs)
                    specMut.ReleaseMutex();

                var iterDelayMS = r.Next(args.iterDelayMin, args.iterDelayMax);
                logger.debug($"sleeping {iterDelayMS} before next iteration");
                Thread.Sleep(iterDelayMS);
            }

            logger.info("worker closing");
            e.Result = sn;
        }

        void performExtraReads(Spectrometer spec)
        {
            const int EXTRA_READ_TYPES = 41;
            for (int i = 0; i < args.extraReads; i++)
            {
                int type = -1;
                if (forceExtraReadSequence == null)
                    type = r.Next(EXTRA_READ_TYPES);
                else
                    type = forceExtraReadSequence[forceExtraReadIndex++ % forceExtraReadSequence.Length];
                logger.debug("performing extraRead {0} ({1} of {2})", type, i + 1, args.extraReads);
                switch (type)
                {
                    case 0: logger.debug("extraRead: actualFrames = {0}", spec.actualFrames); break;
                    case 1: logger.debug("extraRead: actualIntegrationTimeUS = {0}", spec.actualIntegrationTimeUS); break;
                    case 2: logger.debug("extraRead: primaryADC = 0x{0:x4}", spec.primaryADC); break;
                    case 3: logger.debug("extraRead: secondaryADC = 0x{0:x4}", spec.secondaryADC); break;
                    case 4: logger.debug("extraRead: batteryCharging = {0}", spec.batteryCharging); break;
                    case 5: logger.debug("extraRead: batteryPercentage = {0}", spec.batteryPercentage); break;
                    case 6: logger.debug("extraRead: continuousAcquisitionEnable = {0}", spec.continuousAcquisitionEnable); break;
                    case 7: logger.debug("extraRead: continuousFrames = {0}", spec.continuousFrames); break;
                    case 8: logger.debug("extraRead: detectorGain = {0:f2}", spec.detectorGain); break;
                    case 9: logger.debug("extraRead: detectorOffset = {0}", spec.detectorOffset); break;
                    case 10: logger.debug("extraRead: detectorSensingThreshold = {0}", spec.detectorSensingThreshold); break;
                    case 11: logger.debug("extraRead: detectorSensingThresholdEnabled = {0}", spec.detectorSensingThresholdEnabled); break;
                    case 12: logger.debug("extraRead: detectorTECEnabled = {0}", spec.detectorTECEnabled); break;
                    case 13: logger.debug("extraRead: detectorTECSetpointRaw = 0x{0:x4}", spec.detectorTECSetpointRaw); break;
                    case 14: logger.debug("extraRead: firmwareRevision = {0}", spec.firmwareRevision); break;
                    case 15: logger.debug("extraRead: fpgaRevision = {0}", spec.fpgaRevision); break;
                    case 16: logger.debug("extraRead: highGainModeEnabled = {0}", spec.highGainModeEnabled); break;
                    case 17: logger.debug("extraRead: horizontalBinning = {0}", spec.horizontalBinning); break;
                    case 18: logger.debug("extraRead: integrationTimeMS = {0}", spec.integrationTimeMS); break;
                    case 19: logger.debug("extraRead: laserEnabled = {0}", spec.laserEnabled); break;
                    case 20: logger.debug("extraRead: laserModulationEnabled = {0}", spec.laserModulationEnabled); break;
                    case 21: logger.debug("extraRead: laserInterlockEnabled = {0}", spec.laserInterlockEnabled); break;
                    case 22: logger.debug("extraRead: laserModulationLinkedToIntegrationTime = {0}", spec.laserModulationLinkedToIntegrationTime); break;
                    case 23: logger.debug("extraRead: laserModulationPulseDelay = {0}", spec.laserModulationPulseDelay); break;
                    case 24: logger.debug("extraRead: laserModulationPulseWidth = {0}", spec.laserModulationPulseWidth); break;
                    case 25: logger.debug("extraRead: laserModulationDuration = {0}", spec.laserModulationDuration); break;
                    case 26: logger.debug("extraRead: laserModulationPeriod = {0}", spec.laserModulationPeriod); break;
                    case 27: logger.debug("extraRead: laserTemperatureDegC = {0}", spec.laserTemperatureDegC); break;
                    case 28: logger.debug("extraRead: laserTemperatureSetpointRaw = {0}", spec.laserTemperatureSetpointRaw); break;
                    case 29: logger.debug("extraRead: lineLength = {0}", spec.lineLength); break;
                    case 30: logger.debug("extraRead: optAreaScan = {0}", spec.optAreaScan); break;
                    case 31: logger.debug("extraRead: optActualIntegrationTime = {0}", spec.optActualIntegrationTime); break;
                    case 32: logger.debug("extraRead: optCFSelect = {0}", spec.optCFSelect); break;
                    case 33: logger.debug("extraRead: optDataHeaderTag = {0}", spec.optDataHeaderTag); break;
                    case 34: logger.debug("extraRead: optHorizontalBinning = {0}", spec.optHorizontalBinning); break;
                    case 35: logger.debug("extraRead: optIntegrationTimeResolution = {0}", spec.optIntegrationTimeResolution); break;
                    case 36: logger.debug("extraRead: optLaserControl = {0}", spec.optLaserControl); break;
                    case 37: logger.debug("extraRead: optLaserType = {0}", spec.optLaserType); break;
                    case 38: logger.debug("extraRead: triggerSource = {0}", spec.triggerSource); break;
                    case 39: logger.debug("extraRead: triggerOutput = {0}", spec.triggerOutput); break;
                    case 40: logger.debug("extraRead: triggerDelay = {0}", spec.triggerDelay); break;
                    default: break;
                }
            }
        }

        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sn = e.Result as string;
            var state = states[sn];
            var status = state.status;

            status.running = false;
            logger.debug($"{sn} worker complete");

            state.metrics.report();

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
                displayTimer.Stop();

                if (shutdownInProgress || args.autoStart)
                {
                    shutdownInProgress = true;
                    Thread.Sleep(1000);
                    Close();
                }
            }
        }
    }
}
