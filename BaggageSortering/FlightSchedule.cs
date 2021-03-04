using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class FlightSchedule
    {
        public FlightSchedule(int flightScreenLength)
        {
            FlightScreenLength = flightScreenLength;
            FlightScreen = new List<Flight>();
            Flights = new List<Flight>();
        }

        public int FlightScreenLength { get; private set; }
        public List<Flight> FlightScreen { get; private set; }
        public List<Flight> Flights { get; private set; }

        public void AddFlight(Flight flight)
        {
            Flights.Add(flight);
        }
        public void AddReservation(Reservation reservation)
        {
            Flights.First(x => x.Name == reservation.Flight.Name)
                .Reservations.Add(reservation);
        }
        public void RemoveOldFlights()
        {
            if (Flights.Count > 0)
            {
                Flights.RemoveAll(f => f.Departure < Simulator.Time);
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
