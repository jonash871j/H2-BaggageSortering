using BaggageSorteringLib;
using System.Windows.Controls;

namespace WpfApp.UserControls
{
    public partial class AirportOverviewControl : UserControl
    {
        public Simulator Simulator { get; private set; }

        public AirportOverviewControl()
        {
            InitializeComponent();
        }

        public void SetSimulator(Simulator simulator)
        {
            Simulator = simulator;

            foreach (Counter counter in Simulator.CheckinArea.Counters)
            {
                WP_Counters.Children.Add(new AirportCounterControl(counter));
            }
            for (int i = 0; i < Simulator.SortingMachine.ConveyorBelt.Length; i++)
            {
                WP_Luggages.Children.Add(new AirportLuggageControl(i));
            }
            foreach (Terminal terminal in Simulator.TerminalsArea.Terminals)
            {
                WP_Terminals.Children.Add(new AirportTerminalControl(terminal));
            }
        }

        public void Update()
        {
            foreach (AirportCounterControl counterControl in WP_Counters.Children)
            {
                counterControl.Update();
            }
            for (int i = 0; i < WP_Luggages.Children.Count; i++)
            {
                AirportLuggageControl luggageControl = (AirportLuggageControl)WP_Luggages.Children[i];
                luggageControl.Luggage = Simulator.SortingMachine.ConveyorBelt[i];
                luggageControl.Update();
            }
            foreach (AirportTerminalControl terminalControl in WP_Terminals.Children)
            {
                terminalControl.Update();
            }
        }
    }
}
