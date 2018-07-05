using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace project1_andromeda_0._0._2
{
    public class Class_binari
    {
        const int TOPMARGIN = 25;

        public List<Class_binario> list_binari = new List<Class_binario>();
        public List<Class_progettoTreno> list_selectedProgetti = new List<Class_progettoTreno>();

        public delegate void NewSelectionDelegate(Class_progettoTreno sender);
        public event NewSelectionDelegate EventNewSelection;
        public event NewSelectionDelegate EventNewDeselection;

        public bool Debug = false;

        private Grid mainContainer;
        private int count_margin;
        private int space_margin = 300;

        private void RemoveEvent(UserControl_binario userControl)
        {
            int index = list_binari.FindIndex((x) => { if (x.userControl == userControl) { return true; } else { return false; } });
            list_binari.RemoveAt(index);

            if (Debug) MessageBox.Show("Index: " + index);

            for (int i = index; i < list_binari.Count; i++)
            {
                Thickness margin = new Thickness(0, TOPMARGIN, 0, 0);
                margin.Left = list_binari[i].userControl.Margin.Left - space_margin;

                list_binari[i].userControl.Margin = margin;
            }
            count_margin -= space_margin;
        }
        private void UpdateEvent(UserControl_binario userControl)
        {
            //
        }

        public Class_binari(Grid mainContainer, int count_margin = 0)
        {
            this.mainContainer = mainContainer;
            this.count_margin = count_margin;

            EventNewSelection += (x) => { };
            EventNewDeselection += (x) => { };
        }

        public Class_binari Add(Class_binario binario)
        {
            binario.userControl.Margin = new Thickness(count_margin + 10, TOPMARGIN, 0, 0);
            count_margin += space_margin;

            binario.userControl.RemoveEvent += RemoveEvent;
            binario.userControl.UpdateEvent += UpdateEvent;
            binario.thisBinari = this;

            list_binari.Add(binario);
            mainContainer.Children.Add(list_binari.Last().userControl);

            return this;
        }

        public Class_binari Remove(Class_binario binario)
        {
            list_binari.Remove(binario);
            mainContainer.Children.Remove(list_binari.Last().userControl);

            return this;
        }

        public Class_binari RemoveAt(int index)
        {
            list_binari.RemoveAt(index);
            mainContainer.Children.RemoveAt(index);

            return this;
        }

        public Class_binari RemoveAll()
        {
            for (int i = 0; i < list_binari.Count; i++)
            {
                list_binari.RemoveAt(i);
                mainContainer.Children.RemoveAt(i);
            }

            return this;
        }

        public void OrdinaTuttiProgetti()
        {
            list_binari.ForEach(x => x.OrdinaProgetti());
        }

        public void NewSelectionEvent(Class_progettoTreno progettoTreno)
        {
            if (Debug) MessageBox.Show("nuova selezione: " + progettoTreno.NomeProgetto);

            list_selectedProgetti.Add(progettoTreno);
            EventNewSelection(progettoTreno);
        }

        public void NewDeselectionEvent(Class_progettoTreno progettoTreno)
        {
            if (Debug) MessageBox.Show("nuova deselezione: " + progettoTreno.NomeProgetto);

            list_selectedProgetti.Remove(progettoTreno);
            EventNewDeselection(progettoTreno);
        }

        public void DeselezionaTutti()
        {
            for (int i = 0; i < list_selectedProgetti.Count; i++) { list_selectedProgetti[0].Selected = false; }
            if (list_selectedProgetti.Count > 0) DeselezionaTutti();
        }
    }
}
