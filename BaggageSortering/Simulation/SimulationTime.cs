using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BaggageSorteringLib
{
    public class SimulationTime
    {
        public SimulationTime()
        {
            timer = new Timer(1000);
            timer.Elapsed += OnTick;
            timer.Enabled = true;

            IsUpdateCycle = true;
            DateTime = DateTime.Now;
            Speed = 1;
        }

        private readonly Timer timer;
        private int _speed;

        internal bool IsUpdateCycle { get; private set; } 
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

        public void MoveTime()
        {
            DateTime = DateTime.AddMinutes(1);
            IsUpdateCycle = false;
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            IsUpdateCycle = true;
        }
    }
}
