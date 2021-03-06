namespace BaggageSorteringLib
{
    public class Simulator
    {
        public Simulator()
        {
            Time = new SimulationTime();

            CheckinArea = new CheckinArea(counterAmount: 10);
            TerminalsArea = new TerminalsArea(terminalAmount: 15);
            SortingMachine = new SortingMachine(Time, CheckinArea, TerminalsArea);
            FlightSchedule = new FlightSchedule(Time, flightScreenLength: 12);
        }
        
        public SimulationTime Time { get; private set; }
        public bool IsAutoGenerationEnabled { get; set; }

        public CheckinArea CheckinArea { get; private set; }
        public TerminalsArea TerminalsArea { get; private set; }
        public SortingMachine SortingMachine { get; private set; }
        public FlightSchedule FlightSchedule { get; private set; }

        public void Start()
        {
            SortingMachine.Start();
            Update();
        }

        public void AddReservation(Reservation reservation)
        {
            FlightSchedule.AddReservation(reservation);
        }

        public void Update()
        {
            if (Time.IsUpdateCycle)
            {
                if (IsAutoGenerationEnabled)
                {
                    FlightSchedule.GenerateRandomFlights();
                    FlightSchedule.GenerateRandomReservations();
                }

                FlightSchedule.UpdateStatuses();
                FlightSchedule.RemoveOldFlights();
                FlightSchedule.UpdateFlightScreen();

                CheckinArea.UpdateAutoCheckinProcess();
                CheckinArea.OpenCountersForIncommingFlights(FlightSchedule);
                CheckinArea.CloseExpiredCounters();

                TerminalsArea.OpenTerminalsForIncommingFlights(FlightSchedule);
                TerminalsArea.CloseTerminalsForExpiredFlights();

                Time.MoveTime();
            }
        }
    }
}
