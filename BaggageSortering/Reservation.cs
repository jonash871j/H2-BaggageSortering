using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Reservation
    {
        public Reservation(Passenger passenger, Flight flight, string seat)
        {
            Passenger =  passenger;
            Flight = flight;
            Seat = seat;
        }

        public Passenger Passenger { get; private set; }
        public Flight Flight { get; private set; }
        public string Seat { get; private set; }
    }
}
