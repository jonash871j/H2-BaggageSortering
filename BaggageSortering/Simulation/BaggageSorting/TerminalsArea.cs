using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal void Clear()
        {
            for (int i = 0; i < Terminals.Length; i++)
            {
                Terminals[i] = new Terminal(id: i);
            }
        }
        internal void OpenTerminalsForIncommingFlights(FlightSchedule flightSchedule)
        {
            List<Flight> flights = flightSchedule.Flights.FindAll(f => f.Status == FlightStatus.OnTheWay);
            
            foreach (Flight flight in flights)
            {
                if (flight.Terminal == null)
                {
                    Terminal terminal = Terminals.FirstOrDefault(t => !t.IsFlightConnectedToTerminal);

                    if (terminal != null)
                    {
                        flight.MoveToTerminal(terminal);
                        terminal.ConnectTerminalToFlight(flight);
                    }
                }
            }
        }
        internal void CloseTerminalsForExpiredFlights()
        {
            foreach (Terminal terminal in Terminals)
            {
                if (terminal.Flight != null)
                {
                    if (terminal.Flight.Status == FlightStatus.Takeoff)
                    {
                        terminal.DisconnectFlightFromTerminal();
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
