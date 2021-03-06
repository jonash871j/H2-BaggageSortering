using BaggageSorteringLib;
using Engine;
using System;

class Program
{
    static Simulator simulator;
    static ConsoleBuffer smBuffer = new ConsoleBuffer(12);
    static ConsoleBuffer gBuffer = new ConsoleBuffer(12);

    static void Main()
    {
        // Starts simulator
        simulator = new Simulator();
        simulator.IsAutoGenerationEnabled = true;
        simulator.SortingMachine.ProcessInfo += OnSortingMachineProcessInfo;
        simulator.SortingMachine.ProcessExceptionInfo += OnSortingMachineProcessError;
        simulator.FlightSchedule.AutoReservationsInfo += OnGeneralInfo;
        simulator.FlightSchedule.FlightInfo += OnGeneralInfo;
        simulator.FlightSchedule.BadFlightInfo += OnGeneralWarningInfo;
        simulator.ProcessExceptionInfo = OnGeneralErrorInfo;
        simulator.Start();

        // Creates advanced console
        ConsoleEx.Create(100, 72);
        ConsoleEx.SetFont("Terminal", 8, 12);

        // Simulation loop
        while (true)
        {
            UserInput();
            DrawAirportOverview();

            ConsoleEx.Update(); // Renderers current content to console
            ConsoleEx.Clear();
        }
    }

    static void UserInput()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.KeyPressed((Key)(i + '1')))
            {
                simulator.Time.Speed = (int)Math.Pow(2, i);
            }
        }
        if (Input.KeyPressed(Key.UP))
        {
            simulator.BustleLevel++;
        }
        if (Input.KeyPressed(Key.DOWN))
        {
            simulator.BustleLevel--;
        }
        if (Input.KeyPressed((Key)'R'))
        {
            simulator.Restart();
        }
    }
    static void DrawAirportOverview()
    {
        ConsoleEx.SetPosition(60, 0);
        ConsoleEx.WriteLine("\f9                CONTROLS");
        ConsoleEx.WriteLine("  SIMULATION SPEED :\ff 1 2 3 4 5 6 7 8 9");
        ConsoleEx.WriteLine("AIRPORT BUSTLE ADD :\ff ARROW UP");
        ConsoleEx.WriteLine("AIRPORT BUSTLE SUB :\ff ARROW DOWN");
        ConsoleEx.WriteLine("           RESTART :\ff R");

        ConsoleEx.SetPosition(0, 0);
        ConsoleEx.WriteLine(
            $"- \faAIRPOT OVERVIEW  " +
            $"\fd{simulator.Time.DateTime.ToShortTimeString()}  " +
            $"\feSPEED {simulator.Time.Speed}x  " +
            $"\fbBUSTLE LVL {simulator.BustleLevel}"
        );
        
        AirportDraw.CheckinArea(simulator.CheckinArea, 1);
        AirportDraw.ConveyorBelt(simulator.SortingMachine.ConveyorBelt, " - Sorting system's conveyor belt", 12);
        AirportDraw.TerminalsArea(simulator.TerminalsArea, 13);

        AirportDraw.FlightSchedule(29, simulator.FlightSchedule);
        AirportDraw.InfoBuffer(43, "SORTING MACHINE INFO", smBuffer);
        AirportDraw.InfoBuffer(57, "GENERAL INFO", gBuffer);
    }

    static void OnSortingMachineProcessInfo(string msg)     => smBuffer.WriteLine(msg);
    static void OnSortingMachineProcessError(string msg)    => smBuffer.WriteLine("\fc" + msg);
    static void OnGeneralInfo(string msg)                   => gBuffer.WriteLine(msg);
    static void OnGeneralWarningInfo(string msg)            => gBuffer.WriteLine("\fe" + msg);
    static void OnGeneralErrorInfo(string msg)              => gBuffer.WriteLine("\fc" + msg);
}