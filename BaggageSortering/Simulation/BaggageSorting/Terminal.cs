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
        public bool IsFlightConnectedToTerminal { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public Flight Flight { get; private set; }

        public override string ToString()
        {
            if (IsOpen)
            {
                return $"{Id}";
            }
            else
            {
                return $"{Id}";
            }
        }

        /// <summary>
        /// Used to close terminal
        /// </summary>
        internal void Close()
        {
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
        internal void ConnectTerminalToFlight(Flight flight)
        {
            Flight = flight;
            IsFlightConnectedToTerminal = true;
        }

        /// <summary>
        /// Used to idsconnect flight from terminal 
        /// </summary>
        internal void DisconnectFlightFromTerminal()
        {
            Flight = null;
            IsFlightConnectedToTerminal = false;
        }

        /// <summary>
        /// Used to export all luggage storted in the terminal to flight
        /// </summary>
        internal void ExportLuggagesToFlight()
        {
            if (IsFlightConnectedToTerminal)
            {
                Flight.LoadWithLuggages(Luggages);
                Luggages = new Queue<Luggage>();
            }
        }
    }
}