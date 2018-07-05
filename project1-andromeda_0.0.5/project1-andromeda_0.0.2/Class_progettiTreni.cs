using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace project1_andromeda_0._0._2
{
    public class Class_progettiTreni
    {
        public List<Class_progettoTreno> list_progettiTreni = new List<Class_progettoTreno>();
        public bool Debug = false;
        public Class_binario thisBinario;

        private Grid mainContainer;
        private int count_zIndex;
        private int count_margin;
        private int space_margin = 200;

        private void RemoveEvent(UserControl_progettoTreno userControl)
        {
            int index = list_progettiTreni.FindIndex((x) => { if (x.userControl == userControl) { return true; } else { return false; } });
            list_progettiTreni.RemoveAt(index);

            if (Debug) MessageBox.Show("Index: " + index);

            for (int i = index; i < list_progettiTreni.Count; i++)
            {
                Thickness margin = new Thickness(0, 0, 0, 0);
                margin.Top = list_progettiTreni[i].userControl.Margin.Top - space_margin;

                list_progettiTreni[i].userControl.Margin = margin;
            }
            count_margin -= space_margin;
        }

        public Class_progettiTreni(Grid mainContainer, int count_zIndex = 999, int count_margin = 0)
        {
            this.mainContainer = mainContainer;
            this.count_zIndex = count_zIndex;
            this.count_margin = count_margin;
        }

        public Class_progettiTreni Add(Class_progettoTreno progettoTreno)
        {
            progettoTreno.zIndex = count_zIndex--;
            progettoTreno.userControl.Margin = new Thickness(0, count_margin, 0, 0);
            count_margin += space_margin;
            progettoTreno.userControl.RemoveEvent += RemoveEvent;
            progettoTreno.userControl.UpdateEvent += UpdateEvent;

            progettoTreno.thisProgettiTreni = this;

            list_progettiTreni.Add(progettoTreno);
            mainContainer.Children.Add(list_progettiTreni.Last().userControl);

            FindConflitti();

            return this;
        }

        public Class_progettiTreni Remove(Class_progettoTreno progettoTreno)
        {
            mainContainer.Children.Remove(list_progettiTreni.Last().userControl);
            list_progettiTreni.Remove(progettoTreno);

            return this;
        }

        public Class_progettiTreni RemoveAt(int index)
        {
            mainContainer.Children.RemoveAt(index);
            list_progettiTreni.RemoveAt(index);

            return this;
        }

        public Class_progettiTreni FindConflitti()
        {
            if (!thisBinario.UtilizzaMisure)
            {
                BaseCalcConflitti();
            }
            else
            {
                //Calcolo confliti con misure

                for (int i = 0; i < list_progettiTreni.Count; i++)
                {
                    list_progettiTreni[i].Collide = false;

                    list_progettiTreni[i].collisioni.progettiCollidenti = new List<Class_progettoTreno>();
                    list_progettiTreni[i].collisioni.progettiCollidenti = list_progettiTreni.FindAll((x) =>
                    {
                        return (x.DataInizio <= list_progettiTreni[i].DataFine && x.DataFine >= list_progettiTreni[i].DataInizio && x != list_progettiTreni[i]);  //Forse >=
                    });

                    if (list_progettiTreni[i].collisioni.progettiCollidenti.Count > 0)
                    {
                        int sommaDimensioniCollidenti = list_progettiTreni[i].DimensioneProgetto;
                        list_progettiTreni[i].collisioni.progettiCollidenti.ForEach((x) =>
                        {
                            sommaDimensioniCollidenti += x.DimensioneProgetto;
                        });

                        if (sommaDimensioniCollidenti > thisBinario.userControl.DimensioneBinario)
                            list_progettiTreni[i].Collide = true;
                    }
                }
            }

            return this;
        }

        private void BaseCalcConflitti()
        {
            for (int i = 0; i < list_progettiTreni.Count; i++)
            {
                list_progettiTreni[i].Collide = false;

                list_progettiTreni[i].collisioni.progettiCollidenti = new List<Class_progettoTreno>();
                list_progettiTreni[i].collisioni.progettiCollidenti = list_progettiTreni.FindAll((x) =>
                {
                    return (x.DataInizio <= list_progettiTreni[i].DataFine && x.DataFine >= list_progettiTreni[i].DataInizio && x != list_progettiTreni[i]);  //Forse >=
                });

                if (list_progettiTreni[i].collisioni.progettiCollidenti.Count > 0)
                    list_progettiTreni[i].Collide = true;
            }
        }

        private void UpdateEvent(UserControl_progettoTreno userControl)
        {
            FindConflitti();
        }
    }
}
