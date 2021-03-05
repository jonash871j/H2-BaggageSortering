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

            Counters = new Counter[10];
            Terminals = new Terminal[10];

            for (int i = 0; i < 10; i++)
            {
                Counters[i] = new Counter(i + 1);
                Terminals[i] = new Terminal(i + 1);
            }

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
            luggage.Reservation.IsCheckedIn = true;
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
                    while (FlightSchedule.Flights.Count < 1000)
                    {
                        FlightSchedule.AddFlight(autoGenerator.CreateRandomFlight());
                    }


                    // Creates 100 random reservations
                    for (int i = 0; i < 100; i++)
                    {
                        Reservation reservation = autoGenerator.CreateRandomReservation();
                        if (reservation != null)
                        {
                            AddReservation(reservation);
                        }
                    }

                    // 1. Finds all counters there is open
                    // 2. Checks people in if they are not
                    foreach (Counter counter in Counters)
                    {
                        if ((counter.IsOpen) && (!counter.IsLuggageSlotAvailable()))
                        {
                            Reservation reservation = counter.Flight.Reservations.Find(r => !r.IsCheckedIn);

                            if (reservation != null)
                            {
                                // TODO: Fix gate
                                CheckLuggageIn(counter.Id, new Luggage(reservation.Flight.TerminalId, counter.Id, reservation));
                            }
                        }
                    }
                }

                // Update flight scheduler
                FlightSchedule.UpdateStatuses(Time);
                FlightSchedule.RemoveOldFlights(Time);
                FlightSchedule.UpdateFlightScreen();
                
                
                AssignFlightGates();

                // Find flights there are ready for check in
                List<Flight> flights = FlightSchedule.Flights.FindAll(f => f.IsReadyForCheckIn() == true);
                foreach (Flight flight in flights)
                {
                    Counter counter = Counters.Where(c => c.IsOpen == false).FirstOrDefault();

                    // If flight already has a counter, skip...
                    if (Counters.Any(c => c.Flight == flight))
                    {
                        continue;
                    }

                    // If counter is available
                    // Update counter flight screen and open counter
                    if (counter != null)
                    {
                        counter.UpdateFlight(flight);
                        counter.Open();
                        continue;
                    }
                }

                // Closes counter if check in period is done
                foreach (Counter counter in Counters)
                {
                    if (!counter.Flight.IsReadyForCheckIn())
                    {
                        counter.Close();
                    }
                }

                // Update simulation time
                Time.IsUpdateCycle = false;
                Time.MoveTime();
            }
        }
        private void AssignFlightGates()
        {
            List<Flight> flights = FlightSchedule.Flights.FindAll(f => f.Status == FlightStatus.FarAway);
            foreach (Flight flight in flights)
            {
                if (flight.TerminalId == 0)
                {
                    Terminal terminal = Terminals.FirstOrDefault(t => t.Flight == null);
                    if (terminal != null)
                    {
                        terminal.Flight = flight;
                        flight.TerminalId = terminal.Id;
                    }
                }
            }
            foreach (Terminal terminal1 in Terminals)
            {
                if (terminal1.Flight != null)
                {
                    if (terminal1.Flight.Status == FlightStatus.Takeoff)
                    {
                        terminal1.Flight = null;
                        terminal1.Close();
                    }
                    else if (terminal1.Flight.Status == FlightStatus.Refilling && terminal1.Luggages.Count > 0)
                    {
                        terminal1.LoadFlightLuggages();
                    }
                }
            }
        }
    }
}
