using System.Linq;
using System.Windows.Media;

public static class AirportColors
{
    /// <summary>
    /// Used to get color from predefined array by id
    /// </summary>
    public static Brush GetColorById(int id)
    {
        var colors = typeof(Brushes).GetProperties().
        Select(p => new { Brush = p.GetValue(null) as Brush }).
        ToArray();

        if (id + 20 < colors.Length)
        {
            // id + 20 because many of the first colors are gray scaled
            return colors[id + 20].Brush;
        }
        else
        {
            return colors[0].Brush;
        }  
    }
}

