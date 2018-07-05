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
using System.Windows.Shapes;

namespace project1_andromeda_0._0._2
{
    /// <summary>
    /// Logica di interazione per Window_message.xaml
    /// </summary>
    public partial class Window_message : Window
    {
        public string ButtonClicked = "null";

        public string message
        {
            get { return Text_message.Text; }
            set { Text_message.Text = value; }
        }

        public Window_message()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_conferma_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked = "conferma";
            Close();
        }

        private void Button_annulla_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked = "annulla";
            Close();
        }
    }
}
