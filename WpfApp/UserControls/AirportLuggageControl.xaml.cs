using BaggageSorteringLib;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp.UserControls
{
    public partial class AirportLuggageControl : UserControl
    {
        public AirportLuggageControl(int index)
        {
            InitializeComponent();
            Lb_Index.Content = index;
        }

        /// <summary>
        /// Used to update luggage control
        /// </summary>
        public void Update(Luggage luggage)
        {
            if (luggage != null)
            {
                Rect_Counter.Fill = AirportColors.GetColorById(luggage.CounterId);
                Rect_Terminal.Fill = AirportColors.GetColorById(luggage.TerminalId);
            }
            else
            {
                Rect_Counter.Fill = Brushes.Black;
                Rect_Terminal.Fill = Brushes.Black;
            }
        }
    }
}
