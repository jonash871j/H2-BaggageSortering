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

        /// <summary>
        /// Used to set simulator and initialize control
        /// </summary>
        public void Initialize(Simulator simulator)
        {
            Simulator = simulator;

            // Adds counters to wrap panel
            foreach (Counter counter in Simulator.CheckinArea.Counters)
            {
                WP_Counters.Children.Add(new AirportCounterControl(counter));
            }
            // Adds conveyorbelt field to wrap panel
            for (int i = 0; i < Simulator.SortingMachine.ConveyorBelt.Length; i++)
            {
                WP_Luggages.Children.Add(new AirportLuggageControl(i));
            }
            // Adds terminals to wrap panel
            foreach (Terminal terminal in Simulator.TerminalsArea.Terminals)
            {
                WP_Terminals.Children.Add(new AirportTerminalControl(terminal));
            }
        }

        /// <summary>
        /// Used to update counters, conveyor belt and terminals
        /// </summary>
        public void Update()
        {
            // Updates all counters
            foreach (AirportCounterControl counterControl in WP_Counters.Children)
            {
                counterControl.Update();
            }
            // Updates conveyor belt
            for (int i = 0; i < WP_Luggages.Children.Count; i++)
            {
                AirportLuggageControl luggageControl = (AirportLuggageControl)WP_Luggages.Children[i];
                luggageControl.Update(Simulator.SortingMachine.ConveyorBelt[i]);
            }
            // Updates terminals
            foreach (AirportTerminalControl terminalControl in WP_Terminals.Children)
            {
                terminalControl.Update();
            }
        }
    }
}