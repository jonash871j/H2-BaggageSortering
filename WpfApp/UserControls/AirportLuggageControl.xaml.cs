using BaggageSorteringLib;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp.UserControls
{
    public partial class AirportLuggageControl : UserControl
    {
        public Luggage Luggage { get; set; } = null;

        public AirportLuggageControl(int index)
        {
            InitializeComponent();
            Lb_Index.Content = index;
            Update();
        }

        public void Update()
        {
            if (Luggage != null)
            {
                Rect_Counter.Fill = AirportColors.GetColorById(Luggage.CounterId);
                Rect_Terminal.Fill = AirportColors.GetColorById(Luggage.TerminalId);
            }
            else
            {
                Rect_Counter.Fill = Brushes.Black;
                Rect_Terminal.Fill = Brushes.Black;
            }
        }
    }
}
