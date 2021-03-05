using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class Flight
    {
        public Flight(string name, int seats, DateTime arrival, DateTime departure, string destination)
        {
            Name = name;
            //Terminal = null;
            Arrival = arrival;
            Departure = departure;
            Destination = destination;
            Status = FlightStatus.OpenForReservation;
            Seats = new string[seats];
            Reservations = new List<Reservation>();
            Luggages = new Queue<Luggage>();

            GenerateSeatNames();
        }

        public static Flight None = new Flight("", 0, DateTime.MinValue, DateTime.MinValue, "");

        public string Name { get; private set; }
        //public Terminal Terminal { get; private set; }
        public DateTime Arrival { get; private set; }
        public DateTime Departure { get; private set; }
        public string Destination { get; private set; }
        public FlightStatus Status { get; private set; }
        public string[] Seats { get; private set; }
        public List<Reservation> Reservations { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }

        public void AddReservation(Reservation reservation)
        {
            Reservations.Add(reservation);
        }

        public bool IsSeatsAvailible()
        {
            return Reservations.Count < Seats.Length;
        }
        public bool IsReadyForCheckIn()
        {
            return 
                Status != FlightStatus.OpenForReservation && 
                Status != FlightStatus.Boarding && 
                Status != FlightStatus.Takeoff;
        }
        public int GetCheckinAmount()
        {
            return Reservations.FindAll(r => r.IsCheckedIn).Count;
        }

        internal void UpdateFlightStatus(SimulationTime time)
        {
            double min = Departure.Subtract(time.DateTime).TotalMinutes;

            if (min > 300)
            {
                Status = FlightStatus.OpenForReservation;
            }
            else if (min > 160)
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
                //Terminal.Open();
            }
            else
            {
                Status = FlightStatus.Takeoff;
                //Terminal.Close();
            }
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
