using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public static class AutoGenerator
    {
        //public AutoGenerator(Simulator simulator)
        //{
        //    Simulator = simulator;
        //}

        private static Random rng = new Random();

        //public Simulator Simulator { get; private set; }

        public static Flight CreateRandomFlight(FlightSchedule flightSchedule)
        {
            // Creates arrival based on lates depature times in flight scheduler
            DateTime arrival = flightSchedule.Time.DateTime.AddMinutes(rng.Next(0, 60));
            if (flightSchedule.Flights.Count > 0)
            {
                arrival = flightSchedule.Flights.OrderByDescending(f => f.Arrival)
                    .FirstOrDefault().Arrival;
                arrival = arrival.AddMinutes(rng.Next(0, 60));
            }

            return new Flight(
                name: $"F-{rng.Next(0, 10000)}",
                seatsAmount: rng.Next(100, 300),
                arrival: arrival,
                departure: arrival.AddMinutes(60),
                destination: Data.GetRandomCity()
            );
        }
        public static Reservation CreateRandomReservation(FlightSchedule flightSchedule)
        {
            string firstName = Data.GetRandomName();

            Passenger passenger = new Passenger(
                firstName: firstName,   
                lastName: Data.GetRandomName(),
                email: firstName + rng.Next(1, 10000) + "@gmail.com",
                phoneNumber: "+45" + rng.Next(10000000, 99999999),
                address: $"{Data.GetRandomCity()} {Data.GetRandomStreet()} {rng.Next(1, 1000)}"
            );
            List<Flight> flights = flightSchedule.Flights.FindAll(
                f => f.Status == FlightStatus.OpenForReservation && f.IsSeatsAvailible());

            if (flights.Count > 0)
            {
                return new Reservation(
                    passenger: passenger,
                    flight: flights[rng.Next(0, flights.Count)]
                );
            }
            else
            {
                return null;
            }
        }
    }
}
