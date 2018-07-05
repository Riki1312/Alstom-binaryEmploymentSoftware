using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace project1_andromeda_0._0._2
{
    public class Class_binario
    {
        public UserControl_binario userControl;
        public Class_binari thisBinari;

        public Class_progettiTreni progettiTreni
        {
            get { return userControl.class_ProgettiTreni; }
            set { userControl.class_ProgettiTreni = value; }
        }

        public string NomeBinario
        {
            get { return userControl.NomeBinario.Text; }
            set { userControl.NomeBinario.Text = value; }
        }

        public int NumeroProgetti
        {
            get { return userControl.ScrollGrid.Children.Count; }
        }

        public bool UtilizzaMisure
        {
            get { return userControl.UtilizzareMisure; }
        }

        public Class_binario()
        {
            userControl = new UserControl_binario();
            userControl.class_ProgettiTreni.thisBinario = this;
        }

        public void OrdinaProgetti()
        {
            progettiTreni.list_progettiTreni = progettiTreni.list_progettiTreni.OrderBy(x => x.DataInizio).ToList();
        }
    }
}
