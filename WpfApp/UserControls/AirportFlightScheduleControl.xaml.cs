using BaggageSorteringLib;
using System.Windows;
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

        private void DG_ActiveFlights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = (Flight)DG_ActiveFlights.SelectedItem;

            if (item is Flight)
            {
                Flight flight = (Flight)item;

                TB_Name.Text = flight.Name;
                TB_SeatsAmount.Text = flight.SeatsAmount.ToString();
                TB_Status.Text = flight.Status.ToString();
                TB_ReservationsAmount.Text = flight.Reservations.Count.ToString();

                TB_Arrival.Text = flight.Arrival.ToString("dd-MM | HH:mm");
                TB_Departure.Text = flight.Departure.ToString("dd-MM | HH:mm");
                TB_Destination.Text = flight.Destination;

                LV_Passengers.ItemsSource = flight.Reservations;
            }
        }
    }
}
