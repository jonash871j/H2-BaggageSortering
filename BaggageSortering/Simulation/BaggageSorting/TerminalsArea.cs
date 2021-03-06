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

            for (int i = 0; i < terminalAmount; i++)
            {
                Terminals[i] = new Terminal(id: i);
            }
        }

        public Terminal[] Terminals { get; private set; }

        public void OpenTerminalsForIncommingFlights(FlightSchedule flightSchedule)
        {
            List<Flight> flights = flightSchedule.Flights.FindAll(f => f.Status == FlightStatus.OnTheWay);
            
            foreach (Flight flight in flights)
            {
                if (flight.Terminal == null)
                {
                    Terminal terminal = Terminals.FirstOrDefault(t => t.Flight == null);
                    if (terminal != null)
                    {
                        terminal.Flight = flight;
                        flight.Terminal = terminal;
                    }
                }
            }
        }
        public void CloseTerminalsForExpiredFlights()
        {
            foreach (Terminal terminal in Terminals)
            {
                if (terminal.Flight != null)
                {
                    if (terminal.Flight.Status == FlightStatus.Takeoff)
                    {
                        terminal.Flight = null;
                        terminal.Close();
                    }
                    else if (terminal.Flight.Status == FlightStatus.Refilling && terminal.Luggages.Count > 0)
                    {
                        terminal.LoadFlightLuggages();
                    }
                }
            }
        }
    }
}
