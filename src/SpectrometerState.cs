using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

using WasatchNET;

namespace CrashTestNET
{
    class SpectrometerState
    {
        public Spectrometer spec;
        public BackgroundWorker worker;
        public Series series;
        public SpectrometerStatus status;

        public SpectrometerState(Spectrometer spec)
        {
            this.spec = spec;
            status = new SpectrometerStatus(spec);
        }
    }
}
