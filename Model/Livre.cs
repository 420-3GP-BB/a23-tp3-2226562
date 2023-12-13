using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    // Création de la classe Livre
    public class Livre
    {
        // Les attributs : 
        // Chaque livre contient le ISBN-13 qui est l'identifiant du livre
        public string Isbn13 { get; set; }
        // Un titre du livre
        public string Titre { get; set; }
        // L'auteur du livre
        public string Auteur { get; set; }
        // L'éditeur du livre
        public string Editeur { get; set; }
        // Son année de publication
        public int Annee { get; set; }

        // Le constructeur de la classe Livre prend en paramètre l'élément xml du livre
        public Livre(XmlElement unLivre) 
        {
            // ISBN13 est l'attribut de l'élément
            Isbn13 = unLivre.GetAttribute("ISBN-13");
            // le titre se trouve dans le InnerText du fils de l'élément (l'élément fils s'appelle titre)
            Titre = unLivre["titre"].InnerText;
            // le titre se trouve dans le InnerText du fils de l'élément (l'élément fils s'appelle auteur)
            Auteur = unLivre["auteur"].InnerText;
            // le titre se trouve dans le InnerText du fils de l'élément (l'élément fils s'appelle editeur)
            Editeur = unLivre["editeur"].InnerText;
            // le titre se trouve dans le InnerText du fils de l'élément (l'élément fils s'appelle annee)
            // Il faut noter que le innerText est de type string et l'année est de type int
            // On doit utiliser la méthode Int32.Parse() pour transformer le string en int
            Annee = Int32.Parse(unLivre["annee"].InnerText);
        }

        // Un constructeur par défaut pour la classe Livre
        public Livre() 
        {
            Isbn13 = "";
            Titre = "";
            Auteur = "";
            Editeur = "";
        }


        // La méthode VersXml qui retourne l'élément (d'un livre) qu'on va ajouter au document
        // cet élément contient les informations du livre
        public XmlElement VersXml(XmlDocument doc)
        {
            // créer l'élément 
            XmlElement NouveauLivre = doc.CreateElement("livre");
            // donner un attribut
            NouveauLivre.SetAttribute("ISBN-13", Isbn13);
            // créer l'élément fils titre
            XmlElement titre = doc.CreateElement("titre");
            titre.InnerText = Titre;
            NouveauLivre.AppendChild(titre);
            // créer l'élément fils auteur
            XmlElement auteur = doc.CreateElement("auteur");
            auteur.InnerText = Auteur;
            NouveauLivre.AppendChild(auteur);
            // créer l'élément fils editeur
            XmlElement editeur = doc.CreateElement("editeur");
            editeur.InnerText = Editeur;
            NouveauLivre.AppendChild(editeur);
            // créer l'élément fils annee
            XmlElement annee = doc.CreateElement("annee");
            annee.InnerText = Annee.ToString();
            NouveauLivre.AppendChild(annee);

            // retouner l'élément
            return NouveauLivre;
        }


        // La méthode ToString()
        override
        public string ToString()
        {
            return $"{Titre}, {Auteur}, ({Annee})";
        }
    }
}
