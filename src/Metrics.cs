using System;
using System.Collections.Generic;
using System.Linq;
using WasatchNET;

namespace CrashTestNET
{
    /// <remarks>
    /// It's rarely a good idea to track ALL peaks.  We could track "5 highest 
    /// peaks," or "top peak per quadrant", or "top peak per half".  Leaning 
    /// toward "top per half" right now, as that should be enough to catch either
    /// "walking/shifting" or "swapped halves".
    /// </remarks>
    class Metrics
    {
        public bool enabled;

        public Metric left;    // e.g. pixels 0-1023
        public Metric right;   // e.g. pixels 1024-2047

        // public double mean { get; private set; }
        // public double stdev { get; private set; }
        // List<double> history = new List<double>();
        // double sum;

        Logger logger = Logger.getInstance();

        SpectrometerState state;

        public Metrics(SpectrometerState state)
        {
            this.state = state;

            left = new Metric(state, "left");
            right = new Metric(state, "right");
        }

        public void reset()
        {
            left.reset();
            right.reset();
        }

        public void report()
        {
            if (!enabled)
                return;

            logger.info($"[Metrics] {state.spec.serialNumber} shifts {state.status.shifts}");
        }

        public void process(double[] spectrum)
        {
            if (!enabled)
                return;

            // split smoothed spectrum in half, and track the highest point
            // in each half
            int len = spectrum.Length;
            var status = state.status;

            left.update(spectrum, 0, len / 2 - 1);
            right.update(spectrum, len / 2, len - 1);

            // for now, just track whether left or right is bigger
            if (left.value <= right.value)
                state.status.shifts++;

            // history.Add(value);
            // sum += value;
            // mean = history.Count > 1 ? sum / history.Count : sum;
            // stdev = history.Count > 1 ? Math.Sqrt(history.Average(v => Math.Pow(v - mean, 2))) : 0; ;
        }
    }

    /// <summary>
    /// Exactly what we're going to track here remains in discussion.
    /// The final version should probably track specific emission peaks,
    /// such as from an Xe or Ar lamp, or using Raman.
    /// 
    /// I tried simply tracking local maxima, and that wasn't sufficient;
    /// simple argon fluourescent room lights have multiple peaks that are
    /// too close in intensity to reliably discriminate between.
    /// 
    /// For the moment, I'm just going to use "area under the curve" as
    /// the metric of interest, as from a VISNIR, room lights are inarguably
    /// brighter in VIS than in NIR.
    /// </summary>
    class Metric
    {
        public double value { get; private set; }

        SpectrometerState state;
        string label;

        Logger logger = Logger.getInstance();

        public Metric(SpectrometerState state, string label)
        {
            this.state = state;
            this.label = label;
        }

        public void reset()
        {
            value = 0;
        }

        /// <summary>
        /// Updates the TrackedPeak with the x-coord of the highest pixel in the
        /// subspectrum between (first, last) pixels (indices) inclusive.
        /// </summary>
        public void update(double[] spectrum, int first, int last)
        {
            // we tried using local maxima, but that wasn't reliable
            // value = computePeakX(spectrum, first, last);

            // try using ratio of areas under the curve
            value = computeIntegral(spectrum, first, last);
        }

        double computeIntegral(double[] spectrum, int first, int last)
        {
            var wavelengths = state.spec.wavelengths;

            int len = last - first + 1;
            double[] x = new double[len];
            double[] y = new double[len];
            Array.Copy(wavelengths, first, x, 0, len); // cache?
            Array.Copy(spectrum,    first, y, 0, len);

            return WasatchMath.NumericalMethods.integrate(x, y, WasatchMath.IntegrationMethod.TRAPEZOIDAL);
        }

        double computePeakX(double[] spectrum, int first, int last)
        {
            var wavelengths = state.spec.wavelengths;

            // could use WasatchMath.SpectrumPeak and PeakFinding, but...honestly the literal
            // maxima is probably fine
            double peakX = wavelengths[first];
            double peakY = spectrum[first];
            for (int i = first + 1; i <= last; i++)
            {
                if (spectrum[i] > peakY)
                {
                    peakY = spectrum[i];
                    peakX = wavelengths[i];
                }
            }

            return peakX;
        }
    }
}
