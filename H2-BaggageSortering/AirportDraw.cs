using BaggageSorteringLib;
using Engine;

static class AirportDraw
{
    public static void ConveyorBelt(ConveyorBelt<Luggage> conveyorBelt, string name, int y)
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
    public static void Counters(Counter[] counters, int y)
    {
        ConsoleEx.SetPosition(0, y);
        foreach (Counter counter in counters)
        {
            ConsoleEx.WriteLine($"Counter {counter.Id}: IsReady {counter.IsReady()}");
        }
    }
    public static void Terminals(Terminal[] terminals, int y)
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
    public static void InfoBuffer(int y, string title, ConsoleBuffer buffer)
    {
        Draw.Rectangle(0, y, ConsoleEx.Width - 1, y + buffer.Length + 2, true, '.');
        ConsoleEx.SetPosition(2, y + 1);
        ConsoleEx.WriteLine("- " + title);
        Draw.ConsoleBuffer(2, y + 2, buffer);
    }
    public static void FlightSchedule(int y, FlightSchedule flightSchedule)
    {
        Draw.Rectangle(0, y, ConsoleEx.Width - 1, y + flightSchedule.FlightScreenLength + 2, true, '.');
        ConsoleEx.SetPosition(2, y + 1);
        ConsoleEx.WriteLine("- Flight schedule");
        if (flightSchedule.FlightScreen.Count > 0)
        {
            foreach (Flight flight in flightSchedule.FlightScreen)
            {
                ConsoleEx.WriteLine(flight.ToString());
            }
        }
    }
}