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

        public SimulationTime Time { get; private set; }
        public int FlightScreenLength { get; private set; }
        public List<Flight> FlightScreen { get; private set; }
        public List<Flight> Flights { get; private set; }

        public MessageEvent NewReservation;

        public void AddFlight(Flight flight)
        {
            Flights.Add(flight);
        }
        public void AddReservation(Reservation reservation)
        {
            if (reservation.Flight.Status == FlightStatus.OpenForReservation)
            {
                reservation.Flight.AddReservation(reservation);
                NewReservation?.Invoke($"{reservation.Passenger.FirstName} has booked a ticket to {reservation.Flight.Destination}");
            }
            else
            {
                // Reservations for current flight is closed
            }
        }

        public void UpdateStatuses()
        {
            for (int i = 0; i < Flights.Count; i++)
            {
                Flights[i].UpdateFlightStatus(Time);
            }
        }
        public void RemoveOldFlights()
        {
            if (Flights.Count > 0)
            {
                Flights.RemoveAll(f => f.Departure < Time.DateTime);
            }
        }
        public void GenerateRandomFlights()
        {
            while (Flights.Count < 1000)
            {
                AddFlight(AutoGenerator.CreateRandomFlight(this));
            }
        }
        public void GenerateRandomReservations()
        {
            for (int i = 0; i < 100; i++)
            {
                Reservation reservation = AutoGenerator.CreateRandomReservation(this);

                if (reservation != null)
                {
                    AddReservation(reservation);
                }
            }
        }

        public void UpdateFlightScreen()
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
