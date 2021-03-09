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
            Lines = new ObservableCollection<string>();
            DataContext = this;

            InitializeComponent();
        }

        public void WriteLine(string text)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() =>
            {
                Lines.Insert(0, text);

                if (Lines.Count > 300)
                {
                    Lines.RemoveAt(Lines.Count - 1);
                }
            }));
        }
    }
}
