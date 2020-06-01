using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using WasatchNET;

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
        int extraReads = 0;
        bool explicitSWTrigger = false;

        Random r = new Random();
        DateTime stopTime = DateTime.Now;

        bool running = false;
        bool shutdownInProgress = false;
        bool autoStart = false;
        bool initialized = false;

        SortedDictionary<string, SpectrometerState> states = new SortedDictionary<string, SpectrometerState>();
        BindingSource statusSource;

        int[] forceExtraReadSequence = null; // { 7, 30, 2, 1, 30 };
        int forceExtraReadIndex = 0;

        WasatchNET.Logger logger = WasatchNET.Logger.getInstance();

        public Form1(string[] args)
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
            extraReads = (int)numericUpDownExtraReads.Value;

            Text = string.Format("CrashTestNET {0}", 
                Assembly.GetExecutingAssembly().GetName().Version.ToString());

            // minimal command-line support
            foreach (var arg in args)
            {
                if (arg == "--start")
                    autoStart = true;
                else if (arg == "--debug" || arg == "--verbose")
                    logger.level = LogLevel.DEBUG;
                else if (arg == "--help")
                    Console.WriteLine("Usage: CrashTestNET.exe [--debug] [--start]");
            }

            if (autoStart)
               Task.Delay(1000).ContinueWith(t => buttonInit_Click(null, null));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (running)
            {
                shutdownInProgress = true;
                foreach (var pair in states)
                    pair.Value.worker.CancelAsync();
                e.Cancel = true;
                return;
            }

            Driver.getInstance().closeAllSpectrometers();
            logger.setTextBox(null);
        }

        void buttonInit_Click(object sender, EventArgs e)
        {
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

            for (int i = 0; i < specCount; i++)
            {
                var spec = driver.getSpectrometer(i);
                var sn = spec.serialNumber;
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

            var bindingList = new BindingList<SpectrometerStatus>();
            foreach (var pair in states)
                bindingList.Add(pair.Value.status);
            statusSource = new BindingSource(bindingList, null);
            dgvStatus.DataSource = statusSource;

            // update settings
            updateReadDelays();
            checkBoxIntegThrowaways_CheckedChanged(null, null);

            groupBoxTestControl.Enabled = true;

            initialized = true;
            if (autoStart)
                buttonStart_Click(null, null);
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
            logger.level = (sender as CheckBox).Checked 
                         ? WasatchNET.LogLevel.DEBUG 
                         : WasatchNET.LogLevel.INFO;

        void numericUpDownTestMinutes_ValueChanged(object sender, EventArgs e)
        {
            testDurationMin = (int)(sender as NumericUpDown).Value;
            stopTime = DateTime.Now.AddMinutes(testDurationMin);
        }

        void numericUpDownIntegTimeMin_ValueChanged(object sender, EventArgs e) =>
            integTimeMin = (int)(sender as NumericUpDown).Value;

        void numericUpDownIntegTimeMax_ValueChanged(object sender, EventArgs e) =>
            integTimeMax = (int)(sender as NumericUpDown).Value;

        private void numericUpDownIterDelayMin_ValueChanged(object sender, EventArgs e) =>
            iterDelayMin = (int)(sender as NumericUpDown).Value;

        private void numericUpDownIterDelayMax_ValueChanged(object sender, EventArgs e) =>
            iterDelayMax = (int)(sender as NumericUpDown).Value;

        void numericUpDownReadDelayMin_ValueChanged(object sender, EventArgs e)
        {
            readDelayMin = (int)(sender as NumericUpDown).Value;
            updateReadDelays();
        }

        void numericUpDownReadDelayMax_ValueChanged(object sender, EventArgs e)
        {
            readDelayMax = (int)(sender as NumericUpDown).Value;
            updateReadDelays();
        }

        private void checkBoxIntegThrowaways_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBoxIntegThrowaways.Checked;
            foreach (var pair in states)
                pair.Value.spec.throwawayAfterIntegrationTime = enabled;
        }

        private void numericUpDownExtraReads_ValueChanged(object sender, EventArgs e) =>
            extraReads = (int)(sender as NumericUpDown).Value;

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

        void updateReadDelays()
        {
            // if there is any read delay at all, we must explicitly send the SW
            // trigger, wait, then perform the read
            explicitSWTrigger = readDelayMin != 0 || readDelayMax != 0;

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

                logger.header($"{sn} iteration {state.status.acquisitions}");

                // randomize integration time
                var integMS = (uint)r.Next(integTimeMin, integTimeMax);
                logger.debug($"randomizing integration time to {integMS}");
                spec.integrationTimeMS = integMS;

                ////////////////////////////////////////////////////////////////
                // take acquisition
                ////////////////////////////////////////////////////////////////

                logger.debug("taking measurement");
                if (explicitSWTrigger)
                {
                    spec.sendSWTrigger();
                    var delayMS = r.Next(readDelayMin, readDelayMax);
                    logger.debug($"waiting {delayMS} between trigger and read");
                    Thread.Sleep(delayMS);
                }
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

                performExtraReads(spec);

                var iterMS = r.Next(iterDelayMin, iterDelayMax);
                logger.debug($"sleeping {iterMS} before next iteration");
                Thread.Sleep(iterMS);
            }

            logger.info("worker closing");
            e.Result = sn;
        }

        void performExtraReads(Spectrometer spec)
        {
            const int EXTRA_READ_TYPES = 41;
            for (int i = 0; i < extraReads; i++)
            {
                int type = -1;
                if (forceExtraReadSequence == null)
                    type = r.Next(EXTRA_READ_TYPES);
                else
                    type = forceExtraReadSequence[forceExtraReadIndex++ % forceExtraReadSequence.Length];
                logger.debug("performing extraRead {0} ({1} of {2})", type, i + 1, extraReads);
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
                if (shutdownInProgress || autoStart)
                    Close();
            }
        }
    }
}
