using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{


    public class SimulationTime
    {
        public SimulationTime()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTick;

            DateTime = DateTime.Now;
            Speed = 1;
        }

        private readonly System.Timers.Timer timer;
        private int _speed;
        private bool isTimeStopRequested = false;

        public DateTime DateTime { get; private set; }
        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                timer.Interval = 1000 / _speed;
            }
        }
        internal TimeUpdateEvent TimeUpdate { get; set; }

        internal void Start()
        {
            timer.Enabled = true;
        }
        internal void Stop()
        {
            isTimeStopRequested = true;
            while (timer.Enabled)
            {
                Thread.Sleep(1);
            }
            DateTime = DateTime.Now;
        }

        private void OnTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(this))
            {
                TimeUpdate.Invoke();
                DateTime = DateTime.AddMinutes(1);
                Monitor.PulseAll(this);
                Monitor.Exit(this);
            }

            if (isTimeStopRequested)
            {
                timer.Enabled = false;
                isTimeStopRequested = false;
            }
        }
    }
}
