using BaggageSorteringLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Simulator Simulator { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            Simulator = new Simulator(
                counterAmount: 10,
                terminalAmount: 15,
                conveyorBeltLength: 20,
                flightScreenLength: 12
            );

            Simulator.IsAutoGenereatedReservationsEnabled = true;
            AOCon_AirportOverview.Simulator = Simulator;
            ACCon_Consoles.SetSimulator(Simulator);
            Simulator.Start();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Simulator.Stop();
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
