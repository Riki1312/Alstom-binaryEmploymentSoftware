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
    /// Logica di interazione per Window_conflitto.xaml
    /// </summary>
    public partial class Window_conflitto : Window
    {
        public string ButtonClicked = "null";

        private bool VisProposata = true;

        public Window_conflitto()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_annulla_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked = "annulla";
            Close();
        }

        private void Button_conferma_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked = "conferma";
            Close();
        }

        private void Button_soluzione_Click(object sender, RoutedEventArgs e)
        {
            if (VisProposata)
            {
                SalvaStatoCorrente();

                TimeSpan tConflitto = ((UserControl_progettoTreno)Grid_progettoSelezionato.Children[0]).thisProgetto.DataFine - ((UserControl_progettoTreno)Grid_progettiCollidenti.Children[0]).thisProgetto.DataInizio;

                List<Class_progettoTreno> listProgettiCollidenti = new List<Class_progettoTreno>();
                foreach (UIElement x in Grid_progettiCollidenti.Children)
                    listProgettiCollidenti.Add(((UserControl_progettoTreno)x).thisProgetto);

                RimandaProgetti(0, listProgettiCollidenti, tConflitto.Add(TimeSpan.FromDays(1)));

                Button_soluzione.Content = "Ripristina stato precedente";
                VisProposata = false;
            }
            else
            {
                //Ritorna allo stato precedente
                ((UserControl_progettoTreno)Grid_progettoSelezionato.Children[0]).thisProgetto.DataInizio = COPY_pSelezionato.DataInizio;
                ((UserControl_progettoTreno)Grid_progettoSelezionato.Children[0]).thisProgetto.DataFine = COPY_pSelezionato.DataFine;

                int i = 0;
                foreach (UIElement x in Grid_progettiCollidenti.Children)
                {
                    ((UserControl_progettoTreno)x).thisProgetto.DataInizio = COPY_pCollidenti[i].DataInizio;
                    ((UserControl_progettoTreno)x).thisProgetto.DataFine = COPY_pCollidenti[i].DataFine;
                    i++;
                }

                Button_soluzione.Content = "Visualizza proposta di soluzione";
                VisProposata = true;
            }
        }

        Class_progettoTreno COPY_pSelezionato = new Class_progettoTreno();
        List<Class_progettoTreno> COPY_pCollidenti = new List<Class_progettoTreno>();
        private void SalvaStatoCorrente()
        {
            COPY_pSelezionato.DataInizio = ((UserControl_progettoTreno)Grid_progettoSelezionato.Children[0]).thisProgetto.DataInizio;
            COPY_pSelezionato.DataFine = ((UserControl_progettoTreno)Grid_progettoSelezionato.Children[0]).thisProgetto.DataFine;

            foreach (UIElement x in Grid_progettiCollidenti.Children)
            {
                Class_progettoTreno temporanCopy = new Class_progettoTreno();
                temporanCopy.DataInizio = ((UserControl_progettoTreno)x).thisProgetto.DataInizio;
                temporanCopy.DataFine = ((UserControl_progettoTreno)x).thisProgetto.DataFine;
                COPY_pCollidenti.Add(temporanCopy);
            }

        }

        public void RimandaProgetti(int indexProgetto_toStart, List<Class_progettoTreno> listProgetti_toEach, TimeSpan tempo_toAdd)
        {
            //Ordinare la lista prima (per data di inizio).
            listProgetti_toEach = listProgetti_toEach.OrderBy(x => x.DataInizio).ToList();

            for (int i = indexProgetto_toStart; i < listProgetti_toEach.Count; i++)
            {
                listProgetti_toEach[i].DataFine += tempo_toAdd;
                listProgetti_toEach[i].DataInizio += tempo_toAdd;
            }
        }
    }
}
