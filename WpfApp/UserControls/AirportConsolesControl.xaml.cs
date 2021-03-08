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

namespace WpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for AirportConsolesControl.xaml
    /// </summary>
    public partial class AirportConsolesControl : UserControl
    {
        public Simulator Simulator { get; private set; }

        public AirportConsolesControl()
        {
            InitializeComponent();

   
        }

        public void SetSimulator(Simulator simulator)
        {
            Simulator = simulator;
            Simulator.SortingMachine.ProcessInfo += OnSortingMachineProcessInfo;
            Simulator.SortingMachine.ProcessExceptionInfo += OnSortingMachineProcessError;
            Simulator.FlightSchedule.AutoReservationsInfo += OnGeneralInfo;
            Simulator.FlightSchedule.FlightInfo += OnGeneralInfo;
            Simulator.FlightSchedule.BadFlightInfo += OnGeneralWarningInfo;
            Simulator.ProcessExceptionInfo = OnGeneralErrorInfo;
        }

        private void OnGeneralErrorInfo(string msg)
        {
            CCon_GeneralInfo.WriteLine($"> [ERROR] {msg}");
        }

        private void OnGeneralWarningInfo(string msg)
        {
            CCon_GeneralInfo.WriteLine($"> [WARNING] {msg}");
        }

        private void OnGeneralInfo(string msg)
        {
            CCon_GeneralInfo.WriteLine($"> {msg}");
        }

        private void OnSortingMachineProcessError(string msg)
        {
            CCon_SortingMachineInfo.WriteLine($"> [ERROR] {msg}");
        }

        private void OnSortingMachineProcessInfo(string msg)
        {
            CCon_SortingMachineInfo.WriteLine($"> {msg}");
        }
    }
}
