using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class CheckinArea
    {
        public CheckinArea(int counterAmount)
        {
            Counters = new Counter[counterAmount];
            Clear();
        }

        public Counter[] Counters { get; private set; }

        internal void Clear()
        {
            for (int i = 0; i < Counters.Length; i++)
            {
                Counters[i] = new Counter(id: i);
            }
        }
        internal void Checkin(Luggage luggage)
        {
            Counter counter = Counters.FirstOrDefault(c => c.Id == luggage.CounterId);
            counter.CheckLuggageIn(luggage);
            luggage.Reservation.IsCheckedIn = true;
        }
        internal void UpdateAutoCheckin()
        {
            // Finds all counters there is open
            foreach (Counter counter in Counters)
            {
                // When counter is open and are not occupied by baggage
                if (counter.IsOpen && !counter.IsLuggageSlotAvailable())
                {
                    // Get reservation if not checked in
                    Reservation reservation = counter.Flight.Reservations.Find(r => !r.IsCheckedIn);

                    // Check luggage in if valid
                    if (reservation != null)
                    {
                        Checkin(new Luggage(reservation.Flight.Terminal.Id, counter.Id, reservation));
                    }
                }
            }
        }
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
                    counter.UpdateFlight(flight);
                    counter.Open();
                    continue;
                }
            }
        }
        internal void CloseExpiredCounters()
        {
            // Closes counter if checkin period is done
            foreach (Counter counter in Counters)
            {
                if (!counter.Flight.IsReadyForCheckIn())
                {
                    counter.Close();
                }
                if (counter.Flight.GetCheckinAmount() == counter.Flight.Reservations.Count)
                {
                    counter.Close();
                }
            }
        }
    }
}
