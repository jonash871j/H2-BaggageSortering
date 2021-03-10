using System;
using System.Threading;

namespace BaggageSorteringLib
{
    public class SimulationTime
    {
        public SimulationTime()
        {
            Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += OnTick;

            DateTime = DateTime.Now;
            Speed = 1;
        }

        private readonly System.Timers.Timer Timer;
        private int _speed;

        public DateTime DateTime { get; private set; }
        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                Timer.Interval = 1000 / _speed;
            }
        }
        internal UpdateEvent TimeUpdate { get; set; }

        /// <summary>
        /// Used to start time
        /// </summary>
        internal void Start()
        {
            Timer.Enabled = true;
        }

        /// <summary>
        /// Used to stop time, caused a small delay in thread!
        /// </summary>
        internal void Stop()
        {
            Timer.Enabled = false;
            DateTime = DateTime.Now;
        }

        /// <summary>
        /// Event: is being called on each tick
        /// </summary>
        private void OnTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(this))
            {
                TimeUpdate.Invoke();
                DateTime = DateTime.AddSeconds(15);
                Monitor.PulseAll(this);
                Monitor.Exit(this);
            }
        }
    }
}
