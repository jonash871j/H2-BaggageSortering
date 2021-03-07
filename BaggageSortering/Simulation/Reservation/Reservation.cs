namespace BaggageSorteringLib
{
    public class Reservation
    {
        internal Reservation(Passenger passenger, Flight flight)
        {
            Passenger =  passenger;
            Flight = flight;
            IsCheckedIn = false;
        }

        public Passenger Passenger { get; private set; }
        public Flight Flight { get; private set; }
        public bool IsCheckedIn { get; set; }
    }
}
