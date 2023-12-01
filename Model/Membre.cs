using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    public class Membre
    {
        public string Nom { get; set; }
        public bool Administrateur { get; set; }
        public ObservableCollection<Livre> mesLivres { get; set; }
        public ObservableCollection<Livre> CommandeTraites { get; set; }
        public ObservableCollection<Livre> CommandeEnAttente { get; set; }

        public Membre(XmlElement unMembre, Dictionary<string, Livre> _dictionnaire)
        {
            Nom = unMembre.GetAttribute("nom");
            Administrateur = bool.Parse(unMembre.GetAttribute("administrateur"));

            XmlNodeList livres = unMembre.GetElementsByTagName("livre");

            foreach (XmlElement livre in livres)
            {
                mesLivres.Add(_dictionnaire[livre.GetAttribute("ISBN-13")]);
            }

            XmlNodeList commandes = unMembre.GetElementsByTagName("commande");

            foreach(XmlElement commande in commandes)
            {
                if (commande.GetAttribute("statut").ToLower().Equals("attente"))
                {
                    CommandeEnAttente.Add(_dictionnaire[commande.GetAttribute("ISBN-13")]);
                }
                else
                {
                    CommandeTraites.Add(_dictionnaire[commande.GetAttribute("ISBN-13")]);
                }
            }
        }



    }
}
