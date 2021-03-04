using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public static class AirportData
    {
        static AirportData()
        {
            CityDestinations = File.ReadLines(@"Resources\Cities.txt").ToArray();
        }

        public static readonly string[] CityDestinations;
    }
}
