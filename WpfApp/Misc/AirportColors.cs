using System.Linq;
using System.Windows.Media;

public static class AirportColors
{
    public static Brush GetColorById(int id)
    {
        var values = typeof(Brushes).GetProperties().
        Select(p => new { Brush = p.GetValue(null) as Brush }).
        ToArray();
        return values[id + 20].Brush;
    }
}

