using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BaggageSorteringLib
{
    public class SortingMachine
    {
        public SortingMachine(SimulationTime time, int conveyorBeltLength, CheckinArea checkinArea, TerminalsArea terminalsArea)
        {
            this.time = time;
            counters = checkinArea.Counters;
            terminals = terminalsArea.Terminals;
            ConveyorBelt = new ConveyorBelt<Luggage>(conveyorBeltLength);
        }

        private readonly static Random rng = new Random();
        private readonly SimulationTime time;
        private readonly Counter[] counters;
        private readonly Terminal[] terminals;
        private Thread sorterThread;
        private bool isClearRequested = false;
        private bool isStopRequested = false;

        public ConveyorBelt<Luggage> ConveyorBelt { get; private set; }
        public event MessageEvent ProcessInfo;
        public event MessageEvent ProcessExceptionInfo;


        /// <summary>
        /// Used to start sorting machine process
        /// </summary>
        internal void Start()
        {
            sorterThread = new Thread(SortingMachineProcess);
            sorterThread.Name = "SorterThread";
            sorterThread.Start();
        }

        /// <summary>
        /// Used to stop sorting machine process
        /// </summary>
        internal void Stop()
        {
            isStopRequested = true;
            sorterThread.Join();
        }

        /// <summary>
        /// Used to clear sorting machines conveyor belt
        /// </summary>
        internal void Clear()
        {
            isClearRequested = true;
        }
        
        /// <summary>
        /// Used to run the sorting machine process
        /// </summary>
        private void SortingMachineProcess()
        {
            ProcessInfo?.Invoke("Sorting machine has started...");

            try
            {
                while (!isStopRequested)
                {
                    // Updates the sortering processes
                    UpdateSorterToTerminalProcess();
                    UpdateCounterToSorterProcess();
                    Thread.Sleep(128 / time.Speed);

                    // Clears conveyor belt when requested
                    if (isClearRequested)
                    {
                        ConveyorBelt.Clear();
                        isClearRequested = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessExceptionInfo?.Invoke("Thread SorterThread crashed: " + ex.Message);
            }
        }

        /// <summary>
        /// Used to update the counter to sorter process
        /// </summary>
        private void UpdateCounterToSorterProcess()
        {
            if (ConveyorBelt.IsSpace())
            {
                // Gets all counters there has luggage to go on the conveyor belt
                List<Counter> counters = this.counters.Where(c => c.IsLuggageReady()).ToList();

                if (counters.Count != 0)
                {
                    // Finds random counter
                    Counter counter = counters[rng.Next(0, counters.Count)];
                    
                    // Gets luggage
                    Luggage luggage = counter.GetLuggageFromCounter();

                    // Push the luggage to the conveyor belt
                    ConveyorBelt.Push(luggage);

                    ProcessInfo?.Invoke($"Luggage owned by {luggage.Reservation.Passenger.FirstName} is now on the conveyor belt to terminal {luggage.TerminalId}");
                }
            }
        }

        /// <summary>
        /// Used to update the sorter to terminal process
        /// </summary>
        private void UpdateSorterToTerminalProcess()
        {
            // Check if next pull is not empty
            if (!ConveyorBelt.IsPullEmpty())
            {
                // Pulls luggage from last index in the buffer
                Luggage luggage = ConveyorBelt.Pull();

                // Finds the terminal where the luggage should goto
                Terminal terminal = terminals.FirstOrDefault(t => t.Id == luggage.TerminalId);

                if (terminal != null)
                {
                    terminal.AddLuggage(luggage); // Adds luggage to terminal storage
                }

                ProcessInfo?.Invoke($"Luggage owned by {luggage.Reservation.Passenger.FirstName} is now in terminal {terminal.Id}");
            }
            else
            {
                ConveyorBelt.MoveForward();
            }
        }
    }
}

