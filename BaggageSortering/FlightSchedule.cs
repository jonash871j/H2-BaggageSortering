using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class FlightSchedule
    {
        public FlightSchedule(int flightScreenLenght)
        {
            FlightScreen = new Flight[flightScreenLenght];
            Flights = new List<Flight>();
        }

        public Flight[] FlightScreen { get; private set; }
        public List<Flight> Flights { get; private set; }

        public void AddFlightDepature(Flight flight)
        {
            Flights.Add(flight);
        }
        public void AddReservation(Reservation reservation)
        {
            Flights.First(x => x.Name == reservation.Flight.Name)
                .Reservations.Add(reservation);
        }
        public void RemoveOldFlightDepartures()
        {
            Flights.RemoveAll(f => f.Departure > Simulator.Time);
        }
    }
}
