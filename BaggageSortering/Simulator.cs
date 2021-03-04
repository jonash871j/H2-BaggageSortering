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
            Time = new SimulationTime();
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
            SortingMachine = new SortingMachine(Time, Counters, Terminals);
            FlightSchedule = new FlightSchedule(12);

            autoGenerator = new AutoGenerator(this);
        }
        
        private readonly Random rng = new Random();
        private readonly AutoGenerator autoGenerator;

        public SimulationTime Time { get; private set; }
        public Counter[] Counters { get; private set; }
        public Terminal[] Terminals { get; private set; }
        public SortingMachine SortingMachine { get; private set; }
        public FlightSchedule FlightSchedule { get; private set; }
        public bool IsAutoGenerationEnabled { get; set; }

        public void Start()
        {
            SortingMachine.Start();
            Update();
        }

        public void CheckLuggageIn(int counterId, Luggage luggage)
        {
            Counter counter = Counters.FirstOrDefault(c => c.Id == counterId);
            counter.CheckLuggageIn(luggage);
        }
        public void AddReservation(Reservation reservation)
        {
            FlightSchedule.AddReservation(reservation);
        }
        public void Update()
        {
            if (Time.IsUpdateCycle)
            {
                // Auto generate flights and reservations
                if (IsAutoGenerationEnabled)
                {
                    // Creates random flights
                    while (FlightSchedule.Flights.Count < 100)
                    {
                        FlightSchedule.AddFlight(autoGenerator.CreateRandomFlight());
                    }
                }

                // Update flight scheduler
                FlightSchedule.UpdateStatuses(Time);
                FlightSchedule.RemoveOldFlights(Time);
                FlightSchedule.UpdateFlightScreen();

                // Update simulation time
                Time.IsUpdateCycle = false;
                Time.MoveTime();
            }
        }
    }
}
