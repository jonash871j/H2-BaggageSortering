using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BaggageSorteringLib
{
    public class SortingMachine
    {
        public SortingMachine(SimulationTime time, CheckinArea checkinArea, TerminalsArea terminalsArea)
        {
            Time = time;
            Counters = checkinArea.Counters;
            Terminals = terminalsArea.Terminals;
            ConveyorBelt = new ConveyorBelt<Luggage>(20);
        }

        private static readonly Random rng = new Random();
        private readonly SimulationTime Time;
        private readonly Counter[] Counters;
        private readonly Terminal[] Terminals;
        private bool isClearRequested = false;

        public ConveyorBelt<Luggage> ConveyorBelt { get; private set; }
        public MessageEvent ProcessInfo { get; set; }
        public MessageEvent ProcessExceptionInfo { get; set; }

        internal void Start()
        {
            Thread sorterThread = new Thread(UpdateSorterProcess);
            sorterThread.Start();
        }
        internal void Clear()
        {
            isClearRequested = true;
        }
        
        private void UpdateSorterProcess()
        {
            ProcessInfo?.Invoke("Sorting machine has started...");

            try
            {
                while (true)
                {
                    UpdateCounterToSorterProcess();
                    UpdateSorterToTerminalProcess();
                    Thread.Sleep(256 / Time.Speed);

                    if (isClearRequested)
                    {
                        ConveyorBelt.Clear();
                        isClearRequested = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessExceptionInfo?.Invoke("Thread USP crashed: " + ex.Message);
            }
        }

        private void UpdateCounterToSorterProcess()
        {
            if (ConveyorBelt.IsSpace())
            {
                List<Counter> counters = Counters.Where(c => c.IsLuggageSlotAvailable()).ToList();

                if (counters.Count != 0)
                {
                    Counter counter = counters[rng.Next(0, counters.Count)];
                    Luggage luggage = counter.GetLuggageFromCounter();
                    ConveyorBelt.Push(luggage);
                    ProcessInfo?.Invoke($"Luggage owned by {luggage.Reservation.Passenger.FirstName} is now on the conveyor belt to terminal {luggage.TerminalId}");
                }
            }
        }
        private void UpdateSorterToTerminalProcess()
        {
            if (!ConveyorBelt.IsPullEmpty())
            {
                Luggage luggage = ConveyorBelt.Pull();
                Terminal terminal = Terminals.FirstOrDefault(t => t.Id == luggage.TerminalId);
                terminal.AddLuggage(luggage);
                ProcessInfo?.Invoke($"Luggage owned by {luggage.Reservation.Passenger.FirstName} is now in terminal {terminal.Id}");
            }
            else
            {
                ConveyorBelt.MoveForward();
            }
        }
    }
}

