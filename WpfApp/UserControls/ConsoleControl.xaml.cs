using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp.UserControls
{
    public partial class ConsoleControl : UserControl
    {
        public ObservableCollection<string> Lines { get; set; }

        public ConsoleControl()
        {
            InitializeComponent();

            // Initialize console control
            Lines = new ObservableCollection<string>();
            DataContext = this;
        }

        /// <summary>
        /// Used to write a new message to the console control 
        /// </summary>
        public void WriteLine(string line)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() =>
            {
                Lines.Insert(0, line);

                if (Lines.Count > 300)
                {
                    Lines.RemoveAt(Lines.Count - 1);
                }
            }));
        }
    }
}
