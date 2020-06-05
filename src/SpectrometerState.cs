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
    // Things associated with a single spectrometer.  The "Status" record is
    // basically what gets displayed on the DataGridView.
    class SpectrometerState
    {
        public Spectrometer spec;
        public BackgroundWorker worker;
        public Series series;
        public SpectrometerStatus status;
        public Metrics metrics;

        public SpectrometerState(Spectrometer spec)
        {
            this.spec = spec;
            status = new SpectrometerStatus(spec);
            metrics = new Metrics(this);
        }

        public void reset()
        {
            status.reset();
            metrics.reset();
        }
    }
}
