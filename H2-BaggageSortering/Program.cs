using BaggageSorteringLib;
using Engine;
using System;

class Program
{
    static Simulator simulator;
    static ConsoleBuffer sortingMachineBuffer = new ConsoleBuffer(12);
    static ConsoleBuffer reservationBuffer = new ConsoleBuffer(12);

    static void Main(string[] args)
    {
        // Starts simulator
        simulator = new Simulator();
        simulator.IsAutoGenerationEnabled = true;
        simulator.SortingMachine.ProcessInfo += OnSortingMachineProcessInfo;
        simulator.SortingMachine.ProcessException += OnSortingMachineProcessException;
        simulator.FlightSchedule.NewReservation += OnNewReservation;
        simulator.Start();

        // Creates advanced console
        ConsoleEx.Create(100, 72);
        ConsoleEx.SetFont("Terminal", 8, 12);

        // Simulation loop
        while (true)
        {
            simulator.Update(); // Updates simulation

            GetUserInput();
            DrawAirportOverview();

            ConsoleEx.Update(); // Renderers current content to console
            ConsoleEx.Clear(); // Clears console buffer
        }
    }

    private static void OnSortingMachineProcessInfo(string msg)
    {
        sortingMachineBuffer.WriteLine(msg);
    }
    private static void OnSortingMachineProcessException(string msg)
    {
        sortingMachineBuffer.WriteLine("\fc" + msg);
    }
    private static void OnNewReservation(string msg)
    {
        reservationBuffer.WriteLine(msg);
    }

    static void GetUserInput()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.KeyPressed((Key)(i + '1')))
            {
                simulator.Time.Speed = (int)Math.Pow(2, i);
            }
        }
    }
    static void DrawAirportOverview()
    {
        ConsoleEx.WriteLine(
            $"- \faAIRPOT OVERVIEW \f7| " +
            $"\feSPEED {simulator.Time.Speed}x \f7| " +
            $"\fd{simulator.Time.DateTime.ToShortTimeString()}"
        );
        
        AirportDraw.CheckinArea(simulator.CheckinArea, 1);
        AirportDraw.ConveyorBelt(simulator.SortingMachine.ConveyorBelt, " - Sorting system's conveyor belt", 12);
        AirportDraw.TerminalsArea(simulator.TerminalsArea, 13);

        AirportDraw.FlightSchedule(29, simulator.FlightSchedule);
        AirportDraw.InfoBuffer(43, "SORTING MACHINE INFO", sortingMachineBuffer);
        AirportDraw.InfoBuffer(57, "RESERVATION INFO", reservationBuffer);
    }
}