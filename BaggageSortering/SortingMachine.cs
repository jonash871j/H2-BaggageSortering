using System;
using System.Linq;
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
            ConveyorBelt = new ConveyorBelt<Luggage>(20);
        }

        private static Random rng = new Random();

        public Counter[] Counters { get; private set; }
        public Terminal[] Terminals { get; private set; }
        public ConveyorBelt<Luggage> ConveyorBelt { get; private set; }
        public MessageEvent ProcessInfo { get; set; }
        public MessageEvent ProcessException { get; set; }

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
                    Monitor.Enter(ConveyorBelt);

                    foreach (Counter counter in Counters)
                    {
                        if (counter.IsReady() && ConveyorBelt.IsSpace())
                        {
                            Luggage luggage = counter.GetLuggageFromCounter();
                            ConveyorBelt.Push(luggage);
                            ConveyorBelt.MoveForward();
                            ProcessInfo?.Invoke($"Luggage owned by {luggage.OwnerName} is now on the conveyor belt to terminal {luggage.TerminalId}");
                            Thread.Sleep(200);
                        }
                    }

                    Monitor.Pulse(ConveyorBelt);
                    Monitor.Wait(ConveyorBelt);
                    Monitor.Exit(ConveyorBelt);
;
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                ProcessException?.Invoke(ex.Message);
            }
        }
        public void SorterToTerminalProcess()
        {
            try
            {
                while (true)
                {
                    Monitor.Enter(ConveyorBelt);

                    if (!ConveyorBelt.IsPullEmpty())
                    {
                        Luggage luggage = ConveyorBelt.Pull();
                        Terminal terminal = Terminals.FirstOrDefault(t => t.Id == luggage.TerminalId);
                        terminal.AddLuggage(luggage);
                        ProcessInfo?.Invoke($"Luggage owned by {luggage.OwnerName} is now in terminal {terminal.Id}");
                    }
                    else
                    {
                        ConveyorBelt.MoveForward();
                    }

                    Monitor.Pulse(ConveyorBelt);
                    Monitor.Wait(ConveyorBelt);
                    Monitor.Exit(ConveyorBelt);
                }
            }
            catch (Exception ex)
            {
                ProcessException?.Invoke(ex.Message);
            }
        }
    }
}

