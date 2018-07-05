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
    /// Logica di interazione per Window_visBinari.xaml
    /// </summary>
    public partial class Window_visBinari : Window
    {
        private Class_binari dataToViw;
        private int currentMargin = 0;
        private int spaceMargin = 380;

        public Window_visBinari(Class_binari dataToViw)
        {
            InitializeComponent();

            this.dataToViw = dataToViw;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            currentMargin = 0;
            for (int i = 0; i < dataToViw.list_binari[0].NumeroProgetti; i++)
            {
                UserControls_visBinari.UserControl_VS_binari userControl_VS_Binari = new UserControls_visBinari.UserControl_VS_binari();
                userControl_VS_Binari.Margin = new Thickness(currentMargin + 10, 37, 0, 0);
                currentMargin += spaceMargin;

                Grid_mainPorogetti.Children.Add(userControl_VS_Binari);
            }
        }
    }
}
