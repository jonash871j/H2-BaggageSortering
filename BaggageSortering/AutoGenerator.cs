using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class AutoGenerator
    {
        public AutoGenerator(Simulator simulator)
        {
            Simulator = simulator;
        }

        private static Random rng = new Random();

        public Simulator Simulator { get; private set; }

        public void UpdateFlightSchedule()
        {
            if (Simulator.FlightSchedule.Flights.Count < 20)
            {
                Simulator.AddFlight(CreateRandomFlight());
            }
        }

        public Flight CreateRandomFlight()
        {
            List<Flight> flights = Simulator.FlightSchedule.Flights;
            DateTime depature = Simulator.Time.AddMinutes(rng.Next(15, 60));

            if (flights.Count > 0)
            {
                depature = flights.OrderByDescending(f => f.Departure).FirstOrDefault().Departure;
                depature = depature.AddMinutes(rng.Next(0, 60));
            }

            Flight flight = new Flight(
                $"F-{rng.Next(0, 10000)}", rng.Next(100, 300),
                Simulator.Terminals[rng.Next(0, Simulator.Terminals.Length)],
                depature,
                depature.AddMinutes(rng.Next(120, 960)),
                AirportData.CityDestinations[rng.Next(0, AirportData.CityDestinations.Length)]
            );

            return flight;
        }
    }
}
