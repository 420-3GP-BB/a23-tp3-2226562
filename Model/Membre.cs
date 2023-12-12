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

        public Membre() 
        {
            
        }

        public Membre(XmlElement unMembre, Dictionary<string, Livre> _dictionnaire)
        {
            Nom = unMembre.GetAttribute("nom");
            Administrateur = bool.Parse(unMembre.GetAttribute("administrateur"));
            mesLivres = new ObservableCollection<Livre>();
            CommandeTraites = new ObservableCollection<Livre>();
            CommandeEnAttente = new ObservableCollection<Livre>();

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

        public XmlElement VersXml(XmlDocument doc)
        {
            XmlElement nouvelElement = doc.CreateElement("membre");
            nouvelElement.SetAttribute("nom", Nom);
            nouvelElement.SetAttribute("administrateur", Administrateur.ToString());
            foreach(Livre livre in mesLivres)
            {
                XmlElement unLivre = doc.CreateElement("livre");
                unLivre.SetAttribute("ISBN-13", livre.Isbn13);
                nouvelElement.AppendChild(unLivre);
            }
            foreach(Livre cmdAttente in CommandeEnAttente)
            {
                XmlElement cmdAtt = doc.CreateElement("commande");
                cmdAtt.SetAttribute("statut", "Attente");
                cmdAtt.SetAttribute("ISBN-13", cmdAttente.Isbn13);
                nouvelElement.AppendChild(cmdAtt);
            }

            foreach (Livre cmdTraite in CommandeTraites)
            {
                XmlElement cmdtermine = doc.CreateElement("commande");
                cmdtermine.SetAttribute("statut", "Traitee");
                cmdtermine.SetAttribute("ISBN-13", cmdTraite.Isbn13);
                nouvelElement.AppendChild (cmdtermine);
            }

            return nouvelElement;
        }

        override
        public string ToString()
        {
            return Nom;
        }



    }
}
