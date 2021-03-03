using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Simulator
    {
        public Simulator()
        {
            Counters = new Counter[]
            {
                new Counter(0), //..
                new Counter(1), //..
                new Counter(2), //..
            };
            Terminals = new Terminal[]
            {
                new Terminal(0), //..
                new Terminal(1), //..
                new Terminal(2), //..
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
    }
}
