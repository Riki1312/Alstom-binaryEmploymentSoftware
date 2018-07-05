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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project1_andromeda_0._0._2
{
    /// <summary>
    /// Logica di interazione per UserControl_progettoTreno.xaml
    /// </summary>
    public partial class UserControl_progettoTreno : UserControl
    {
        public delegate void RemoveDelegate(UserControl_progettoTreno userControl);
        public event RemoveDelegate RemoveEvent;
        public delegate void UpdateDelegate(UserControl_progettoTreno userControl);
        public event UpdateDelegate UpdateEvent;

        public delegate void NewSelectionDelegate(Class_progettoTreno sender);
        public event NewSelectionDelegate EventNewSelection;
        public event NewSelectionDelegate EventNewDeselection;

        private Window_calendar window_Calendar;
        private int LeftMarginProgettiCollidenti = 200;
        public int DimensioneProgetto = 0;

        public bool Selected
        {
            get { return (bool)CheckBox_progettoSelezionato.IsChecked; }
            set { CheckBox_progettoSelezionato.IsChecked = value; }
        }

        public Class_progettoTreno thisProgetto;    //Riferimento all'oggetto in cui è contenuto questo control.
        public string currentColor = "Nero";

        private bool conflitto;
        public bool Conflitto
        {
            get { return conflitto; }
            set { conflitto = value; if (value) Button_conflitto.Visibility = Visibility.Visible; else Button_conflitto.Visibility = Visibility.Collapsed; }
        }

        private string textAvviso = null;
        public string SetAvviso
        {
            get { return textAvviso; }
            set { textAvviso = value; if (value != null) Button_avviso.Visibility = Visibility.Visible; else Button_avviso.Visibility = Visibility.Collapsed; }
        }

        public UserControl_progettoTreno()
        {
            InitializeComponent();

            Conflitto = false;
            UpdateEvent += (UserControl_progettoTreno x) => { };

            ChangeDataFine.Content = ChangeDataInizio.Content = DateTime.Now.ToString("dd/MM/yyyy");

            UpdateDurata();
        }

        public bool ChiediConfermaElimina = true;
        public void EliminaProgetto_Click(object sender, RoutedEventArgs e)
        {
            if (ChiediConfermaElimina)
            {
                Window_message window_Message = new Window_message();
                window_Message.message = "Sei sicuro di voler eliminare il progetto ''" + GroupBox_nomeProgetto.Header + "''\nQuesta operazione non è reversibile.";
                window_Message.ShowDialog();

                if (window_Message.ButtonClicked == "conferma")
                {
                    RemoveEvent(this);
                    ((Grid)Parent).Children.Remove(this);
                }
            }
            else
            {
                RemoveEvent(this);
                ((Grid)Parent).Children.Remove(this);
            }

            thisProgetto.thisProgettiTreni.FindConflitti();
        }

        private void ModificaProgetto_Click(object sender, RoutedEventArgs e)
        {
            Window_modificaProgetto window_ModificaProgetto = new Window_modificaProgetto();

            window_ModificaProgetto.CloseEvent += CloseWindowModifica;
            window_ModificaProgetto.DataInizio = Convert.ToDateTime(ChangeDataInizio.Content);
            window_ModificaProgetto.DataFine = Convert.ToDateTime(ChangeDataFine.Content);
            window_ModificaProgetto.NomeProgetto = GroupBox_nomeProgetto.Header.ToString();
            window_ModificaProgetto.ColoreProgetto = currentColor;
            window_ModificaProgetto.TextBoxDimensione.IsEnabled = window_ModificaProgetto.TextBlockDimensione.IsEnabled = thisProgetto.thisProgettiTreni.thisBinario.userControl.UtilizzareMisure;
            window_ModificaProgetto.DimensioneProgetto = DimensioneProgetto;
            window_ModificaProgetto.DImensioneBinario = thisProgetto.thisProgettiTreni.thisBinario.userControl.DimensioneBinario;
            SetAvviso = null;

            window_ModificaProgetto.ShowDialog();
        }

        private void ChangeDataFine_Click(object sender, RoutedEventArgs e)
        {
            window_Calendar = new Window_calendar();
            window_Calendar.CloseEvent += CloseWindowCaledar;
            window_Calendar.ControlParent = this;

            window_Calendar.DataSelezionata = Convert.ToDateTime(((Button)sender).Content);
            window_Calendar.CurrentTipe = "fine";
            window_Calendar.ShowDialog();
        }

        private void ChangeDataInizio_Click(object sender, RoutedEventArgs e)
        {
            window_Calendar = new Window_calendar();
            window_Calendar.CloseEvent += CloseWindowCaledar;
            window_Calendar.ControlParent = this;

            window_Calendar.DataSelezionata = Convert.ToDateTime(((Button)sender).Content);
            window_Calendar.CurrentTipe = "inizio";
            window_Calendar.ShowDialog();
        }

        private void CloseWindowCaledar()
        {
            if (window_Calendar.CurrentTipe == "inizio")
                ChangeDataInizio.Content = window_Calendar.DataSelezionata.ToString("dd/MM/yyyy");
            else if (window_Calendar.CurrentTipe == "fine")
                ChangeDataFine.Content = window_Calendar.DataSelezionata.ToString("dd/MM/yyyy");

            UpdateDurata();
        }

        private void CloseWindowModifica(Window window)
        {
            if (((Window_modificaProgetto)window).ButtonClicked == "conferma")
            {
                ChangeDataInizio.Content = ((Window_modificaProgetto)window).DataInizio.ToString("dd/MM/yyyy");
                ChangeDataFine.Content = ((Window_modificaProgetto)window).DataFine.ToString("dd/MM/yyyy");
                GroupBox_nomeProgetto.Header = ((Window_modificaProgetto)window).NomeProgetto;
                currentColor = ((Window_modificaProgetto)window).ColoreProgetto;
                DimensioneProgetto = ((Window_modificaProgetto)window).DimensioneProgetto;
                TextBlock_dimensione.Text = DimensioneProgetto + " m";

                UpdateDurata();

                CalcolaColore();
            }
        }

        public void CalcolaColore()
        {
            switch (currentColor)
            {
                case "Rosso": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(254, 0, 0)); break;
                case "Verde": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 204, 41)); break;
                case "Blu": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(25, 16, 204)); break;
                case "Viola": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(170, 13, 128)); break;
                case "Giallo": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(249, 245, 7)); break;
                case "Azzurro": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(7, 249, 220)); break;
                case "Marrone": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(124, 60, 19)); break;
                case "Nero": GroupBox_nomeProgetto.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)); break;
            }
        }

        private void Button_conflitto_Click(object sender, RoutedEventArgs e)
        {
            Window_conflitto window_Conflitto = new Window_conflitto();

            Class_progettoTreno progettoCorrente = DuplicateProgetto(thisProgetto, true);
            window_Conflitto.Grid_progettoSelezionato.Children.Add(progettoCorrente.userControl);

            int currentMargin = 0;
            for (int i = 0; i < thisProgetto.collisioni.progettiCollidenti.Count; i++)
            {
                Class_progettoTreno pCollidente = DuplicateProgetto(thisProgetto.collisioni.progettiCollidenti[i], true);
                pCollidente.userControl.Margin = new Thickness(currentMargin, 0, 0, 0);
                window_Conflitto.Grid_progettiCollidenti.Children.Add(pCollidente.userControl);

                currentMargin += LeftMarginProgettiCollidenti;
            }
            window_Conflitto.TextBlock_npCollidenti.Text = "Progetti collidenti: (" + window_Conflitto.Grid_progettiCollidenti.Children.Count + ")";

            window_Conflitto.ShowDialog();

            if (window_Conflitto.ButtonClicked == "conferma")
            {
                //Recuperare modifiche progetti collidenti (ciclare al contrario).
                for (int i = window_Conflitto.Grid_progettiCollidenti.Children.Count - 1; i >= 0; i--)
                {
                    thisProgetto.collisioni.progettiCollidenti[i].NomeProgetto = ((UserControl_progettoTreno)window_Conflitto.Grid_progettiCollidenti.Children[i]).thisProgetto.NomeProgetto;
                    thisProgetto.collisioni.progettiCollidenti[i].Colore = ((UserControl_progettoTreno)window_Conflitto.Grid_progettiCollidenti.Children[i]).thisProgetto.Colore;
                    thisProgetto.collisioni.progettiCollidenti[i].Commenti = ((UserControl_progettoTreno)window_Conflitto.Grid_progettiCollidenti.Children[i]).thisProgetto.Commenti;
                    thisProgetto.collisioni.progettiCollidenti[i].DataFine = ((UserControl_progettoTreno)window_Conflitto.Grid_progettiCollidenti.Children[i]).thisProgetto.DataFine;
                    thisProgetto.collisioni.progettiCollidenti[i].DataInizio = ((UserControl_progettoTreno)window_Conflitto.Grid_progettiCollidenti.Children[i]).thisProgetto.DataInizio;
                }

                //Recuperare e salvare le modifiche (effettuare dopo le modifiche ai collidenti).
                thisProgetto.Colore = ((UserControl_progettoTreno)window_Conflitto.Grid_progettoSelezionato.Children[0]).thisProgetto.Colore;
                thisProgetto.Commenti = ((UserControl_progettoTreno)window_Conflitto.Grid_progettoSelezionato.Children[0]).thisProgetto.Commenti;
                thisProgetto.DataFine = ((UserControl_progettoTreno)window_Conflitto.Grid_progettoSelezionato.Children[0]).thisProgetto.DataFine;
                thisProgetto.DataInizio = ((UserControl_progettoTreno)window_Conflitto.Grid_progettoSelezionato.Children[0]).thisProgetto.DataInizio;
                thisProgetto.NomeProgetto = ((UserControl_progettoTreno)window_Conflitto.Grid_progettoSelezionato.Children[0]).thisProgetto.NomeProgetto;
            }
        }
        private Class_progettoTreno DuplicateProgetto(Class_progettoTreno progettoTreno, bool anteprima = false)
        {
            Class_progettoTreno newProgetto = new Class_progettoTreno();
            newProgetto.Collide = progettoTreno.Collide;
            newProgetto.collisioni = progettoTreno.collisioni;
            newProgetto.Colore = progettoTreno.Colore;
            newProgetto.Commenti = progettoTreno.Commenti;
            newProgetto.DataFine = progettoTreno.DataFine;
            newProgetto.DataInizio = progettoTreno.DataInizio;
            newProgetto.NomeProgetto = progettoTreno.NomeProgetto;
            newProgetto.zIndex = progettoTreno.zIndex;
            newProgetto.thisProgettiTreni = progettoTreno.thisProgettiTreni;
            newProgetto.DimensioneProgetto = progettoTreno.DimensioneProgetto;

            if (anteprima)
                newProgetto.userControl.SetAnteprima();

            return newProgetto;
        }

        public void SetAnteprima()
        {
            Button_conflitto.Visibility = Visibility.Hidden;
            CheckBox_progettoSelezionato.Visibility = Visibility.Hidden;

            Button_avviso.Visibility = Visibility.Hidden;
            SetAvviso = null;

            //Risolvere problema elimina
        }

        public void UpdateDurata()
        {
            TimeSpan durata = Convert.ToDateTime(ChangeDataFine.Content) - Convert.ToDateTime(ChangeDataInizio.Content);
            TextBox_numeroGiorni.Text = durata.TotalDays.ToString();

            UpdateEvent(this);
        }

        private void CheckBox_progettoSelezionato_Checked(object sender, RoutedEventArgs e)
        {
            GroupBox_nomeProgetto.Effect = new DropShadowEffect {
                    Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                    Direction = 320,
                    Opacity = 0.35,
                    BlurRadius = 10
                };

            Selected = (bool)CheckBox_progettoSelezionato.IsChecked;
            EventNewSelection(thisProgetto);
        }

        private void CheckBox_progettoSelezionato_Unchecked(object sender, RoutedEventArgs e)
        {
            GroupBox_nomeProgetto.ClearValue(EffectProperty);

            Selected = (bool)CheckBox_progettoSelezionato.IsChecked;
            EventNewDeselection(thisProgetto);
        }

        private void Button_avviso_Click(object sender, RoutedEventArgs e)
        {
            Window_progettoAvviso window_ProgettoAvviso = new Window_progettoAvviso();
            window_ProgettoAvviso.TextBlock_messaggio.Text = textAvviso;
            window_ProgettoAvviso.ShowDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!thisProgetto.thisProgettiTreni.thisBinario.UtilizzaMisure)
            {
                Button_avviso.Visibility = Visibility.Collapsed;
                TextBlock_dimensione.Visibility = Visibility.Hidden;
            }
            else if (DimensioneProgetto == 0)
                SetAvviso = "Il binario è impostato per utilizzare il calcolo dei conflitti basato sulle misure \nma non è stata impostata la dimensione di questo treno.";

            //Set eventi per selezione
            EventNewSelection += thisProgetto.thisProgettiTreni.thisBinario.thisBinari.NewSelectionEvent;
            EventNewDeselection += thisProgetto.thisProgettiTreni.thisBinario.thisBinari.NewDeselectionEvent;
        }

        private void TextBox_numeroGiorni_LostFocus(object sender, RoutedEventArgs e)
        {
            int numeroGiorni = 0;
            try
            {
                numeroGiorni = Convert.ToInt32(TextBox_numeroGiorni.Text);
                ChangeDataFine.Content = ((Convert.ToDateTime(ChangeDataInizio.Content)).AddDays(numeroGiorni)).ToString("dd/MM/yyyy");
            }
            catch { MessageBox.Show("La durata deve essere un numero intero non negativo"); TextBox_numeroGiorni.Text = ""; UpdateDurata(); }

            UpdateEvent(this);
        }
    }
}
