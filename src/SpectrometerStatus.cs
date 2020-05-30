using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasatchNET;

namespace CrashTestNET
{
    class SpectrometerStatus
    {
        Spectrometer spec;

        public string serial { get => spec.serialNumber; }
        public string model { get => spec.model; }
        public uint integrationTimeMS { get => spec.integrationTimeMS; }
        public float detectorTempDegC { get => spec.lastDetectorTemperatureDegC; }

        public bool running { get; set; }
        public int acquisitions { get; set; }
        public int readFailures { get; set; }
        public int shifts { get; set; }
        public int consecutiveFailures { get; set; }

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
