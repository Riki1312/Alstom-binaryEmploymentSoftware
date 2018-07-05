using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace project1_andromeda_0._0._2
{
    /// <summary>
    /// Logica di interazione per Window_loading.xaml
    /// </summary>
    public partial class Window_loading : Window
    {
        private DispatcherTimer timerToClose = new DispatcherTimer();

        public Window_loading()
        {
            InitializeComponent();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            timerToClose.Interval = new TimeSpan(0, 0, 3/*5*/);
            timerToClose.Tick += new EventHandler(timer_Tick);
            timerToClose.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
