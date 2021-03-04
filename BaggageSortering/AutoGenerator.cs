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

        public Flight CreateRandomFlight()
        {
            //Flight plan = new Flight(
            //    new Flight($"F-{rng.Next(0, 10000)}", rng.Next(100, 300)),
            //    Simulator.Terminals[rng.Next(0, Simulator.Terminals.Length)],
            //    DateTime.Now,
            //    DateTime.Now,
            //    AirportData.CityDestinations[rng.Next(0, AirportData.CityDestinations.Length)]
            //);
            return null;
        }
    }
}
