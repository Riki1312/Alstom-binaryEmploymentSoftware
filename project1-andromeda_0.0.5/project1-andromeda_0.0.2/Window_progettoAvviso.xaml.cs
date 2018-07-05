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
    /// Logica di interazione per Window_progettoAvviso.xaml
    /// </summary>
    public partial class Window_progettoAvviso : Window
    {
        public Window_progettoAvviso()
        {
            InitializeComponent();
        }

        private void Button_conferma_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
