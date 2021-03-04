using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace BaggageSorteringLib
{
    public class Simulator
    {
        public Simulator()
        {
            Counters = new Counter[]
            {
                new Counter(1),
                new Counter(2),
                new Counter(3),
                new Counter(4),
                new Counter(5),
            };
            Terminals = new Terminal[]
            {
                new Terminal(1), 
                new Terminal(2), 
                new Terminal(3), 
                new Terminal(4), 
                new Terminal(5), 
            };
            SortingMachine = new SortingMachine(Counters, Terminals);
            FlightSchedule = new FlightSchedule(10);

            timer = new Timer(1000);
            timer.Elapsed += OnTick;
            timer.Enabled = true;
        }
        
        private readonly Random rng = new Random();
        private readonly Timer timer;

        public static DateTime Time { get; private set; }
        public Counter[] Counters { get; private set; }
        public Terminal[] Terminals { get; private set; }
        public SortingMachine SortingMachine { get; private set; }
        public FlightSchedule FlightSchedule { get; private set; }

        public void Start()
        {
            SortingMachine.Start();
        }

        public void CheckLuggageIn(int terminalId, Luggage luggage)
        {
            Terminal terminal = Terminals.FirstOrDefault(t => t.Id == terminalId);
        }

        public void CreateReservation(Reservation reservation)
        {
            
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            //if (FlightPlans.Count < 15)
            //{
            //    autoGenerator.GenerateFlightPlan();
            //}
        }
    }
}
