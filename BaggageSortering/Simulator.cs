using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace BaggageSorteringLib
{
    public class Simulator
    {
        public Simulator(bool isAutoGenerationEnabled)
        {
            Time = DateTime.Now;
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
            FlightSchedule = new FlightSchedule(12);
            IsAutoGenerationEnabled = isAutoGenerationEnabled;

            timer = new Timer(1000);
            timer.Elapsed += OnTick;
            timer.Enabled = true;

            autoGenerator = new AutoGenerator(this);
        }
        
        private int _speed = 1;
        private bool isUpdateCycle = false;
        private readonly Random rng = new Random();
        private readonly Timer timer;
        private readonly AutoGenerator autoGenerator;

        public static DateTime Time { get; private set; }
        public Counter[] Counters { get; private set; }
        public Terminal[] Terminals { get; private set; }
        public SortingMachine SortingMachine { get; private set; }
        public FlightSchedule FlightSchedule { get; private set; }
        public bool IsAutoGenerationEnabled { get; set; }
        public int Speed 
        {
            get => _speed;
            set
            {
                _speed = value;
                timer.Interval = 1000 / _speed;
            }
        }

        public void Start()
        {
            SortingMachine.Start();
        }

        public void CheckLuggageIn(int terminalId, Luggage luggage)
        {
            Terminal terminal = Terminals.FirstOrDefault(t => t.Id == terminalId);
        }
        public void AddReservation(Reservation reservation)
        {
            FlightSchedule.AddReservation(reservation);
        }
        public void AddFlight(Flight flight)
        {
            FlightSchedule.AddFlight(flight);
        }

        public void Update()
        {
            if (isUpdateCycle)
            {
                Time = Time.AddMinutes(1);
                FlightSchedule.RemoveOldFlights();
                FlightSchedule.UpdateFlightScreen();

                if (IsAutoGenerationEnabled)
                {
                    autoGenerator.UpdateFlightSchedule();
                }
                isUpdateCycle = false;
            }
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            isUpdateCycle = true;
        }
    }
}
