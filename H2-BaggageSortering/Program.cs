using System;
using System.Collections.Generic;
using System.Threading;
using BaggageSorteringLib;
using Engine;

class Program
{
    static Random rng = new Random();
    static string sortingMachineInfo = "";
    static void Main(string[] args)
    {
        Simulator simulator = new Simulator();
        simulator.SortingMachine.ProcessInfo += OnSortingMachineProcessInfo;
        simulator.Start();

        ConsoleEx.Create(128, 32);
        ConsoleEx.SetFont("Consolas", 8, 16);

        while (true)
        {
            for (int i = 0; i < simulator.Counters.Length; i++)
            {
                ConsoleEx.WriteLine($"Counter {simulator.Counters[i].Id}: IsReady {simulator.Counters[i].IsReady()}");
            }

            if (Input.KeyPressed((Key)'A'))
            {
                simulator.Counters[0].Open();
                simulator.Counters[0].CheckLuggageIn(new Luggage("Tobias", 0));
            }
            if (Input.KeyPressed((Key)'B'))
            {
                simulator.Counters[1].Open();
                simulator.Counters[1].CheckLuggageIn(new Luggage("Tobias", 1));
            }
            if (Input.KeyPressed((Key)'C'))
            {
                simulator.Counters[2].Open();
                simulator.Counters[2].CheckLuggageIn(new Luggage("Tobias", 2));
            }

            DrawTray(simulator.SortingMachine.BigTrayFromCounters, "BigTrayFromCounters", 16);
            DrawTerminals(simulator.Terminals, 17);


            ConsoleEx.WriteLine();
            ConsoleEx.WriteLine();
            ConsoleEx.WriteLine(sortingMachineInfo);

            ConsoleEx.Update();
            ConsoleEx.Clear();
        }
    }

    private static void OnSortingMachineProcessInfo(string msg)
    {
        sortingMachineInfo = msg;
    }

    static void DrawTray(BufferTray<Luggage> tray, string name, int y)
    {
        for (int i = 0; i < tray.Position; i++)
        {
            Luggage luggage = tray.Buffer[i];

            ConsoleEx.WriteCharacter(i, y, '■', (byte)(luggage.TerminalId + 1));
        }
        ConsoleEx.WriteCoord(tray.Position + 3, y, name);
    }
    static void DrawTerminals(Terminal[] terminals, int y)
    {
        for (int i = 0; i < terminals.Length; i++)
        {
            Terminal terminal = terminals[i];
            int j = 0;
            foreach (Luggage luggage in terminal.Luggages)
            {
                ConsoleEx.WriteCharacter(j, y + i, '■', (byte)(luggage.TerminalId + 1));
                j++;
            }
            ConsoleEx.WriteCoord(terminal.Luggages.Count + 3, y + i, $"Terminal {terminal.Id}");
        }
    }
}