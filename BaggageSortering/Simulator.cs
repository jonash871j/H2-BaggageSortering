using System.Linq;

namespace BaggageSorteringLib
{
    public class Simulator
    {
        public Simulator()
        {
            Counters = new Counter[]
            {
                new Counter(1),
                new Counter(2),
                new Counter(3),
                new Counter(4),
                new Counter(5),
            };
            Terminals = new Terminal[]
            {
                new Terminal(1), 
                new Terminal(2), 
                new Terminal(3), 
                new Terminal(4), 
                new Terminal(5), 
            };
            SortingMachine = new SortingMachine(Counters, Terminals);
        }

        public SortingMachine SortingMachine { get; private set; }
        public Counter[] Counters { get; private set; }
        public Terminal[] Terminals { get; private set; }

        public void Start()
        {
            SortingMachine.Start();
        }

        public void CheckLuggageIn(int terminalId, Luggage luggage)
        {
            Terminal terminal = Terminals.FirstOrDefault(t => t.Id == terminalId);
        }

    }
}
