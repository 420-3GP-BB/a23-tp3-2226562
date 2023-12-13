using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    // Création de la classe Bibliothèque
    public class Bibliotheque
    {
        // L'attribut DernierUser de type string qui signifie le nom du dernier utilisateur qui a accédé à son compte
        public string DernierUser { get; set; }
        // Le dictionnaire qui contient tous les livres dans la bibliotheque, le ISBN-13 comme clé, et le livre avec ce ISBN-13 en valeur
        public Dictionary<string, Livre> Dictionnaire { get; set; }
        // Une observable collection qui contient tous les membres
        public ObservableCollection<Membre> LesMembres { get; set; }
        // Une observable collection qui contient tous les commandes en attente
        public ObservableCollection<Commande> TousCommandesEnAttente { get; set; }
        // une observable collection qui contient tous les commandes traités
        public ObservableCollection<Commande> TousCommandesTraites { get; set; }

        // Le constructeur de la classe bibliothèque
        public Bibliotheque() 
        {
            // Initialiser les attributs
            DernierUser = string.Empty;
            Dictionnaire = new Dictionary<string, Livre>();
            LesMembres = new ObservableCollection<Membre>();
            TousCommandesEnAttente = new ObservableCollection<Commande>();
            TousCommandesTraites = new ObservableCollection<Commande>();


        }

        // Une méthode qui charge le fichier xml et remplis le dictionnaire et les collections.
        public void ChargerFichierXml(string _nomFichier)
        {
            XmlDocument document = new XmlDocument();
            // charger le fichier
            document.Load(_nomFichier);
            // créer la racine
            XmlElement racine = document.DocumentElement;
            // assigner le valeur de l'attribut de la racine "dernierUtilisateur" avec l'attribut dernierUser
            DernierUser = racine.GetAttribute("dernierUtilisateur");
            // Créer l'élément livres qui est fils de la racine
            XmlElement baliseLivres = racine["livres"];
            // créer la liste qui contient tous les élément avec le nom "livre", elle contient tous les livres qui se situent dans le fichier
            XmlNodeList lesLivres = baliseLivres.GetElementsByTagName("livre");
            // pour chaque élément qui se trouve dans cette liste
            foreach (XmlElement elem in lesLivres)
            {
                // Ajouter au dictionnaire des livres cet élément.
                Dictionnaire.Add(elem.GetAttribute("ISBN-13"), new Livre(elem));
            }
            // Créer l'élément des membres qui est fils de racine
            XmlElement baliseMembres = racine["membres"];
            // créer la liste qui contient tous les éléments avec le nom "membre", elle contient tous les membres qui se situent dans le fichier
            XmlNodeList lesMembres = baliseMembres.GetElementsByTagName("membre");
            // pour chaque élément qui se trouve dans cette liste
            foreach (XmlElement elem in lesMembres)
            {
                // Ajouter à la collection des membres cet élément en lui passant en paramètres l'élément xml et le dictionnaire
                LesMembres.Add(new Membre(elem, Dictionnaire));
            }

            // appeler la méthode qui remplis toutes les commandes en attente et toutes les commandes traitées
            remplirTousCmdEnAttente();
            remplirTousCmdTraites();
            

           
        }

        // la méthode qui remplit toutes les commandes en attente, elle parcours dans chaque membre tous ses livres commandés en attente et les ajouter à la collection
        // des commandes en attente
        public void remplirTousCmdEnAttente()
        {
            TousCommandesEnAttente.Clear();
            foreach(Membre mem in LesMembres)
            {
                foreach(Livre livre in mem.CommandeEnAttente)
                {
                    TousCommandesEnAttente.Add(new Commande(mem, livre, "Attente"));
                }
                
            }
        }

        // la méthode qui remplis toutes les commandes traitées, elle parcours dans chaque membre tous ses livres commandés en attente et les ajouter à la collection
        // des commandes traitées
        public void remplirTousCmdTraites()
        {
            TousCommandesTraites.Clear();
            foreach (Membre mem in LesMembres)
            {
                foreach (Livre livre in mem.CommandeTraites)
                {
                    TousCommandesTraites.Add(new Commande(mem, livre, "Traitee"));
                }

            }
        }

        // La méthode qui sauvegarde le fichier xml et le met à jour après chaque changement.
        public void SauvegarderXml(string _nomfichier)
        {
            // créer un nouveau document
            XmlDocument document = new XmlDocument();
            // créer la racine
            XmlElement racine = document.CreateElement("bibliothque");
            // ajouter l'attribut dernierUtilisateur à la racine
            racine.SetAttribute("dernierUtilisateur", DernierUser);
            
            // ajouter la racine au doc
            document.AppendChild(racine);

            // créer l'élément membres qui contient tous les membres
            XmlElement elementMembres = document.CreateElement("membres");
            racine.AppendChild(elementMembres);

            // pour chaque membre dans la collection des membres
            foreach (Membre unMembre in LesMembres)
            {
                // on crée un élément et on l'ajoute à "membres"
                XmlElement element = unMembre.VersXml(document);
                elementMembres.AppendChild(element);
            }

            // créer l'élément livres qui contient tous les livres
            XmlElement elementLivres = document.CreateElement("livres");
            racine.AppendChild(elementLivres);

            // pour chaque livre à l'intérieur du dictionnaire
            // source : https://waytolearnx.com/2019/09/parcourir-un-dictionnaire-en-csharp.html 
            foreach (KeyValuePair<string, Livre> unLivre in Dictionnaire)
            {
                // on crée un élément et on l'ajoute à "livres"
                XmlElement livre = unLivre.Value.VersXml(document);
                elementLivres.AppendChild(livre);

            }

            // On sauvegarde le document en lui donnant le nom du fichier initial
            // ça va écraser l'ancienne version et enregistrer les nouvelles modifications
            document.Save(_nomfichier);
        }
    }
}
