using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_andromeda_0._0._2
{
    public class Class_progettoCollisione
    {
        public Class_progettoTreno soggetto;
        public List<Class_progettoTreno> progettiCollidenti = new List<Class_progettoTreno>();

        public Class_progettoCollisione(Class_progettoTreno soggetto)
        {
            this.soggetto = soggetto;
        }
    }
}
