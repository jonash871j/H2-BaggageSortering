using System;
using System.IO;
using System.Linq;

namespace BaggageSorteringLib
{
    public static class Data
    {
        static Data()
        {
            CityDestinations = File.ReadLines(@"Resources\Cities.txt").ToArray();
            Names = File.ReadLines(@"Resources\Names.txt").ToArray();
            Streets = File.ReadLines(@"Resources\Streets.txt").ToArray();
        }

        private static Random rng = new Random();

        public static readonly string[] CityDestinations;
        public static readonly string[] Names;
        public static readonly string[] Streets;
    
        public static string GetRandomCity()
        {
            return CityDestinations[rng.Next(0, CityDestinations.Length)];
        }
        public static string GetRandomName()
        {
            return Names[rng.Next(0, Names.Length)];
        }
        public static string GetRandomStreet()
        {
            return Names[rng.Next(0, Names.Length)];
        }
    }
}
