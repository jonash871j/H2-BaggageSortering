using BaggageSorteringLib;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp.UserControls
{
    public partial class AirportFlightScheduleControl : UserControl
    {
        public Simulator Simulator { get; private set; }

        public AirportFlightScheduleControl()
        {
            InitializeComponent();
        }

        public void Intialize(Simulator simulator)
        {
            Simulator = simulator;
            Simulator.FlightSchedule.FlightScheduleUpdate += OnFlightScheduleUpdate;
        }

        /// <summary>
        /// Used to update flight schedule
        /// </summary>
        private void OnFlightScheduleUpdate()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() =>
            {
                DG_ActiveFlights.ItemsSource = Simulator.FlightSchedule.ActiveFlights;
                DG_ActiveFlights.Items.Refresh();
            }));
        }

        /// <summary>
        /// Used to update flight group box on selection
        /// </summary>
        private void DG_ActiveFlights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DG_ActiveFlights.SelectedItem is Flight f)
            {
                TB_Name.Text = f.Name;
                TB_SeatsAmount.Text = f.SeatsAmount.ToString();
                TB_Status.Text = f.Status.ToString();
                TB_ReservationsAmount.Text = f.Reservations.Count.ToString();

                TB_Arrival.Text = f.Arrival.ToString("dd-MM | HH:mm");
                TB_Departure.Text = f.Departure.ToString("dd-MM | HH:mm");
                TB_Destination.Text = f.Destination;

                LV_Passengers.ItemsSource = f.Reservations;
            }
        }
    }
}
