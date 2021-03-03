using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaggageSorteringLib
{
    public delegate void MessageEvent(string msg);

    public class SortingMachine
    {
        public SortingMachine(Counter[] counters, Terminal[] terminals)
        {
            Counters = counters;
            Terminals = terminals;
            BigTrayFromCounters = new BufferTray<Luggage>(100);
            //BigTrayToTerminals = new BufferTray<Luggage>(100);
        }

        private static Random rng = new Random();

        public Counter[] Counters { get; private set; }
        public Terminal[] Terminals { get; private set; }
        public BufferTray<Luggage> BigTrayFromCounters { get; private set; }
        //public BufferTray<Luggage> BigTrayToTerminals { get; private set; }
        public MessageEvent ProcessInfo { get; set; }

        public void Start()
        {
            Thread counterToSorterThread = new Thread(CounterToSorterProcess);
            Thread sorterToTerminalThread = new Thread(SorterToTerminalProcess);
            counterToSorterThread.Start();
            sorterToTerminalThread.Start();
        }

        public void CounterToSorterProcess()
        {
            try
            {
                while (true)
                {
                    if (Monitor.TryEnter(Counters))
                    {
                        if (CheckIfCountersContainLuggage() && Monitor.TryEnter(BigTrayFromCounters))
                        {
                            foreach (Counter counter in Counters)
                            {
                                if (counter.IsReady() && BigTrayFromCounters.IsSpace())
                                {
                                    Thread.Sleep(200);
                                    Luggage luggage = counter.GetLuggageFromCounter();
                                    BigTrayFromCounters.PushToFirst(luggage);
                                    break;
                                }
                            }
                            Monitor.PulseAll(BigTrayFromCounters);
                            Monitor.Exit(BigTrayFromCounters);
                        }
                        Monitor.PulseAll(Counters);
                        Monitor.Exit(Counters);
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessInfo?.Invoke(ex.Message);
            }
        }
        public void SorterToTerminalProcess()
        {
            try
            {
                while (true)
                {
                    if ((BigTrayFromCounters.Length > 0) && Monitor.TryEnter(BigTrayFromCounters))
                    {
                        if (Monitor.TryEnter(Terminals))
                        {
                            if (!BigTrayFromCounters.IsEmpty())
                            {
                                Thread.Sleep(200);
                                Luggage luggage = BigTrayFromCounters.Pull();
                                Terminal terminal = FindTerminal(luggage.TerminalId);
                                terminal.AddLuggage(luggage);
                            }
               
                            Monitor.PulseAll(Terminals);
                            Monitor.Exit(Terminals);
                        }

                        Monitor.PulseAll(BigTrayFromCounters);
                        Monitor.Exit(BigTrayFromCounters);
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                ProcessInfo?.Invoke(ex.Message);
            }
        }

        private bool CheckIfCountersContainLuggage()
        {
            foreach (Counter counter in Counters)
            {
                if (counter.IsReady())
                {
                    return true;
                }
            }
            return false;
        }
        private Terminal FindTerminal(int id)
        {
            foreach (Terminal terminal in Terminals)
            {
                if (terminal.Id == id)
                {
                    return terminal;
                }
            }
            throw new Exception("Invalid terminal id");
        }
    }
}

