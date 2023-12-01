using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    public class Bibliotheque
    {
        public string DernierUser { get; set; }
        public Dictionary<string, Livre> Dictionnaire { get; set; }
        public List<Membre> LesMembres { get; set; }

        public Bibliotheque() 
        {
            DernierUser = string.Empty;
            Dictionnaire = new Dictionary<string, Livre>();
            LesMembres = new List<Membre>();
        }
    }
}
