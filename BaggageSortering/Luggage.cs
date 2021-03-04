using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Luggage
    {
        public Luggage(int terminalId, Reservation reservation)
        {
            TerminalId = terminalId;
            Reservation = reservation;
        }

        public int TerminalId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
