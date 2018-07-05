using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace project1_andromeda_0._0._2
{
    public class Class_progettoTreno
    {
        public UserControl_progettoTreno userControl;
        public Class_progettoCollisione collisioni;

        public Class_progettiTreni thisProgettiTreni;

        public bool Selected
        {
            get { return userControl.Selected; }
            set { userControl.Selected = value; }
        }

        private int private_zIndex = 0;
        public int zIndex
        {
            get { return private_zIndex; }
            set
            {
                private_zIndex = value;
                Panel.SetZIndex(userControl, value);
            }
        }
        public string NomeProgetto
        {
            get { return userControl.GroupBox_nomeProgetto.Header.ToString(); }
            set { userControl.GroupBox_nomeProgetto.Header = value; }
        }
        public DateTime DataInizio
        {
            get { return Convert.ToDateTime(userControl.ChangeDataInizio.Content); }
            set { userControl.ChangeDataInizio.Content = value.ToString("dd/MM/yyyy"); userControl.UpdateDurata(); }
        }
        public DateTime DataFine
        {
            get { return Convert.ToDateTime(userControl.ChangeDataFine.Content); }
            set { userControl.ChangeDataFine.Content = value.ToString("dd/MM/yyyy"); userControl.UpdateDurata(); }
        }
        public string Durata
        {
            get { return userControl.TextBox_numeroGiorni.Text; }
        }
        public string Colore
        {
            get { return userControl.currentColor; }
            set { userControl.currentColor = value; userControl.CalcolaColore(); }
        }
        public string Commenti
        {
            get { return new TextRange(userControl.TextCommenti.Document.ContentStart, userControl.TextCommenti.Document.ContentEnd).Text; }
            set { userControl.TextCommenti.Document.Blocks.Clear(); userControl.TextCommenti.Document.Blocks.Add(new Paragraph(new Run(value))); }
        }
        public bool Collide
        {
            get { return userControl.Conflitto; }
            set { userControl.Conflitto = value; }
        }
        public int DimensioneProgetto
        {
            get { return userControl.DimensioneProgetto; }
            set { userControl.DimensioneProgetto = value; userControl.TextBlock_dimensione.Text = value + " m"; }
        }

        public Class_progettoTreno()
        {
            userControl = new UserControl_progettoTreno();
            userControl.thisProgetto = this;

            collisioni = new Class_progettoCollisione(this);
        }
    }
}
