using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public enum FlightStatus
    {
        OnTheWay,
        Landing,
        Refilling,
        Boarding,
        Takeoff,
    }

    public class Flight
    {
        public Flight(string name, int seats, Terminal terminal, DateTime arrival, DateTime departure, string destination)
        {
            Name = name;
            Terminal = terminal;
            Arrival = arrival;
            Departure = departure;
            Destination = destination;
            Status = FlightStatus.OnTheWay;
            Seats = new string[seats];
            Luggages = new Queue<Luggage>();

            GenerateSeatNames();
        }

        public string Name { get; private set; }
        public Terminal Terminal { get; private set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public string[] Seats { get; private set; }

        public Queue<Luggage> Luggages { get; private set; }
        public List<Reservation> Reservations { get; private set; }

        private void GenerateSeatNames()
        {
            for (int i = 0; i < Seats.Length; i++)
            {
                int y = i / 6;
                int x = i % 6;

                Seats[i] = $"{y}-{(char)('A' + x)}";
            }
        }

        public void UpdateFlightStatus(SimulationTime time)
        {
            double min = Departure.Subtract(time.DateTime).TotalMinutes;

            if (min > 70)
            {
                Status = FlightStatus.OnTheWay;
            }
            else if(min > 60)
            {
                Status = FlightStatus.Landing;
            }
            else if (min > 30)
            {
                Status = FlightStatus.Refilling;
            }
            else if (min > 5)
            {
                Status = FlightStatus.Boarding;
                Terminal.Open();
            }
            else
            {
                Status = FlightStatus.Takeoff;
                Terminal.Close();
            }
        }
    }
}
