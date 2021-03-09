using System.Collections.Generic;
using System.Linq;

namespace BaggageSorteringLib
{
    public partial class Simulator
    {
        /// <summary>
        /// Used to add reservation
        /// </summary>
        public void BookFlightTicket(Passenger passenger, Flight flight)
        {
            flight.BookFlightTicket(passenger);
        }

        /// <summary>
        /// Used to add flight to flight schedule
        /// </summary>
        /// <param name="flight"></param>
        public void AddFlight(Flight flight)
        {
            FlightSchedule.AddFlight(flight);
        }
    }
}
