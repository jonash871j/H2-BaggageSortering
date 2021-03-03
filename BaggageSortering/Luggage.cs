using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Luggage
    {
        public Luggage(string ownerName, int terminalId)
        {
            OwnerName = ownerName;
            TerminalId = terminalId;
        }

        public string OwnerName { get; set; }
        public int TerminalId { get; set; }
    }
}
