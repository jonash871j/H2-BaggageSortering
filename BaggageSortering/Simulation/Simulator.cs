using System;
using System.Configuration;
using System.Threading;

namespace BaggageSorteringLib
{
    public class Simulator
    {
        public Simulator()
        {
            Time = new SimulationTime();
            Time.TimeUpdate = OnTimeUpdate;

            CheckinArea = new CheckinArea(counterAmount: 10);
            TerminalsArea = new TerminalsArea(terminalAmount: 15);
            SortingMachine = new SortingMachine(Time, CheckinArea, TerminalsArea);
            FlightSchedule = new FlightSchedule(Time, flightScreenLength: 12);
        }

        private int _bustleLevel = 1;

        public SimulationTime Time { get; private set; }
        public CheckinArea CheckinArea { get; private set; }
        public TerminalsArea TerminalsArea { get; private set; }
        public SortingMachine SortingMachine { get; private set; }
        public FlightSchedule FlightSchedule { get; private set; }

        public MessageEvent ProcessExceptionInfo { get; set; }
        public bool IsAutoGenerationEnabled { get; set; }

        public int BustleLevel 
        {
            get => _bustleLevel;
            set
            {
                if (value > 0 && value <= 10)
                {
                    _bustleLevel = value;
                }
            }
        }

        public void Start()
        {
            Time.Start();
            SortingMachine.Start();
            Update();
        }
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

        public void AddReservation(Reservation reservation)
        {
            FlightSchedule.AddReservation(reservation);
        }

        private void OnTimeUpdate()
        {
            try
            {
                Update();
            }
            catch (Exception exception)
            {
                ProcessExceptionInfo?.Invoke("Main simulation thread crashed: " + exception.Message);
                Time.Stop();
            }
        }
        private void Update()
        {
            if (IsAutoGenerationEnabled)
            {
                FlightSchedule.GenerateRandomFlights(BustleLevel, isPreBookedWithRandomAmount: true);
            }

            FlightSchedule.UpdateStatuses();
            FlightSchedule.RemoveOldFlights();
            FlightSchedule.UpdateFlightScreen();

            CheckinArea.UpdateAutoCheckin();
            CheckinArea.OpenCountersForIncommingFlights(FlightSchedule);
            CheckinArea.CloseExpiredCounters();

            TerminalsArea.OpenTerminalsForIncommingFlights(FlightSchedule);
            TerminalsArea.CloseTerminalsForExpiredFlights();
        }
    }
}
