using WasatchNET;

namespace CrashTestNET
{
    // These are the fields of the DataGridView table shown below the Chart.
    class SpectrometerStatus
    {
        public string serial { get => spec.serialNumber; }
        public string model { get => spec.model; }
        public string FW { get => spec.firmwareRevision; }
        public string FPGA { get => spec.fpgaRevision; }
        public uint integrationTimeMS { get => spec.integrationTimeMS; }
        public float detectorTempDegC { get => spec.lastDetectorTemperatureDegC; }

        public bool running { get; set; }
        public int acquisitions { get; set; }
        public int readFailures { get; set; }
        public int shifts { get; set; }
        public int consecutiveFailures { get; set; }

        Spectrometer spec;

        public SpectrometerStatus(Spectrometer spec)
        {
            this.spec = spec;
            reset();
        }

        public void reset()
        {
            shifts = 0;
            acquisitions = 0;   
            readFailures = 0;
            consecutiveFailures = 0;
        }
    }
}
