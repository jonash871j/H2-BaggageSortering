using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace WpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for ConsoleControl.xaml
    /// </summary>
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
            Dispatcher.BeginInvoke(new Action(() => WriteLineAction(text)), DispatcherPriority.SystemIdle);
        }

        private void WriteLineAction(string text)
        {
            Lines.Insert(0, text);

            if (Lines.Count > 300)
            {
                Lines.RemoveAt(Lines.Count - 1);
            }
        }
    }
}
