using BaggageSorteringLib;
using System.Windows.Controls;

namespace WpfApp.UserControls
{
    public partial class AirportTerminalControl : UserControl
    {
        public Terminal Terminal { get; private set; }

        public AirportTerminalControl(Terminal terminal)
        {
            InitializeComponent();

            // Initalize terminal
            Terminal = terminal;
            Rect_Color.Fill = AirportColors.GetColorById(terminal.Id);
            Update();
        }

        /// <summary>
        /// Used to update terminal
        /// </summary>
        public void Update()
        {
            if (Terminal.IsFlightReservedToTerminal)
            {
                Lb_Title.Content = $"Gate {Terminal.Id} - {Terminal.Flight.Status}";
                Lb_Destination.Content = Terminal.Flight.Destination;
                Lb_Name.Content = Terminal.Flight.Name;
                Lb_Depature.Content = Terminal.Flight.Departure.ToShortTimeString();
                Lb_Checkedin.Content = $"Luggages: {Terminal.Luggages.Count}";
            }
            else
            {
                Lb_Title.Content = $"Terminal {Terminal.Id} - Closed";
                Lb_Destination.Content = "";
                Lb_Name.Content = "";
                Lb_Depature.Content = "";
                Lb_Checkedin.Content = "";
            }
        }
    }
}
