using System.Collections.Generic;

namespace BaggageSorteringLib
{
    public class Terminal
    {
        public Terminal(int id)
        {
            Id = id;
            IsOpen = false;
            Luggages = new Queue<Luggage>();
        }

        public int Id { get; private set; }
        public bool IsOpen { get; private set; }
        public bool IsFlightReservedToTerminal { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public Flight Flight { get; private set; }

        public override string ToString()
        {
            if (IsFlightReservedToTerminal)
            {
                return $"Gate {Id} {Flight.Name} {Flight.Status}";
            }
            else
            {
                return $"Gate {Id}";
            }
        }

        /// <summary>
        /// Used to close terminal
        /// </summary>
        internal void Close()
        {
            RemoveFlightFromTerminal();
            IsOpen = false;
        }

        /// <summary>
        /// Used to open terminal
        /// </summary>
        internal void Open()
        {
            IsOpen = true;
        }

        /// <summary>
        /// Used to add luggage to terminal storage
        /// </summary>
        internal void AddLuggage(Luggage luggage)
        {
            Luggages.Enqueue(luggage);
        }

        /// <summary>
        /// Used to connect terminal to flight
        /// </summary>
        internal void ReserveTerminalToFlight(Flight flight)
        {
            Flight = flight;
            IsFlightReservedToTerminal = true;
        }

        /// <summary>
        /// Used to idsconnect flight from terminal 
        /// </summary>
        internal void RemoveFlightFromTerminal()
        {
            Flight = null;
            IsFlightReservedToTerminal = false;
        }

        /// <summary>
        /// Used to export all luggage storted in the terminal to flight
        /// </summary>
        internal void ExportLuggagesToFlight()
        {
            if (IsFlightReservedToTerminal)
            {
                Flight.LoadWithLuggages(Luggages);
                Luggages = new Queue<Luggage>();
            }
        }
    }
}