using BaggageSorteringLib;
using System.Windows.Controls;

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
            Simulator.FlightSchedule.FlightInfo += OnGeneralInfo;
            Simulator.FlightSchedule.BadFlightInfo += OnGeneralWarningInfo;
            Simulator.ProcessExceptionInfo += OnGeneralErrorInfo;
        }

        private void OnGeneralErrorInfo(string msg) => CCon_GeneralInfo.WriteLine($"> [ERROR] {msg}");
        private void OnGeneralWarningInfo(string msg) => CCon_GeneralInfo.WriteLine($"> [WARNING] {msg}");
        private void OnGeneralInfo(string msg) => CCon_GeneralInfo.WriteLine($"> {msg}");
        private void OnSortingMachineProcessError(string msg) => CCon_SortingMachineInfo.WriteLine($"> [ERROR] {msg}");
        private void OnSortingMachineProcessInfo(string msg) => CCon_SortingMachineInfo.WriteLine($"> {msg}");
    }
}
