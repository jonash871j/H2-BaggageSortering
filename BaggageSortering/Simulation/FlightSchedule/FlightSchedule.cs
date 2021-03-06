using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MessageEvent ReservationInfo;
        public MessageEvent AutoReservationsInfo;
        public MessageEvent FlightInfo;
        public MessageEvent BadFlightInfo;

        internal void Clear()
        {
            FlightScreen.Clear();
            Flights.Clear();
        }

        internal void AddFlight(Flight flight)
        {
            Flights.Add(flight);
        }
        internal void AddReservation(Reservation reservation)
        {
            if (reservation.Flight.Status == FlightStatus.OpenForReservation)
            {
                reservation.Flight.AddReservation(reservation);
                ReservationInfo?.Invoke($"{reservation.Passenger.FirstName} has booked a ticket to {reservation.Flight.Destination}");
            }
            else
            {
                throw new Exception("Reservation failed, flight is full!");
            }
        }

        internal void UpdateStatuses()
        {
            for (int i = 0; i < Flights.Count; i++)
            {
                Flights[i].UpdateFlightStatus(Time);
            }
        }
        internal void RemoveOldFlights()
        {
            if (Flights.Count > 0)
            {
                Flights.RemoveAll(f => f.Departure < Time.DateTime);
            }
        }

        internal void GenerateRandomFlights(int bustleLevel, bool isPreBookedWithRandomAmount)
        {
            if (bustleLevel <= 0)
            {
                bustleLevel = 1;
            }

            while (Flights.Count < 1000)
            {
                DateTime startArrival = Time.DateTime;

                if (Flights.Count > 0)
                {
                    startArrival = Flights.OrderByDescending(f => f.Arrival).FirstOrDefault().Arrival;
                }

                Flight flight = AutoGenerator.CreateRandomFlight(startArrival, 0, 600 / bustleLevel);

                if (isPreBookedWithRandomAmount)
                {
                    int minSeats = bustleLevel * 10;
                    if (minSeats > flight.SeatsAmount)
                    {
                        minSeats = flight.SeatsAmount;
                    }
                    int amount = rng.Next(minSeats, flight.SeatsAmount);

                    for (int i = 0; i < amount; i++)
                    {
                        AddReservation(AutoGenerator.CreateRandomReservation(flight));
                    }
                    AutoReservationsInfo?.Invoke($"{amount} auto generated people has made a reservation on {flight.Name}");
                }

                flight.FlightInfo = FlightInfo;
                flight.BadFlightInfo = BadFlightInfo;
                AddFlight(flight);
            }
        }

        internal void UpdateFlightScreen()
        {
            List<Flight> flights = Flights.OrderBy(x => x.Departure).ToList();
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
