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
            FlightSchedule.AddFlight(new Flight("F-123", 400, Terminals[0], Time, Time.AddMinutes(15), "New York"));
            FlightSchedule.AddFlight(new Flight("F-123", 400, Terminals[0], Time, Time.AddMinutes(30), "New York"));
            FlightSchedule.AddFlight(new Flight("F-123", 400, Terminals[0], Time, Time.AddMinutes(45), "New York"));
            FlightSchedule.AddFlight(new Flight("SAS-1234", 400, Terminals[0], Time, Time.AddMinutes(60), "Copenhagen"));
            FlightSchedule.AddFlight(new Flight("F-123", 400, Terminals[0], Time, Time.AddMinutes(120), "New York"));
            FlightSchedule.AddFlight(new Flight("F-123", 400, Terminals[0], Time, Time.AddMinutes(160), "New York"));

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
        public void AddReservation(Reservation reservation)
        {
            FlightSchedule.AddReservation(reservation);
        }
        public void AddFlight(Flight flight)
        {
            FlightSchedule.AddFlight(flight);
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            Time = Time.AddMinutes(1);
            FlightSchedule.RemoveOldFlights();
            FlightSchedule.UpdateFlightScreen();
            //if (FlightPlans.Count < 15)
            //{
            //    autoGenerator.GenerateFlightPlan();
            //}
        }
    }
}
