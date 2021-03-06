using System;
using System.Collections.Generic;

namespace BaggageSorteringLib
{
    public class Flight
    {
        public Flight(string name, int seatsAmount, DateTime arrival, DateTime departure, string destination)
        {
            Name = name;
            Terminal = null;
            Arrival = arrival;
            Departure = departure;
            Destination = destination;
            Status = FlightStatus.OpenForReservation;
            SeatsAmount = seatsAmount;
            Reservations = new List<Reservation>();
            Luggages = new Queue<Luggage>();
        }

        public static Flight None = new Flight("", 0, DateTime.MinValue, DateTime.MinValue, "");

        public string Name { get; private set; }
        public Terminal Terminal { get; private set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public int SeatsAmount { get; private set; }
        public List<Reservation> Reservations { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public MessageEvent FlightInfo { get; set; }
        public MessageEvent BadFlightInfo { get; set; }

        public bool IsSeatsAvailible()
        {
            return Reservations.Count < SeatsAmount;
        }
        public bool IsReadyForCheckIn()
        {
            return
                Status == FlightStatus.OnTheWay ||
                Status == FlightStatus.Landing;
        }
        public int GetCheckinAmount()
        {
            return Reservations.FindAll(r => r.IsCheckedIn).Count;
        }

        internal void AddReservation(Reservation reservation)
        {
            Reservations.Add(reservation);
        }
        internal void MoveToTerminal(Terminal terminal)
        {
            Terminal = terminal;
        }
        internal void LoadWithLuggages(Queue<Luggage> luggages)
        {
            Luggages = luggages;
        }
        internal void UpdateFlightStatus(SimulationTime time)
        {
            double timeToTakeoff = Departure.Subtract(time.DateTime).TotalMinutes;

            if (Status != FlightStatus.OpenForReservation && Reservations.Count < 20)
            {
                Status = FlightStatus.Canceled;
            }

            GotoNextStatus(timeToTakeoff, 360, 900, FlightStatus.FarAway);
            GotoNextStatus(timeToTakeoff, 70, 360, FlightStatus.OnTheWay);
            GotoNextStatus(timeToTakeoff, 60, 70, FlightStatus.Landing);
            GotoNextStatus(timeToTakeoff, 30, 60, FlightStatus.Refilling);
            GotoNextStatus(timeToTakeoff, 5, 30, FlightStatus.Boarding);
            if (GotoNextStatus(timeToTakeoff, 0, 5, FlightStatus.Takeoff))
            {
                if (GetCheckinAmount() == Reservations.Count)
                {
                    FlightInfo?.Invoke($"{Name} is about to takeoff with all booked passengers");
                }
                else
                {
                    BadFlightInfo?.Invoke($"{Name} is about to takeoff with missing passengers due to bustle");
                }
            }
        }
        private bool GotoNextStatus(double timeToTakeoff, int minPeriod, int maxPeriod, FlightStatus flightStatus)
        {
            if (flightStatus == FlightStatus.Canceled)
            {
                return false;
            }

            if ((timeToTakeoff > minPeriod) && (timeToTakeoff < maxPeriod))
            {
                if (Status != flightStatus)
                {
                    Status = flightStatus;
                    return true;
                }
            }
            return false;
        }
    }
}
