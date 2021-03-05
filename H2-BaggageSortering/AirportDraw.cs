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
                ConsoleEx.WriteCharacter(i, y, 'G', Color.Get((byte)(luggage.TerminalId), (byte)(luggage.CounterId)));
            }
        }
        ConsoleEx.SetPosition(conveyorBelt.Length + 3, y);
        ConsoleEx.WriteLine(name);
    }
    public static void Counters(Counter[] counters, int y)
    {
        ConsoleEx.SetPosition(0, y);
        for (int i = 0; i < counters.Length; i++)
        {
            Counter counter = counters[i];
            ConsoleEx.WriteLine($"  Counter {counter.Id}  {(counter.IsOpen ? "open" : "")} {counter.Flight.Name} {counter.Flight.Destination}");
            ConsoleEx.WriteCharacter(0, y + i, '■', (byte)counter.Id);
        }
    }
    public static void Terminals(Terminal[] terminals, int y)
    {
        for (int i = 0; i < terminals.Length; i++)
        {
            Terminal terminal = terminals[i];

            ConsoleEx.SetPosition(0, y + i);
            ConsoleEx.WriteLine($"  Gate {terminal.Id}");
            ConsoleEx.WriteCharacter(0, y + i, '■', (byte)terminal.Id);

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
        Draw.Color = Color.Grey;
        Draw.Rectangle(0, y, ConsoleEx.Width - 1, y + buffer.Length + 2, true, '.');
        ConsoleEx.SetPosition(2, y + 1);
        ConsoleEx.WriteLine("\f5- " + title);
        Draw.ConsoleBuffer(2, y + 2, buffer);
    }
    public static void FlightSchedule(int y, FlightSchedule flightSchedule)
    {
        Draw.Color = Color.Grey;
        Draw.Rectangle(0, y, ConsoleEx.Width - 1, y + flightSchedule.FlightScreenLength + 2, true, '.');
        ConsoleEx.SetPosition(2, y + 1);
        ConsoleEx.WriteLine("\f3- FLIGHT SCHEDULE");
        ConsoleEx.SetPosition(2, y + 2);
        ConsoleEx.Write("\f3DEPATURE");
        ConsoleEx.SetPosition(16, y + 2);
        ConsoleEx.Write("\f3DESTINATION");
        ConsoleEx.SetPosition(36, y + 2);
        ConsoleEx.Write("\f3FLIGHT");
        ConsoleEx.SetPosition(48, y + 2);
        ConsoleEx.Write("\f3GATE");
        ConsoleEx.SetPosition(56, y + 2);
        ConsoleEx.Write("\f3STATUS");

        if (flightSchedule.FlightScreen.Count > 0)
        {
            for (int i = 0; i < flightSchedule.FlightScreen.Count; i++)
            {
                Flight flight = flightSchedule.FlightScreen[i];
                ConsoleEx.SetPosition(2, y + i + 3);
                ConsoleEx.Write($"{flight.Departure.ToString("HH:mm")}");
                ConsoleEx.SetPosition(16, y + i + 3);
                ConsoleEx.Write($"{flight.Destination}");
                ConsoleEx.SetPosition(36, y + i + 3);
                ConsoleEx.Write($"{flight.Name}");
                ConsoleEx.SetPosition(48, y + i + 3);
                //ConsoleEx.Write($"{flight.Terminal}");
                ConsoleEx.SetPosition(56, y + i + 3);
                ConsoleEx.Write($"{flight.Status} {flight.GetCheckinAmount()}/{flight.Reservations.Count}");
            }
        }
    }
}