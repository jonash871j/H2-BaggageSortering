using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Counter
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

        /// <summary>
        /// Used to close counter
        /// </summary>
        internal void Close()
        {
            IsOpen = false;
            Flight = Flight.None;
        }

        /// <summary>
        /// Used to open counter
        /// </summary>
        internal void Open()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Used to set targeted flight, 
        /// this will determine where the luggage should goto
        /// </summary>
        internal void SetTargetedFlight(Flight flight)
        {
            Flight = flight;
        }

        /// <summary>
        /// Used to check luggage in
        /// </summary>
        internal void Checkin(Luggage luggage)
        {
            if (Luggage == null)
            {
                Luggage = luggage;
            }
            else
            {
                throw new Exception("There is already luggage on counter!");
            }
        }
        /// <summary>
        /// Used to get luggage from counter
        /// </summary>
        /// <returns></returns>
        internal Luggage GetLuggageFromCounter()
        {
            Luggage luggage = Luggage;
            Luggage = null;
            return luggage;
        }

        /// <summary>
        /// Used to check if luggage is ready to go into the sorting machine
        /// </summary>
        /// <returns></returns>
        public bool IsLuggageReady()
        {
            return (IsOpen) && (Luggage != null);
        }
    }
}
