using System;
using System.Collections.Generic;
using System.Text;

namespace Sooda.Utils
{
    class SDStopWatch : StopWatch
    {
        public System.Diagnostics.Stopwatch _stopwatch;

        public SDStopWatch()
        {
            _stopwatch = new System.Diagnostics.Stopwatch();
        }

        public override void Start()
        {
            _stopwatch.Start();
        }

        public override void Stop()
        {
            _stopwatch.Stop();
        }

        public override double Seconds
        {
            get { return (double)(_stopwatch.ElapsedMilliseconds) / System.Diagnostics.Stopwatch.Frequency; }
        }
    }
}
