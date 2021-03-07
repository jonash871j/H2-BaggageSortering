using System;
using System.Collections.Generic;
using System.Linq;

namespace BaggageSorteringLib
{
    public class FlightSchedule
    {
        public FlightSchedule(SimulationTime time, int flightScreenLength)
        {
            Time = time;
            FlightScreenLength = flightScreenLength;
            FlightScreen = new List<Flight>();
            Flights = new List<Flight>();
        }

        private static Random rng = new Random();

        public SimulationTime Time { get; private set; }
        public int FlightScreenLength { get; private set; }
        public List<Flight> FlightScreen { get; private set; }
        public List<Flight> Flights { get; private set; }

        public MessageEvent AutoReservationsInfo;
        public MessageEvent FlightInfo;
        public MessageEvent BadFlightInfo;

        /// <summary>
        /// Used to clear flight schedule
        /// </summary>
        internal void Clear()
        {
            FlightScreen.Clear();
            Flights.Clear();
        }

        /// <summary>
        /// Used to add flight to schedule
        /// </summary>
        internal void AddFlight(Flight flight)
        {
            Flights.Add(flight);
        }

        /// <summary>
        /// Used to update flight statuses
        /// </summary>
        internal void UpdateStatuses()
        {
            for (int i = 0; i < Flights.Count; i++)
            {
                Flights[i].UpdateFlightStatuses(Time);
            }
        }

        /// <summary>
        /// Used to remove old flights from the schedule
        /// </summary>
        internal void RemoveOldFlights()
        {
            if (Flights.Count > 0)
            {
                Flights.RemoveAll(f => f.Departure < Time.DateTime);
            }
        }

        /// <summary>
        /// Used to generate random flights
        /// </summary>
        internal void GenerateRandomFlights(int bustleLevel, bool isAutoGenereatedReservationsEnabled)
        {
            while (Flights.Count < 1000)
            {
                DateTime startArrival = Time.DateTime;

                if (Flights.Count > 0)
                {
                    // Finds most far away flight arrival from now
                    startArrival = Flights.OrderByDescending(f => f.Arrival).FirstOrDefault().Arrival;
                }

                // Creates random flight
                Flight flight = AutoGenerator.CreateRandomFlight(
                    startArrival: startArrival, 
                    minArrival: 0, 
                    maxArrival: 600 / bustleLevel
                );

                // Auto generate flight ticket if enabled
                if (isAutoGenereatedReservationsEnabled)
                {
                    flight.AutoBookFlightTickets(bustleLevel * 10);
                }

                // Sets event refrences
                flight.FlightInfo = FlightInfo;
                flight.BadFlightInfo = BadFlightInfo;

                AddFlight(flight);
            }
        }

        /// <summary>
        /// Used to update flight screen with newest flight information
        /// </summary>
        internal void UpdateFlightScreen()
        {
            // Gets all flights ordered by the depature
            List<Flight> flights = Flights.OrderBy(x => x.Departure).ToList();
            
            // Clears flight screen
            FlightScreen.Clear();

            for (int i = 0; i < flights.Count; i++)
            {
                if (i >= FlightScreenLength)
                {
                    break;
                }
                else
                {
                    FlightScreen.Add(flights[i]);
                }
            }
        }
    }
}