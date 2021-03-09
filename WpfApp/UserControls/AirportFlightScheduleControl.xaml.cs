using BaggageSorteringLib;
using System.Windows.Controls;

namespace WpfApp.UserControls
{
    public partial class AirportFlightScheduleControl : UserControl
    {
        public Simulator Simulator { get; private set; }

        public AirportFlightScheduleControl()
        {
            InitializeComponent();
        }

        public void SetSimulator(Simulator simulator)
        {
            Simulator = simulator;
        }

        public void Update()
        {
            DG_ActiveFlights.ItemsSource = Simulator.FlightSchedule.ActiveFlights;
            DG_ActiveFlights.Items.Refresh();
        }
    }
}
