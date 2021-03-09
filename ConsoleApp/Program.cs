using BaggageSorteringLib;
using Engine;
using System;
using System.Threading;

class Program
{
    static ConsoleBuffer smBuffer = new ConsoleBuffer(12);
    static ConsoleBuffer gBuffer = new ConsoleBuffer(12);

    static void Main()
    {
        //  Initialize simulator
        Simulator sim = new Simulator(
            counterAmount: 10,
            terminalAmount: 15,
            conveyorBeltLength: 20
        );
        sim.IsAutoGenereatedReservationsEnabled = true;
        sim.SortingMachine.ProcessInfo += OnSortingMachineProcessInfo;
        sim.SortingMachine.ProcessExceptionInfo += OnSortingMachineProcessError;
        sim.FlightSchedule.FlightInfo += OnGeneralInfo;
        sim.FlightSchedule.BadFlightInfo += OnGeneralWarningInfo;
        sim.ProcessExceptionInfo += OnGeneralErrorInfo;
        sim.Start();

        // Creates advanced console
        ConsoleEx.Create(100, 72);
        ConsoleEx.SetFont("Terminal", 8, 12);

        // Simulation loop
        while (true)
        {
            if (Monitor.TryEnter(sim))
            {
                // User input section *************************
                for (int i = 0; i < 8; i++)
                {
                    if (Input.KeyPressed((Key)(i + '1')))
                    {
                        sim.Time.Speed = (int)Math.Pow(2, i);
                    }
                }
                if (Input.KeyPressed(Key.UP))
                {
                    sim.BustleLevel++;
                }
                if (Input.KeyPressed(Key.DOWN))
                {
                    sim.BustleLevel--;
                }
                if (Input.KeyPressed((Key)'R'))
                {
                    sim.Restart();
                }

                // Draw section *******************************
                ConsoleEx.SetPosition(60, 0);
                ConsoleEx.WriteLine("\f9                CONTROLS");
                ConsoleEx.WriteLine("  SIMULATION SPEED :\ff 1 2 3 4 5 6 7 8");
                ConsoleEx.WriteLine("AIRPORT BUSTLE ADD :\ff ARROW UP");
                ConsoleEx.WriteLine("AIRPORT BUSTLE SUB :\ff ARROW DOWN");
                ConsoleEx.WriteLine("           RESTART :\ff R");

                ConsoleEx.SetPosition(0, 0);
                ConsoleEx.WriteLine(
                    $"- \faAIRPOT OVERVIEW  " +
                    $"\fd{sim.Time.DateTime.ToShortTimeString()}  " +
                    $"\feSPEED {sim.Time.Speed}x  " +
                    $"\fbBUSTLE LVL {sim.BustleLevel}"
                );

                AirportDraw.CheckinArea(y: 1, sim.CheckinArea);
                AirportDraw.ConveyorBelt(y: 12, sim.SortingMachine.ConveyorBelt);
                AirportDraw.TerminalsArea(y: 13, sim.TerminalsArea);

                AirportDraw.FlightSchedule(y: 29, sim.FlightSchedule);
                AirportDraw.InfoBuffer(y: 43, "SORTING MACHINE INFO", smBuffer);
                AirportDraw.InfoBuffer(y: 57, "GENERAL INFO", gBuffer);

                // Renderers current content to console
                ConsoleEx.Update(); 
                ConsoleEx.Clear();

                // Thread synchronize
                Monitor.PulseAll(sim);
                Monitor.Exit(sim);
            }
            Thread.Sleep(10);
        }
    }

    static void OnSortingMachineProcessInfo(string msg)     => smBuffer.WriteLine(msg);
    static void OnSortingMachineProcessError(string msg)    => smBuffer.WriteLine("\fc" + msg);
    static void OnGeneralInfo(string msg)                   => gBuffer.WriteLine(msg);
    static void OnGeneralWarningInfo(string msg)            => gBuffer.WriteLine("\fe" + msg);
    static void OnGeneralErrorInfo(string msg)              => gBuffer.WriteLine("\fc" + msg);
}