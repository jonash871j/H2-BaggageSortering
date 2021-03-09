using BaggageSorteringLib;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Simulator Simulator { get; private set; }
        public int slowCounter = 100;

        public MainWindow()
        {
            InitializeComponent();

            Simulator = new Simulator(
                counterAmount: 10,
                terminalAmount: 15,
                conveyorBeltLength: 20
            );

            Simulator.IsAutoGenereatedReservationsEnabled = true;
            AOCon_AirportOverview.SetSimulator(Simulator);
            ACCon_Consoles.SetSimulator(Simulator);
            AFSCon_FlightSchedule.SetSimulator(Simulator);
            Simulator.Start();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += OnTick;
            timer.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Simulator.Stop();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(Simulator))
            {
                if (slowCounter >= 100)
                {
                    AFSCon_FlightSchedule.Update();
                    slowCounter = 0;
                }
                AOCon_AirportOverview.Update();

                SBI_Time.Content = $" {Simulator.Time.DateTime.ToString("dd-MM | HH:mm")}  ";
                SBI_Speed.Content = $"Speed {Simulator.Time.Speed}x  ";
                SBI_Bustle.Content = $"Bustle lvl {Simulator.BustleLevel} ";

                Monitor.PulseAll(Simulator);
                Monitor.Exit(Simulator);
            }
            slowCounter++;
        }

        private void MI_Speed1x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 1;
        private void MI_Speed2x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 2;
        private void MI_Speed4x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 4;
        private void MI_Speed8x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 8;
        private void MI_Speed16x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 16;
        private void MI_Speed32x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 32;
        private void MI_Speed64x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 64;
        private void MI_Speed128x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 128;

        private void MI_About_Click(object sender, RoutedEventArgs e) => MessageBox.Show("By Jonas");
        private void MI_Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
