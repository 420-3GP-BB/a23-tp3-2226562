using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    // Création de la classe Membre 
    public class Membre
    {
        // Chaque membre a un nom
        public string Nom { get; set; }
        // le booléen qui mentionne si le membre a le droit des administrateurs, true si c'est vrai, false si c'est faux
        public bool Administrateur { get; set; }
        // La collection des livres pour chaque membre
        public ObservableCollection<Livre> mesLivres { get; set; }
        // la collection des commandes traitées pour chaque membre
        public ObservableCollection<Livre> CommandeTraites { get; set; }
        // la collection des commandes en attente pour chaque membre
        public ObservableCollection<Livre> CommandeEnAttente { get; set; }

        // le constructeur qui prend en paramètres l'élément xml du membre + le dictionnaire des livres
        public Membre(XmlElement unMembre, Dictionary<string, Livre> _dictionnaire)
        {
            // initialiser le nom avec l'attribut de l'élément
            Nom = unMembre.GetAttribute("nom");
            // initialiser le booléen admin avec l'attribut de l'élément
            Administrateur = bool.Parse(unMembre.GetAttribute("administrateur"));
            // initialiser les collections
            mesLivres = new ObservableCollection<Livre>();
            CommandeTraites = new ObservableCollection<Livre>();
            CommandeEnAttente = new ObservableCollection<Livre>();

            // mettre tous les tags avec le nom "livre" l'intérieur de l'élément membre dans une liste à 
            XmlNodeList livres = unMembre.GetElementsByTagName("livre");

            // pour chaque tag "livre" on ajoute à la collection le livre en cherchant son ISBN13 (l'attribut du tag) dans le dictionnaire 
            foreach (XmlElement livre in livres)
            {
                mesLivres.Add(_dictionnaire[livre.GetAttribute("ISBN-13")]);
            }

            // mettre tous les tags avec le nom commande dans une liste 
            XmlNodeList commandes = unMembre.GetElementsByTagName("commande");

            // pour chaque tag "commande"
            foreach(XmlElement commande in commandes)
            {
                // si le statut (dans l'attribut) est attente, on ajoute à la collection des commandes en attente le livre
                // en cherchant son ISBN13 dans le dictionnaire
                if (commande.GetAttribute("statut").ToLower().Equals("attente"))
                {
                    CommandeEnAttente.Add(_dictionnaire[commande.GetAttribute("ISBN-13")]);
                }
                else
                //si le statut (dans l'attribut) est traitee, on refait la meme chose mais dans la collection des commandes traitees
                {
                    CommandeTraites.Add(_dictionnaire[commande.GetAttribute("ISBN-13")]);
                }
            }

        }

        // La méthode VersXml qui retourne l'élément (d'un élément) qu'on va ajouter au document
        // cet élément contient les informations du livre
        public XmlElement VersXml(XmlDocument doc)
        {
            // créer l'élément
            XmlElement nouvelElement = doc.CreateElement("membre");
            nouvelElement.SetAttribute("nom", Nom);
            nouvelElement.SetAttribute("administrateur", Administrateur.ToString());
            // pour chaque livre qui se trouve dans la collection des livres d'un membre
            foreach(Livre livre in mesLivres)
            {
                // créer un élément du livre 
                XmlElement unLivre = doc.CreateElement("livre");
                unLivre.SetAttribute("ISBN-13", livre.Isbn13);
                // on l'ajoute à l'élément principal
                nouvelElement.AppendChild(unLivre);
            }

            // pour chaque commande en attente, on refait les memes instructions
            foreach(Livre cmdAttente in CommandeEnAttente)
            {
                XmlElement cmdAtt = doc.CreateElement("commande");
                cmdAtt.SetAttribute("statut", "Attente");
                cmdAtt.SetAttribute("ISBN-13", cmdAttente.Isbn13);
                nouvelElement.AppendChild(cmdAtt);
            }

            // aussi pour chaque commande traitees
            if(CommandeTraites.Count > 0) 
            {
                foreach (Livre cmdTraite in CommandeTraites)
                {
                    XmlElement cmdtermine = doc.CreateElement("commande");
                    cmdtermine.SetAttribute("statut", "Traitee");
                    cmdtermine.SetAttribute("ISBN-13", cmdTraite.Isbn13);
                    nouvelElement.AppendChild(cmdtermine);
                }
            }
            
            // on retourne l'élément
            return nouvelElement;
        }

        // la méthode ToString qui retourne seulement le nom de l'utilisateur
        override
        public string ToString()
        {
            return Nom;
        }



    }
}
