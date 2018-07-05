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
    /// Logica di interazione per Window_binarioFiltri.xaml
    /// </summary>
    public partial class Window_binarioFiltri : Window
    {
        public string ButtonClicked = "null";

        public int dimensioneBinario = 0;
        public int DimensioneBinario
        {
            get { return dimensioneBinario; }
            set { dimensioneBinario = value; TextBox_lungezza.Text = value.ToString(); }
        }

        private bool utilizzaMisure = false;
        public bool UtilizzaMisure
        {
            get { return utilizzaMisure; }
            set { utilizzaMisure = TextBlock_lunghezza.IsEnabled = TextBox_lungezza.IsEnabled = value; CheckBox_utilizzaMisure.IsChecked = value; }
        }

        public Window_binarioFiltri()
        {
            InitializeComponent();

            TextBlock_lunghezza.IsEnabled = TextBox_lungezza.IsEnabled = utilizzaMisure;
        }

        private void CheckBox_utilizzaMisure_Checked(object sender, RoutedEventArgs e)
        {
            UtilizzaMisure = TextBlock_lunghezza.IsEnabled = TextBox_lungezza.IsEnabled = true;
            CheckBox_utilizzaMisure.IsChecked = true;
        }

        private void CheckBox_utilizzaMisure_Unchecked(object sender, RoutedEventArgs e)
        {
            UtilizzaMisure = TextBlock_lunghezza.IsEnabled = TextBox_lungezza.IsEnabled = false;
            CheckBox_utilizzaMisure.IsChecked = false;
        }

        private void Button_annulla_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked = "annulla";
            Close();
        }

        private void Button_conferma_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(TextBox_lungezza.Text) > 0 || !UtilizzaMisure)
                {
                    DimensioneBinario = Convert.ToInt32(TextBox_lungezza.Text);
                    ButtonClicked = "conferma";
                    Close();
                }
                else
                    MessageBox.Show("Se si sceglie il calcolo sulla base delle misure è necessario impostare una dimensione del binario");
            }
            catch { MessageBox.Show("La dimensione del binario deve essere un numero maggiore di zero"); }
        }
    }
}
