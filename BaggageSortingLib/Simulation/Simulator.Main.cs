using System;
using System.Threading;

namespace BaggageSorteringLib
{
    public partial class Simulator
    {
        public Simulator(int counterAmount, int terminalAmount, int conveyorBeltLength)
        {
            Time = new SimulationTime();
            Time.TimeUpdate = OnTimeUpdate;

            CheckinArea = new CheckinArea(counterAmount);
            TerminalsArea = new TerminalsArea(terminalAmount);
            SortingMachine = new SortingMachine(Time, conveyorBeltLength, CheckinArea, TerminalsArea);
            FlightSchedule = new FlightSchedule(Time);
        }

        private int _bustleLevel = 5;

        public SimulationTime Time { get; private set; }
        public CheckinArea CheckinArea { get; private set; }
        public TerminalsArea TerminalsArea { get; private set; }
        public SortingMachine SortingMachine { get; private set; }
        public FlightSchedule FlightSchedule { get; private set; }
        public bool IsSimulationStarted { get; private set; }

        public event MessageEvent ProcessExceptionInfo;
        public bool IsAutoGenereatedReservationsEnabled { get; set; }
        public int BustleLevel 
        {
            get => _bustleLevel;
            set
            {
                if (value > 0 && value <= 10)
                {
                    _bustleLevel = value;
                    Restart();
                }
            }
        }

        /// <summary>
        /// Used to start the simulation, can only be called once
        /// </summary>
        public void Start()
        {
            if (!IsSimulationStarted)
            {
                Time.Start();
                SortingMachine.Start(); // Starts sorting machine thread
                Update();

                IsSimulationStarted = true;
            }
        }

        /// <summary>
        /// Used to stop simulation
        /// </summary>
        public void Stop()
        {
            Time.Stop();
            SortingMachine.Stop();
        }

        /// <summary>
        /// Used to restart the simulation
        /// </summary>
        public void Restart()
        {
            Time.Stop();

            FlightSchedule.Clear();
            SortingMachine.Clear();
            CheckinArea.Clear();
            TerminalsArea.Clear();
            Update();

            Time.Start();
        }

        /// <summary>
        /// Event: Is being called every time the simulation time has moved a minute
        /// </summary>
        private void OnTimeUpdate()
        {
            try
            {
                Monitor.Enter(this);

                Update();

                Monitor.PulseAll(this);
                Monitor.Exit(this);
            }
            catch (Exception exception)
            {
                ProcessExceptionInfo?.Invoke("Main simulation thread crashed: " + exception.Message);
                Time.Stop();
            }
        }

        /// <summary>
        /// Used to update airport
        /// </summary>
        private void Update()
        {
            FlightSchedule.GenerateRandomFlights(BustleLevel, IsAutoGenereatedReservationsEnabled);
            FlightSchedule.UpdateStatuses();
            FlightSchedule.RemoveOldFlights();
            FlightSchedule.UpdateActiveFlights();

            CheckinArea.UpdateAutoCheckin();
            CheckinArea.OpenCountersForIncommingFlights(FlightSchedule);
            CheckinArea.CloseExpiredCounters();

            TerminalsArea.OpenTerminalsForIncommingFlights(FlightSchedule);
            TerminalsArea.CloseTerminalsForExpiredFlights();
        }
    }
}