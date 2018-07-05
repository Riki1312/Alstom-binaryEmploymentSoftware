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

using CustomLibrary;

namespace project1_andromeda_0._0._2
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Class_binari binari;
        private Class_comando comando;

        public MainWindow()
        {
            Window windowLoading = new Window_loading();

            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            binari = new Class_binari(Grid_scrollOrizzontale);
            binari.EventNewSelection += NewSelectionEvent;
            binari.EventNewDeselection += NewDeselectionEvent;
            binari.Add(new Class_binario());

            comando = new Class_comando(this);

            windowLoading.ShowDialog();
        }

        private void AddBinario_Click(object sender, RoutedEventArgs e)
        {
            binari.Add(new Class_binario());
            binari.list_binari.Last().NomeBinario = "Binario " + binari.list_binari.Count;
        }

        private void Button_debug_Click(object sender, RoutedEventArgs e)
        {
            Window windowDebug = new Window_debug();
            windowDebug.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Risolvere: rimane aperto qualcosa.
            comando.AnnullaAzione(this, null);
        }

        private void Button_visBinari_Click(object sender, RoutedEventArgs e)
        {
            Window windowVisBinari = new Window_visBinari(binari);
            windowVisBinari.Show();
        }

        public bool ExportAllData = true;
        private void Button_exportCSV_Click(object sender, RoutedEventArgs e)
        {
            ExportAllData = (bool)CheckBox_ExportAll.IsChecked;
            string FileContent = "";

            // { Aprire una finestra di caricamento }.

            try
            {
                //Trovare intervallo di date coperto [
                List<DateTime> dateInizioCandidati = new List<DateTime>();
                binari.list_binari.ForEach((x) =>
                {
                    if (x.progettiTreni.list_progettiTreni.Count > 0)
                        dateInizioCandidati.Add(x.progettiTreni.list_progettiTreni[0].DataInizio);
                });
                dateInizioCandidati.Sort((a, b) => a.CompareTo(b));
                DateTime dataInizio = dateInizioCandidati[0];

                List<DateTime> dateFineCandidati = new List<DateTime>();
                binari.list_binari.ForEach((x) =>
                {
                    if (x.progettiTreni.list_progettiTreni.Count > 0)
                        dateFineCandidati.Add(x.progettiTreni.list_progettiTreni.Last().DataFine);
                });
                dateFineCandidati.Sort((a, b) => a.CompareTo(b));
                DateTime dataFine = dateFineCandidati.Last();
                TimeSpan intervalloDate = dataFine - dataInizio;
                // ]

                string sBinari = "";
                binari.list_binari.ForEach(x =>
                {
                    if (x.UtilizzaMisure)
                        sBinari += x.NomeBinario.Replace(";", " ") + " {" + x.userControl.DimensioneBinario + " m}";
                    else
                        sBinari += x.NomeBinario.Replace(";", " ");

                    if (ExportAllData)
                        sBinari += " (Commento: " + x.userControl.Commenti.Replace("\n", " ").Replace("\r", "").Replace(";", " ") + ")";

                    sBinari += ";";
                });

                FileContent += "Calendario;";
                FileContent += sBinari + " \n";

                for (int i = 0; i < intervalloDate.Days + 1; i++)
                {
                    //Ottenere il progetto presente in questa data per ogni binario.

                    string sData = (dataInizio.AddDays(i)).ToString("dd/MM/yyyy");
                    string sProgetti = "";

                    List<Class_progettoTreno> progettiInData = new List<Class_progettoTreno>();
                    for (int j = 0; j < binari.list_binari.Count; j++)
                    {
                        List<Class_progettoTreno> progettiInDataBinario = binari.list_binari[j].progettiTreni.list_progettiTreni.FindAll(x =>
                        {
                            if (x.DataInizio <= Convert.ToDateTime(sData) && x.DataFine >= Convert.ToDateTime(sData))
                                return true;
                            else
                                return false;
                        });

                        progettiInData.AddRange(progettiInDataBinario);
                    }

                    if (progettiInData.Count > 0)
                    {
                        for (int j = 0; j < binari.list_binari.Count; j++)
                        {
                            string s = "";

                            progettiInData.ForEach(x =>
                            {
                                bool progettiMultipli = false;
                                progettiInData.ForEach((z) => { if (z != x && z.thisProgettiTreni.thisBinario.NomeBinario == x.thisProgettiTreni.thisBinario.NomeBinario) progettiMultipli = true; });

                                if (x.thisProgettiTreni.thisBinario.NomeBinario == binari.list_binari[j].NomeBinario)
                                {
                                    if (binari.list_binari[j].UtilizzaMisure)
                                        s += x.NomeProgetto.Replace(";", " ") + " {" + x.DimensioneProgetto + " m}";
                                    else
                                        s += x.NomeProgetto.Replace(";", " ");

                                    if (ExportAllData && x.Commenti.Replace("\n", "").Replace("\r", "").Replace(";", "").Replace(" ", "") != "")
                                        s += " (Commento: " + x.Commenti.Replace("\n", " ").Replace("\r", "").Replace(";", " ") + ")" + " [" + x.Colore + "]";

                                    if (progettiMultipli)
                                        s += " / ";
                                    else
                                        s += "  ";
                                }
                            });

                            if (s == "") s = " ";

                            sProgetti += s + ";";
                        }
                    }

                    FileContent += sData + ";";
                    FileContent += sProgetti + " \n";
                }

                FileContent += " \n ";

                FileMenager.SaveFile("Export", FileContent, new string[] { "CSV ", "Text" }, new string[] { "csv ", "txt" });
            }
            catch { MessageBox.Show("Errore durante l'esportazione, impossibile portare a termine l'operazione"); }
        }

        private void Button_info_Click(object sender, RoutedEventArgs e)
        {
            Window_info window_Info = new Window_info();
            window_Info.Show();
        }

        private void Button_impostazioni_Click(object sender, RoutedEventArgs e)
        {
            Window_impostazioni window_Impostazioni = new Window_impostazioni();
            window_Impostazioni.ShowDialog();
        }

        private void Button_exportExcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funzione esportazione in formato excel completo non supportata su questo dispositivo, utilizzare l'esportazione in CSV.");
        }

        private void Button_importExcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funzione importazione da formato excel non supportata su questo dispositivo, utilizzare l'importazione da CSV.");
        }

        private void Button_visStandard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Visualizzazione standard attualmente impostata");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> importFileLines = FileMenager.ReadFileLines(FileMenager.SelectFile(new string[] { "CSV ", "Text" }, new string[] { "csv ", "txt" }));
                
                List<string> binariStringa = importFileLines[0].Replace("Calendario;", "").Split(';').ToList();
                
                List<string> nomiBinari = new List<string>();
                List<string> dimensioneBinari = new List<string>();
                List<string> commentiBinari = new List<string>();
                
                binariStringa.ForEach(x =>
                {
                    nomiBinari.Add(x.Split('(')[0].Split('{')[0]);

                    if (x.Contains("(") && x.Contains("}"))
                    {
                        commentiBinari.Add(x.Split('(')[1].Split(')')[0]);
                        dimensioneBinari.Add(x.Split('{')[1].Split('}')[0]);
                    }
                    else if (x.Contains("("))
                        commentiBinari.Add(x.Split('(')[1].Split(')')[0]);
                    else if (x.Contains("{"))
                        dimensioneBinari.Add(x.Split('{')[1].Split('}')[0]);

                    if (!x.Contains("{")) dimensioneBinari.Add(null);
                });
                
                binari.RemoveAll();
                binari = new Class_binari(Grid_scrollOrizzontale);

                for (int i = 0; i < binariStringa.Count; i++)
                {
                    if (binariStringa[i] != "" && binariStringa[i] != " ")
                    {
                        Class_binario binario = new Class_binario();
                        binario.NomeBinario = nomiBinari[i].Split('{')[0];

                        if (dimensioneBinari.Count > i && dimensioneBinari[i] != null)
                        {
                            binario.userControl.UtilizzareMisure = true;
                            binario.userControl.DimensioneBinario = Convert.ToInt32(dimensioneBinari[i].Replace("m", ""));
                        }
                        if (commentiBinari.Count > i)
                            binario.userControl.Commenti = commentiBinari[i];

                        binari.Add(binario);
                        
                        //Ciclo lines per progetti
                        bool enter = false;
                        List<string> listNomePMultipli = new List<string>();
                        List<DateTime> listDataPMultipli = new List<DateTime>();
                        
                        for (int j = 1; j < importFileLines.Count; j++)
                        {
                            if (importFileLines[j].Replace(";", "").Replace("\n", "") != "" && importFileLines[j].Replace(";", "").Replace("\n", "") != " " && i + 1 < importFileLines[j].Split(';').Length)
                            {
                                string progettoStringa = importFileLines[j].Split(';')[i + 1];
                                
                                if (progettoStringa.Replace(";", "").Replace("\n", "") != "" && progettoStringa.Replace(";", "").Replace("\n", "") != " ")
                                {
                                    if (!enter)
                                    {
                                        Class_progettoTreno progettoTreno = new Class_progettoTreno();

                                        progettoTreno.NomeProgetto = progettoStringa.Split('(')[0].Split('{')[0];
                                        
                                        if (progettoStringa.Contains("{") && dimensioneBinari.Count > i && dimensioneBinari[i] != null)
                                        {
                                            progettoTreno.DimensioneProgetto = Convert.ToInt32(progettoStringa.Split('{')[1].Split('}')[0].Replace("m", ""));

                                            if (progettoTreno.DimensioneProgetto > 0)
                                                progettoTreno.userControl.SetAvviso = null;
                                        }

                                        if (progettoStringa.Contains("("))
                                            progettoTreno.Commenti = progettoStringa.Split('(')[1].Split(')')[0];

                                        if (progettoStringa.Contains("["))
                                            progettoTreno.Colore = progettoStringa.Split('[')[1].Split(']')[0];

                                        progettoTreno.DataInizio = Convert.ToDateTime(importFileLines[j].Split(';')[0]);
                                        binari.list_binari[i].progettiTreni.Add(progettoTreno);

                                        if (progettoStringa.Contains("/"))
                                        {
                                            //Gestione progetti multipli

                                            listNomePMultipli.Add(progettoStringa.Split('/')[1].Split('(')[0].Split('{')[0].Split('[')[0]);
                                            listDataPMultipli.Add(progettoTreno.DataInizio);
                                        }
                                    }
                                    else
                                    {
                                        enter = false;

                                        DateTime dataFine = new DateTime();
                                        int index = listNomePMultipli.FindIndex((x) => x == binari.list_binari[i].progettiTreni.list_progettiTreni.Last().NomeProgetto);
                                        if (index > 0)
                                            dataFine = listDataPMultipli[index];
                                        else
                                            dataFine = Convert.ToDateTime(importFileLines[j].Split(';')[0]);

                                        if (dataFine >= binari.list_binari[i].progettiTreni.list_progettiTreni.Last().DataInizio)
                                            binari.list_binari[i].progettiTreni.list_progettiTreni.Last().DataFine = dataFine;
                                        else
                                            binari.list_binari[i].progettiTreni.list_progettiTreni.Last().DataFine = binari.list_binari[i].progettiTreni.list_progettiTreni.Last().DataInizio;
                                    }

                                    if (j + 1 < importFileLines.Count && importFileLines[j + 1] != "" && importFileLines[j + 1] != " ")
                                    {
                                        try
                                        {
                                            if (importFileLines[j + 1].Split(';')[i + 1].Split('(')[0] == progettoStringa.Split('(')[0])
                                                enter = true;
                                        }
                                        catch { enter = false; }
                                    }
                                }
                            }
                        }
                    }
                }

                binari.list_binari.ForEach(x => x.progettiTreni.list_progettiTreni.ForEach(z => { if (Convert.ToInt32(z.Durata) < 0) z.DataFine = z.DataInizio; }));
            }
            catch(Exception ex) { MessageBox.Show("Errore durante l'importazione:\n" + ex.Message); }
        }

        //Gestione comandi azioni [

        private string ComandoAttuale = "";
        private int MinimoSelezionari = 0;

        private void NewSelectionEvent(Class_progettoTreno sender)
        {
            if (binari.list_selectedProgetti.Count >= MinimoSelezionari)
                comando.SetAcceptInvio = true;
            else
                comando.SetAcceptInvio = false;
        }

        private void NewDeselectionEvent(Class_progettoTreno sender)
        {
            if (binari.list_selectedProgetti.Count >= MinimoSelezionari)
                comando.SetAcceptInvio = true;
            else
                comando.SetAcceptInvio = false;
        }

        private void ConfermaComando()
        {
            switch(ComandoAttuale)
            {
                case "elimina": EliminaSelezionati(); break;
            }
        }

        private void EliminaSelezionati()
        {
            binari.list_selectedProgetti.ForEach(x =>
            {
                x.userControl.ChiediConfermaElimina = false;
                x.userControl.EliminaProgetto_Click(null, null);
                x.userControl.ChiediConfermaElimina = true;
            });
        }

        private void Button_COM_spostaDopo_Click(object sender, RoutedEventArgs e)
        {
            comando.Text = "Selezionare i progetti";
            comando.AzionaComando();
        }
        private void Button_COM_elimina_Click(object sender, RoutedEventArgs e)
        {
            ComandoAttuale = "elimina";
            MinimoSelezionari = 1;

            binari.DeselezionaTutti();
            comando.Text = "Selezionare i progetti da eliminare";
            comando.EventAccepted = ConfermaComando;
            comando.AzionaComando();
        }

        //Gestione comandi azione ]
    }
}
