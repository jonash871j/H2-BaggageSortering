using BaggageSorteringLib;
using System;
using System.Windows.Controls;

namespace WpfApp.UserControls
{
    public partial class AirportCounterControl : UserControl
    {
        private static Random rng = new Random(0);
        public Counter Counter { get; private set; }


        public AirportCounterControl(Counter counter)
        {
            Counter = counter;
            InitializeComponent();

            Rect_Color.Fill = AirportColors.GetColorById(counter.Id);

            Update();
        }

        /// <summary>
        /// Used to update counter
        /// </summary>
        public void Update()
        {
            if (Counter.IsOpen)
            {
                Lb_Title.Content = $"Counter {Counter.Id} - Open";
                Lb_Destination.Content = Counter.Flight.Destination;
                Lb_Name.Content = Counter.Flight.Name;
                Lb_Depature.Content = Counter.Flight.Departure.ToShortTimeString();
                Lb_Queue.Content = $"Queue: {Counter.Flight.GetNotCheckinAmount()}";
                Lb_Checkedin.Content = $"Checkedin: {Counter.Flight.GetCheckinAmount()}";
            }
            else
            {
                Lb_Title.Content = $"Counter {Counter.Id} - Closed";
                Lb_Destination.Content = "";
                Lb_Name.Content = "";
                Lb_Depature.Content = "";
                Lb_Queue.Content = "";
                Lb_Checkedin.Content = "";
            }
        }
    }
}
