using System;

namespace BaggageSorteringLib
{
    public static class AutoGenerator
    {
        private static Random rng = new Random();

        /// <summary>
        /// Used to generate random flight
        /// </summary>
        /// <returns>flight</returns>
        public static Flight CreateRandomFlight(DateTime startArrival, int minArrival, int maxArrival)
        {
            startArrival = startArrival.AddMinutes(rng.Next(minArrival, maxArrival));

            return new Flight(
                name: $"F-{rng.Next(0, 10000)}",
                seatsAmount: rng.Next(100, 300),
                arrival: startArrival,
                departure: startArrival.AddMinutes(60),
                destination: Data.GetRandomCity()
            );
        }

        /// <summary>
        /// Used to create random reservation
        /// </summary>
        /// <returns>reservation</returns>
        public static Reservation CreateRandomReservation(Flight flight)
        {
            string firstName = Data.GetRandomName();

            Passenger passenger = new Passenger(
                firstName: firstName,   
                lastName: Data.GetRandomName(),
                email: firstName + rng.Next(1, 10000) + "@gmail.com",
                phoneNumber: "+45" + rng.Next(10000000, 99999999),
                address: $"{Data.GetRandomCity()} {Data.GetRandomStreet()} {rng.Next(1, 1000)}"
            );

            return new Reservation(
                passenger,
                flight
            );
        }
    }
}
