using BaggageSorteringLib;
using Engine;

static class AirportDraw
{
    /// <summary>
    /// Used to draw checkin area
    /// </summary>
    public static void CheckinArea(int y, CheckinArea checkinArea)
    {
        ConsoleEx.SetPosition(0, y);
        foreach(Counter c in checkinArea.Counters)
        {
            ConsoleEx.WriteLine($"\f{c.Id+1:X}█ \f7{c}");
        }
    }

    /// <summary>
    /// Used to draw conveyor belt to console
    /// </summary>
    public static void ConveyorBelt(int y, ConveyorBelt<Luggage> conveyorBelt)
    {
        for (int i = 0; i < conveyorBelt.Length; i++)
        {
            Luggage luggage = conveyorBelt.Buffer[i];

            if (luggage != null)
            {
                ConsoleEx.WriteCharacter(i, y, 'G', Color.Get((byte)(luggage.TerminalId + 1), (byte)(luggage.CounterId + 1)));
            }
        }

        ConsoleEx.SetPosition(conveyorBelt.Length + 3, y);
        ConsoleEx.WriteLine("- Sorting system's conveyor belt");
    }

    /// <summary>
    /// Used to draw terminal area
    /// </summary>
    public static void TerminalsArea(int y, TerminalsArea terminalsArea)
    {
        ConsoleEx.SetPosition(0, y);
        foreach (Terminal t in terminalsArea.Terminals)
        {
            ConsoleEx.Write($"\f{t.Id+1:X}G \f7{t}");
            ConsoleEx.CursorX = 32;
            for (int i = 0; i < t.Luggages.Count && i < 50; i++)
            {
                ConsoleEx.Write($"\f{t.Id + 1:X}■");
            }
            ConsoleEx.Write($"\f7 : {t.Luggages.Count}\n");
        }
    }

    /// <summary>
    /// Used to draw flight schedule
    /// </summary>
    public static void FlightSchedule(int y, FlightSchedule flightSchedule)
    {
        Draw.Color = Color.Grey;
        Draw.Rectangle(0, y, ConsoleEx.Width - 1, y + 12 + 2, true, '.');

        ConsoleEx.SetPosition(2, y + 1);
        ConsoleEx.WriteLine("\f3- FLIGHT SCHEDULE");
        ConsoleEx.WriteLine(
            "\f3DEPATURE      " +
            "DESTINATION         " +
            "FLIGHT      " +
            "GATE    " +
            "STATUS                  " +
            "CHECKIN/BOOKED/MAX"
        );
        foreach (Flight f in flightSchedule.ActiveFlights)
        {

            ConsoleEx.Write($"{f.Departure.ToString("HH:mm")}");
            ConsoleEx.CursorX = 14;
            ConsoleEx.Write($"{f.Destination}");
            ConsoleEx.CursorX = 34;
            ConsoleEx.Write($"{f.Name}");
            ConsoleEx.CursorX = 46;
            if (f.IsAtTerminal)
            {
                ConsoleEx.Write(f.Terminal.Id.ToString());
            }
            ConsoleEx.CursorX = 54;
            ConsoleEx.Write($"{f.Status}");
            ConsoleEx.CursorX = 78;
            ConsoleEx.Write($"{f.GetCheckinAmount()}/{f.Reservations.Count}/{f.SeatsAmount}\n");
        }
    }

    /// <summary>
    /// Used to draw info buffer window
    /// </summary>
    public static void InfoBuffer(int y, string title, ConsoleBuffer buffer)
    {
        Draw.Color = Color.Grey;
        Draw.Rectangle(0, y, ConsoleEx.Width - 1, y + buffer.Length + 2, true, '.');
        ConsoleEx.SetPosition(2, y + 1);
        ConsoleEx.WriteLine("\f5- " + title);
        Draw.ConsoleBuffer(2, y + 2, buffer);
    }
}