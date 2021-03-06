using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Counter : IOpenClose
    {
        public Counter(int id)
        {
            Id = id;
            Luggage = null;
            Flight = Flight.None;
            IsOpen = false;
        }

        public int Id { get; private set; }
        public Luggage Luggage { get; private set; }
        public Flight Flight { get; private set; }
        public bool IsOpen { get; private set; }

        public void Close()
        {
            IsOpen = false;
            Flight = Flight.None;
        }
        public void Open()
        {
            IsOpen = true;
        }

        public void UpdateFlight(Flight flight)
        {
            Flight = flight;
        }

        public void CheckLuggageIn(Luggage luggage)
        {
            if (Luggage == null)
            {
                Luggage = luggage;
            }
            else
            {
                // There is already a luggage on counter
            }
        }
        internal Luggage GetLuggageFromCounter()
        {
            Luggage luggage = Luggage;
            Luggage = null;
            return luggage;
        }
        public bool IsLuggageSlotAvailable()
        {
            return (IsOpen) && (Luggage != null);
        }
    }
}
