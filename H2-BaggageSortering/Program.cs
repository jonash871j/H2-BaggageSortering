using BaggageSorteringLib;
using Engine;

class Program
{
    static ConsoleBuffer infoBuffer = new ConsoleBuffer(12);

    static void Main(string[] args)
    {
        Simulator simulator = new Simulator();
        simulator.SortingMachine.ProcessInfo += OnSortingMachineProcessInfo;
        simulator.SortingMachine.ProcessException += OnSortingMachineProcessException;
        simulator.Start();

        ConsoleEx.Create(80, 32);
        ConsoleEx.SetFont("Consolas", 8, 16);

        while (true)
        {
            UserInput(simulator);

            DrawCounters(simulator.Counters, 1);
            DrawConveyorBelt(simulator.SortingMachine.ConveyorBelt, " - Sorting system's conveyor belt", 8);
            DrawTerminals(simulator.Terminals, 9);
            DrawInfoBuffer();

            ConsoleEx.Update();
            ConsoleEx.Clear();
        }
    }

    // User input ***********************************
    private static void UserInput(Simulator simulator)
    {
        if (Input.KeyPressed((Key)'1'))
        {
            simulator.Counters[0].CheckLuggageIn(new Luggage("Name 1", 1));
        }
        if (Input.KeyPressed((Key)'2'))
        {
            simulator.Counters[1].CheckLuggageIn(new Luggage("Name 2", 2));
        }
        if (Input.KeyPressed((Key)'3'))
        {
            simulator.Counters[2].CheckLuggageIn(new Luggage("Name 3", 3));
        }
        if (Input.KeyPressed((Key)'4'))
        {
            simulator.Counters[3].CheckLuggageIn(new Luggage("Name 4", 4));
        }
        if (Input.KeyPressed((Key)'5'))
        {
            simulator.Counters[4].CheckLuggageIn(new Luggage("Name 5", 5));
        }
    }

    // Events ***************************************
    private static void OnSortingMachineProcessInfo(string msg)
    {
        infoBuffer.WriteLine(msg);
    }
    private static void OnSortingMachineProcessException(string msg)
    {
        infoBuffer.WriteLine("\fc" + msg);
    }

    // Draw functions *******************************
    static void DrawConveyorBelt(ConveyorBelt<Luggage> conveyorBelt, string name, int y)
    {
        for (int i = 0; i < conveyorBelt.Length; i++)
        {
            Luggage luggage = conveyorBelt.Buffer[i];

            if (luggage != null)
            {
                ConsoleEx.WriteCharacter(i, y, '■', (byte)(luggage.TerminalId));
            }
        }
        ConsoleEx.SetPosition(conveyorBelt.Length + 3, y);
        ConsoleEx.WriteLine(name);
    }
    static void DrawCounters(Counter[] counters, int y)
    {
        ConsoleEx.SetPosition(0, y);
        foreach (Counter counter in counters)
        {
            ConsoleEx.WriteLine($"Counter {counter.Id}: IsReady {counter.IsReady()}");
        }
    }
    static void DrawTerminals(Terminal[] terminals, int y)
    {
        for (int i = 0; i < terminals.Length; i++)
        {
            Terminal terminal = terminals[i];

            ConsoleEx.SetPosition(0, y + i);
            ConsoleEx.WriteLine($"Terminal {terminal.Id} \f{terminal.Id} = ");

            int j = 0;
            foreach (Luggage luggage in terminal.Luggages)
            {
                ConsoleEx.WriteCharacter(j + 14, y + i, '■', (byte)(terminal.Id));
                j++;
            }
        }
    }
    static void DrawInfoBuffer()
    {
        Draw.Rectangle(0, 18, ConsoleEx.Width - 1, ConsoleEx.Height - 1, true, '.');
        Draw.ConsoleBuffer(2, 19, infoBuffer);
    }
}