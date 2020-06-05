using WasatchNET;

namespace CrashTestNET
{
    // These are the fields of the DataGridView table shown below the Chart.
    class SpectrometerStatus
    {
        ////////////////////////////////////////////////////////////////////////
        // Properties are auto-rendered to the DataGridView
        ////////////////////////////////////////////////////////////////////////

        public string serial { get => spec.serialNumber; }
        public string model { get => spec.model; }
        public string FW { get => spec.firmwareRevision; }
        public string FPGA { get => spec.fpgaRevision; }
        public uint integTimeMS { get => spec.integrationTimeMS; }
        public float detTempDegC { get => spec.lastDetectorTemperatureDegC; }

        public bool running { get; set; }
        public int count { get; set; }
        public int readFailures { get; set; }
        public int shifts { get; set; }
        public int consecFailures { get; set; }

        Spectrometer spec; 

        public SpectrometerStatus(Spectrometer spec)
        {
            this.spec = spec;
            reset();
        }

        public void reset()
        {
            shifts = 0;
            count = 0;   
            readFailures = 0;
            consecFailures = 0;

            // leftPeakMean = leftPeakStdev = rightPeakMean = rightPeakStdev = 0;
        }
    }
}
