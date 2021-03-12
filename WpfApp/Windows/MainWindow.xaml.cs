using BaggageSorteringLib;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public Simulator Simulator { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            InitalizeSimulator();
        }

        private void InitalizeSimulator()
        {
            // Initalizes simulator
            Simulator = new Simulator(
                 counterAmount: 15,
                 terminalAmount: 20,
                 conveyorBeltLength: 23
             );
            Simulator.IsAutoGenereatedReservationsEnabled = true;
            AOCon_AirportOverview.Initialize(Simulator);
            AFSCon_FlightSchedule.Intialize(Simulator);
            ACCon_Consoles.Initalize(Simulator);
            Simulator.Start();

            // Starts dispatcherT timer
            new DispatcherTimer(
                TimeSpan.FromSeconds(0.01), 
                DispatcherPriority.Normal, 
                OnTick,
                Dispatcher.CurrentDispatcher
            );
        }
        private void OnTick(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(Simulator))
            {
                // Updates airport overview: counters, conveyorbelt and terminals
                AOCon_AirportOverview.Update();

                // Updates status bar
                SBI_Time.Content = $" {Simulator.Time.DateTime.ToString("dd-MM | HH:mm")}  ";
                SBI_Speed.Content = $"Speed {Simulator.Time.Speed}x  ";
                SBI_Bustle.Content = $"Bustle lvl {Simulator.BustleLevel} ";

                Monitor.PulseAll(Simulator);
                Monitor.Exit(Simulator);
            }
        }

        private void Window_Closed(object sender, EventArgs e) => Simulator.Stop();

        private void MI_Speed1x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 1;
        private void MI_Speed2x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 2;
        private void MI_Speed4x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 4;
        private void MI_Speed8x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 8;
        private void MI_Speed16x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 16;
        private void MI_Speed32x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 32;
        private void MI_Speed64x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 64;
        private void MI_Speed128x_Click(object sender, RoutedEventArgs e) => Simulator.Time.Speed = 128;

        private void MI_BustleLvl1_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 1;
        private void MI_BustleLvl2_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 2;
        private void MI_BustleLvl3_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 3;
        private void MI_BustleLvl4_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 4;
        private void MI_BustleLvl5_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 5;
        private void MI_BustleLvl6_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 6;
        private void MI_BustleLvl7_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 7;
        private void MI_BustleLvl8_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 8;
        private void MI_BustleLvl9_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 9;
        private void MI_BustleLvl10_Click(object sender, RoutedEventArgs e) => Simulator.BustleLevel = 10;

        private void MI_About_Click(object sender, RoutedEventArgs e) => MessageBox.Show("By Jonas");
        private void MI_Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}