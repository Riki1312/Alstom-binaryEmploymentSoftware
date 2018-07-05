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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project1_andromeda_0._0._2
{
    /// <summary>
    /// Logica di interazione per UserControl_binario.xaml
    /// </summary>
    public partial class UserControl_binario : UserControl
    {
        public Class_progettiTreni class_ProgettiTreni;

        public delegate void RemoveDelegate(UserControl_binario userControl);
        public event RemoveDelegate RemoveEvent;
        public delegate void UpdateDelegate(UserControl_binario userControl);
        public event UpdateDelegate UpdateEvent;

        public bool UtilizzareMisure = false;
        public int DimensioneBinario = 0;
        public string Commenti = "";

        public UserControl_binario()
        {
            InitializeComponent();

            UpdateEvent += (UserControl_binario x) => { };
            TextBlock_dimensione.Visibility = Visibility.Collapsed;

            class_ProgettiTreni = new Class_progettiTreni(ScrollGrid);
        }

        private void AddProgetto_Click(object sender, RoutedEventArgs e)
        {
            class_ProgettiTreni.Add(new Class_progettoTreno());
        }

        private void FiltriBinario_Click(object sender, RoutedEventArgs e)
        {
            Window_binarioFiltri windowFiltri = new Window_binarioFiltri();
            windowFiltri.UtilizzaMisure = UtilizzareMisure;
            windowFiltri.DimensioneBinario = DimensioneBinario;
            windowFiltri.TextCommenti.Document.Blocks.Clear(); windowFiltri.TextCommenti.Document.Blocks.Add(new Paragraph(new Run(Commenti)));

            windowFiltri.ShowDialog();

            if (windowFiltri.ButtonClicked == "conferma")
            {
                UtilizzareMisure = windowFiltri.UtilizzaMisure;
                DimensioneBinario = windowFiltri.DimensioneBinario;
                Commenti = new TextRange(windowFiltri.TextCommenti.Document.ContentStart, windowFiltri.TextCommenti.Document.ContentEnd).Text;

                if (UtilizzareMisure)
                {
                    class_ProgettiTreni.list_progettiTreni.ForEach(x => x.userControl.SetAvviso = "Il binario è impostato per utilizzare il calcolo dei conflitti basato sulle misure \nma non è stata impostata la dimensione di questo treno.");
                    TextBlock_dimensione.Text = "(" + DimensioneBinario + " m)";
                    TextBlock_dimensione.Visibility = Visibility.Visible;

                    class_ProgettiTreni.list_progettiTreni.ForEach(x => x.userControl.TextBlock_dimensione.Visibility = Visibility.Visible);
                }
                else
                {
                    class_ProgettiTreni.list_progettiTreni.ForEach(x => x.userControl.SetAvviso = null);
                    TextBlock_dimensione.Visibility = Visibility.Collapsed;

                    class_ProgettiTreni.list_progettiTreni.ForEach(x => x.userControl.TextBlock_dimensione.Visibility = Visibility.Hidden);
                }

                class_ProgettiTreni.FindConflitti();
            }
        }

        private void EliminaBinario_Click(object sender, RoutedEventArgs e)
        {
            Window_message window_Message = new Window_message();
            window_Message.message = "Sei sicuro di voler eliminare il binario ''" + NomeBinario.Text + "''\nVerranno eliminati anche tutti i progetti interni a esso. \nQuesta operazione non è reversibile.";
            window_Message.ShowDialog();

            if (window_Message.ButtonClicked == "conferma")
            {
                RemoveEvent(this);
                ((Grid)Parent).Children.Remove(this);
            }
        }

        private void NomeBinario_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateEvent(this);
        }
    }
}
