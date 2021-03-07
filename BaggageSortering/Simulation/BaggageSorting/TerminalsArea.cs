using System.Collections.Generic;
using System.Linq;

namespace BaggageSorteringLib
{
    public class TerminalsArea
    {
        public TerminalsArea(int terminalAmount)
        {
            Terminals = new Terminal[terminalAmount];
            Clear();
        }

        public Terminal[] Terminals { get; private set; }
        
        /// <summary>
        /// Used to clear all terminals
        /// </summary>
        internal void Clear()
        {
            for (int i = 0; i < Terminals.Length; i++)
            {
                Terminals[i] = new Terminal(id: i);
            }
        }

        /// <summary>
        /// Used to open terminas for incomming flights
        /// </summary>
        internal void OpenTerminalsForIncommingFlights(FlightSchedule flightSchedule)
        {
            // Gets all the flights there is on the way
            List<Flight> flights = flightSchedule.Flights.FindAll(f => f.Status == FlightStatus.OnTheWay);
            
            foreach (Flight flight in flights)
            {
                if (!flight.IsAtTerminal)
                {
                    Terminal terminal = Terminals.FirstOrDefault(t => !t.IsFlightReservedToTerminal);

                    if (terminal != null)
                    {
                        flight.MoveToTerminal(terminal);
                        terminal.ReserveTerminalToFlight(flight);
                    }
                }
            }
        }

        /// <summary>
        /// Used to close terminals for experied flights
        /// </summary>
        internal void CloseTerminalsForExpiredFlights()
        {
            foreach (Terminal terminal in Terminals)
            {
                if (terminal.IsFlightReservedToTerminal)
                {
                    if (terminal.Flight.Status == FlightStatus.Takeoff)
                    {
                        terminal.RemoveFlightFromTerminal();
                        terminal.Close();
                    }
                    else if (terminal.Flight.Status == FlightStatus.Refilling && terminal.Luggages.Count > 0)
                    {
                        terminal.ExportLuggagesToFlight();
                    }
                }
            }
        }
    }
}
