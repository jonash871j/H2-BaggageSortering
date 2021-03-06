using System;
using System.Collections.Generic;

namespace BaggageSorteringLib
{
    public enum FlightStatus
    {
        OpenForReservation,
        FarAway,
        OnTheWay,
        Landing,
        Refilling,
        Boarding,
        Takeoff,
        Canceled,
    }

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
        public Terminal Terminal { get; set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public int SeatsAmount { get; private set; }
        public List<Reservation> Reservations { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }

        public void AddReservation(Reservation reservation)
        {
            Reservations.Add(reservation);
        }

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

        internal void UpdateFlightStatus(SimulationTime time)
        {
            double min = Departure.Subtract(time.DateTime).TotalMinutes;

            if (min > 900)
            {
                Status = FlightStatus.OpenForReservation;
            }
            else if (min > 360)
            {
                Status = FlightStatus.FarAway;
            }
            else if (min > 70)
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
            }
            else
            {
                Status = FlightStatus.Takeoff;
            }

            if (Status != FlightStatus.OpenForReservation && Reservations.Count < 20)
            {
                Status = FlightStatus.Canceled;
            }
        }

        public void LoadWithLuggages(Queue<Luggage> luggages)
        {
            Luggages = luggages;
        }
    }
}
