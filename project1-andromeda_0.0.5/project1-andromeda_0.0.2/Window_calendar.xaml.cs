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
    /// Logica di interazione per Window_calendar.xaml
    /// </summary>
    public partial class Window_calendar : Window
    {
        public delegate void CloseDelegate();
        public event CloseDelegate CloseEvent;
        public string CurrentTipe = "none";
        public UserControl_progettoTreno ControlParent;

        private DateTime dataSelezionata = DateTime.Now;
        public DateTime DataSelezionata
        {
            get { return dataSelezionata; }
            set { dataSelezionata = value; Calendar_main.SelectedDate = value; }
        }

        private bool WindowFocus = false;

        public Window_calendar()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Calendar_main.SelectedDate = DataSelezionata;
        }

        private void Calendar_main_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WindowFocus)
            {
                DataSelezionata = (DateTime)Calendar_main.SelectedDate;

                bool corretto = false;
                if (CurrentTipe == "inizio")
                    if (DataSelezionata <= Convert.ToDateTime(ControlParent.ChangeDataFine.Content))
                        corretto = true;
                if (CurrentTipe == "fine")
                    if (DataSelezionata >= Convert.ToDateTime(ControlParent.ChangeDataInizio.Content))
                        corretto = true;
                if (corretto)
                {
                    CloseEvent();
                    Close();
                }
                else
                    MessageBox.Show("Intervallo temporale errato\nLa data di inizio deve essere minore della fine");
            }
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            WindowFocus = true;
        }
    }
}
