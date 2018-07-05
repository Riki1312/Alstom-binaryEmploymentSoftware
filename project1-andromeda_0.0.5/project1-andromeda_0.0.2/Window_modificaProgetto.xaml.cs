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
    /// Logica di interazione per Window_modificaProgetto.xaml
    /// </summary>
    public partial class Window_modificaProgetto : Window
    {
        public delegate void CloseDelegate(Window window);
        public event CloseDelegate CloseEvent;

        public string ButtonClicked = "null";

        private int dimensioneProgetto = 0;
        public int DimensioneProgetto { get => dimensioneProgetto; set { dimensioneProgetto = value; TextBoxDimensione.Text = value.ToString(); } }
        public int DImensioneBinario = 0;

        private string nomeProgetto;
        public string NomeProgetto
        {
            get { return nomeProgetto; }
            set { nomeProgetto = value; TextBox_nomeProgetto.Text = value; }
        }
        private string coloreProgetto;
        public string ColoreProgetto
        {
            get { return coloreProgetto; }
            set { coloreProgetto = value; ComboBox_colore.Text = value; }
        }
        private DateTime dataInizio = DateTime.Now;
        public DateTime DataInizio
        {
            get { return dataInizio; }
            set { dataInizio = value; Calendar_inizio.SelectedDate = value; }
        }
        private DateTime dataFine = DateTime.Now;
        public DateTime DataFine
        {
            get { return dataFine; }
            set { dataFine = value; Calendar_fine.SelectedDate = value; }
        }

        public Window_modificaProgetto()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            TextBox_nomeProgetto.Text = NomeProgetto;
            Text_dataInizio.Text = dataInizio.ToString("dd/MM/yyyy");
            Text_datafine.Text = dataFine.ToString("dd/MM/yyyy");
            Calendar_inizio.SelectedDate = dataInizio;
            Calendar_fine.SelectedDate = dataFine;

            ComboBox_colore.Items.Add("Rosso");
            ComboBox_colore.Items.Add("Verde");
            ComboBox_colore.Items.Add("Blu");
            ComboBox_colore.Items.Add("Viola");
            ComboBox_colore.Items.Add("Giallo");
            ComboBox_colore.Items.Add("Azzurro");
            ComboBox_colore.Items.Add("Marrone");
            ComboBox_colore.Items.Add("Nero");
            ComboBox_colore.SelectedIndex = ComboBox_colore.Items.Count - 1;
        }

        private void Calendar_inizio_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Text_dataInizio.Text = Calendar_inizio.SelectedDate.ToString().Remove(10, 9);
            dataInizio = (DateTime)Calendar_inizio.SelectedDate;
        }

        private void Calendar_fine_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Text_datafine.Text = Calendar_fine.SelectedDate.ToString().Remove(10, 9);
            dataFine = (DateTime)Calendar_fine.SelectedDate;
        }

        private void Button_annulla_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked = "annulla";
            CloseEvent(this);
            Close();
        }

        private void Button_conferma_Click(object sender, RoutedEventArgs e)
        {
            if (Calendar_inizio.SelectedDate <= Calendar_fine.SelectedDate)
            {
                if (TextBox_nomeProgetto.Text != "")
                {
                    try
                    {
                        if ((Convert.ToInt32(TextBoxDimensione.Text) > 0 && Convert.ToInt32(TextBoxDimensione.Text) <= DImensioneBinario) || !TextBoxDimensione.IsEnabled)
                        {
                            dimensioneProgetto = Convert.ToInt32(TextBoxDimensione.Text);

                            ButtonClicked = "conferma";
                            nomeProgetto = TextBox_nomeProgetto.Text;
                            coloreProgetto = ComboBox_colore.Text;

                            CloseEvent(this);
                            Close();
                        }
                        else
                            MessageBox.Show("La dimensione del progetto deve essere un numero maggiore di zero e minore della dimensione del binario");
                    }
                    catch { MessageBox.Show("Se si sceglie il calcolo sulla base delle misure è necessario impostare una dimensione del progetto"); }
                }
                else
                    MessageBox.Show("Assegnare un nome al progetto");
            }
            else
                MessageBox.Show("Intervallo temporale errato\nLa data di inizio deve essere minore della fine");
        }
    }
}
