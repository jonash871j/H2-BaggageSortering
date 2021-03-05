using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Luggage
    {
        public Luggage(int terminalId, int counterId, Reservation reservation)
        {
            TerminalId = terminalId;
            CounterId = counterId;
            Reservation = reservation;
        }

        public int TerminalId { get; set; }
        public int CounterId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
