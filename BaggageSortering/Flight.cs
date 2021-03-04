using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Flight
    {
        public Flight(string name, int seatAmount, Terminal terminal, DateTime arrival, DateTime departure, string destination)
        {
            Name = name;
            Terminal = terminal;
            Arrival = arrival;
            Departure = departure;
            Destination = destination;
            Luggages = new Queue<Luggage>();
            Seats = new string[seatAmount];
            GenerateSeatNames();
        }

        public string Name { get; private set; }
        public Terminal Terminal { get; private set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public string[] Seats { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public List<Reservation> Reservations { get; private set; }

        public override string ToString()
        {
            return $"{Departure.ToString("HH:mm")}  {Destination}  {Name}  {Terminal.Id}";
        }

        private void GenerateSeatNames()
        {
            for (int i = 0; i < Seats.Length; i++)
            {
                int y = i / 6;
                int x = i % 6;

                Seats[i] = $"{y}-{(char)('A' + x)}";
            }
        }
    }
}
