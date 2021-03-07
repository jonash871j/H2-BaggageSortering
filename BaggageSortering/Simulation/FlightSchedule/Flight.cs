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

        private static readonly Random rng = new Random();
        public static Flight None = new Flight("", 0, DateTime.MinValue, DateTime.MinValue, "");

        public string Name { get; private set; }
        public int SeatsAmount { get; private set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public List<Reservation> Reservations { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public Terminal Terminal { get; private set; }
        public bool IsAtTerminal { get; private set; }

        public MessageEvent FlightInfo { get; set; }
        public MessageEvent BadFlightInfo { get; set; }

        /// <summary>
        /// Used to check if there is any seats available on the flight
        /// </summary>
        /// <returns>true is there are seats available</returns>
        public bool IsSeatsAvailible()
        {
            return Reservations.Count < SeatsAmount;
        }

        /// <summary>
        /// Used to check if flight is ready for the checkin period
        /// </summary>
        public bool IsReadyForCheckIn()
        {
            return
                Status == FlightStatus.OnTheWay ||
                Status == FlightStatus.Landing;
        }

        /// <summary>
        /// Used to get the amount of people who is checked in on the flight
        /// </summary>
        public int GetCheckinAmount()
        {
            return Reservations.FindAll(r => r.IsCheckedIn).Count;
        }

        /// <summary>
        /// Used to add reservation on flight
        /// </summary>
        internal void BookFlightTicket(Passenger passenger)
        {
            if (Status == FlightStatus.OpenForReservation)
            {
                Reservations.Add(new Reservation(passenger, this));
                FlightInfo?.Invoke($"{passenger.FirstName} has booked a ticket to {Destination}");
            }
            else
            {
                throw new Exception("Reservation failed, flight is full!");
            }
        }

        /// <summary>
        /// Used to auto generate reservations on flight
        /// </summary>
        /// <param name="bustleLevel"></param>
        internal void AutoBookFlightTickets(int minSeats)
        {
            // Limit min seats
            if (minSeats > SeatsAmount)
            {
                minSeats = SeatsAmount-1;
            }

            // Generates random amount of reservations
            int amount = rng.Next(minSeats, SeatsAmount);
            for (int i = 0; i < amount; i++)
            {
                BookFlightTicket(AutoGenerator.CreateRandomPassenger());
            }

            FlightInfo?.Invoke($"{amount} auto generated people has made a reservation on {Name}");
        }

        /// <summary>
        /// Used to move fligh to terminal
        /// </summary>
        internal void MoveToTerminal(Terminal terminal)
        {
            Terminal = terminal;
            IsAtTerminal = true;
        }

        /// <summary>
        /// Used to load flight storage with luggaes
        /// </summary>
        internal void LoadWithLuggages(Queue<Luggage> luggages)
        {
            Luggages = luggages;
        }

        /// <summary>
        /// Used to update flight statuses
        /// </summary>
        internal void UpdateFlightStatuses(SimulationTime time)
        {
            // Gets the time until takeoff
            double timeToTakeoff = Departure.Subtract(time.DateTime).TotalMinutes;

            // When reservations count is to small
            // Cancel the flight
            if (Status != FlightStatus.OpenForReservation && Reservations.Count < 20)
            {
                if (ChangeStatusIfNewer(FlightStatus.Canceled))
                {
                    BadFlightInfo?.Invoke($"{Name} got canceled due to reservation amount was < 20");
                }
            }

            // Updates the different flight states
            if (ChangeStatusInsidePeriod(timeToTakeoff, 360, 900, FlightStatus.FarAway))
            {
                FlightInfo?.Invoke($"{Name} is now closed for reservations");
            }
            if (ChangeStatusInsidePeriod(timeToTakeoff, 70, 360, FlightStatus.OnTheWay))
            {
                FlightInfo?.Invoke($"{Name} is 290 min from the airport");
            }
            if (ChangeStatusInsidePeriod(timeToTakeoff, 60, 70, FlightStatus.Landing))
            {
                FlightInfo?.Invoke($"{Name} has just landed");
            }
            if (ChangeStatusInsidePeriod(timeToTakeoff, 30, 60, FlightStatus.Refilling))
            {
                FlightInfo?.Invoke($"{Name} is being filled with luggages");
            }
            if (ChangeStatusInsidePeriod(timeToTakeoff, 5, 30, FlightStatus.Boarding))
            {
                FlightInfo?.Invoke($"{Name} is now boading");
            }
            if (ChangeStatusInsidePeriod(timeToTakeoff, 0, 5, FlightStatus.Takeoff))
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

        /// <summary>
        /// Used to change status when timeToTakeOff is inside of the given period
        /// </summary>
        /// <returns>true if status was changed</returns>
        private bool ChangeStatusInsidePeriod(double timeToTakeoff, int minPeriod, int maxPeriod, FlightStatus flightStatus)
        {
            if (Status == FlightStatus.Canceled)
            {
                return false;
            }

            if ((timeToTakeoff > minPeriod) && (timeToTakeoff < maxPeriod))
            {
                return ChangeStatusIfNewer(flightStatus);
            }
            return false;
        }

        /// <summary>
        /// Used to change status if status is not already set
        /// </summary>
        /// <returns>true if status was changed</returns>
        private bool ChangeStatusIfNewer(FlightStatus flightStatus)
        {
            if (Status != flightStatus)
            {
                Status = flightStatus;
                return true;
            }
            return false;
        }
    }
}
