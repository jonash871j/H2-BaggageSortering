using System.Collections.Generic;
using System.Linq;

namespace BaggageSorteringLib
{
    public class CheckinArea
    {
        public CheckinArea(int counterAmount)
        {
            Counters = new Counter[counterAmount];

            for (int i = 0; i < Counters.Length; i++)
            {
                Counters[i] = new Counter(id: i);
            }
        }

        public Counter[] Counters { get; private set; }

        /// <summary>
        /// Used to clear terminals
        /// </summary>
        internal void Clear()
        {
            for (int i = 0; i < Counters.Length; i++)
            {
                Counters[i].Close();
            }
        }

        /// <summary>
        /// Used to check luggage in at terminal
        /// </summary>
        internal void Checkin(Luggage luggage)
        {
            Counter counter = Counters.FirstOrDefault(c => c.Id == luggage.CounterId);
            counter.Checkin(luggage);
            luggage.Reservation.IsCheckedIn = true;
        }

        /// <summary>
        /// Used to auto check in luggage at terminal
        /// </summary>
        internal void UpdateAutoCheckin()
        {
            // Finds all counters there is open
            foreach (Counter counter in Counters)
            {
                // When counter is open and are not occupied by baggage
                if (counter.IsOpen && !counter.IsLuggageReady())
                {
                    // Get reservation if not checked in
                    Reservation reservation = counter.Flight.Reservations.Find(r => !r.IsCheckedIn);

                    // Check luggage in if valid
                    if (reservation != null && reservation.Flight.Terminal != null)
                    {
                        Checkin(new Luggage(reservation.Flight.Terminal.Id, counter.Id, reservation));
                    }
                }
            }
        }

        /// <summary>
        /// Used to open counters for incomming flights
        /// </summary>
        internal void OpenCountersForIncommingFlights(FlightSchedule flightSchedule)
        {
            // Find flights there are ready for check in
            List<Flight> flights = flightSchedule.Flights.FindAll(f => f.IsReadyForCheckIn() == true);
            foreach (Flight flight in flights)
            {
                Counter counter = Counters.Where(c => c.IsOpen == false).FirstOrDefault();

                // If flight already has a counter, skip...
                if (Counters.Any(c => c.Flight == flight))
                {
                    continue;
                }

                // If counter is available
                // Update counter flight screen and open counter
                if (counter != null)
                {
                    counter.SetTargetedFlight(flight);
                    counter.Open();
                    continue;
                }
            }
        }

        /// <summary>
        /// Used to close expired counters
        /// </summary>
        internal void CloseExpiredCounters()
        {
            // Closes counter if checkin period is done
            foreach (Counter counter in Counters)
            {
                Flight flight = counter.Flight;

                // When flight is no longer ready for check in
                if (!flight.IsReadyForCheckIn())
                {
                    counter.Close();
                }
                // When check in amount is the same as the reservation amount
                if (flight.GetCheckinAmount() == flight.Reservations.Count)
                {
                    counter.Close();
                }
            }
        }
    }
}
