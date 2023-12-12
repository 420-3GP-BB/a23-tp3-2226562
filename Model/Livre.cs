using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    public class Livre
    {
        public string Isbn13 { get; set; }
        public string Titre { get; set; }
        public string Auteur { get; set; }
        public string Editeur { get; set; }
        public int Annee { get; set; }

        public Livre(XmlElement unLivre) 
        {
            Isbn13 = unLivre.GetAttribute("ISBN-13");
            Titre = unLivre["titre"].InnerText;
            Auteur = unLivre["auteur"].InnerText;
            Editeur = unLivre["editeur"].InnerText;
            Annee = Int32.Parse(unLivre["annee"].InnerText);
        }

        public Livre() 
        {
            Isbn13 = "";
            Titre = "";
            Auteur = "";
            Editeur = "";
            
        }



        public XmlElement VersXml(XmlDocument doc)
        {
            XmlElement NouveauLivre = doc.CreateElement("livre");
            NouveauLivre.SetAttribute("ISBN-13", Isbn13);
            XmlElement titre = doc.CreateElement("titre");
            titre.InnerText = Titre;
            NouveauLivre.AppendChild(titre);
            XmlElement auteur = doc.CreateElement("auteur");
            auteur.InnerText = Auteur;
            NouveauLivre.AppendChild(auteur);
            XmlElement editeur = doc.CreateElement("editeur");
            editeur.InnerText = Editeur;
            NouveauLivre.AppendChild(editeur);
            XmlElement annee = doc.CreateElement("annee");
            annee.InnerText = Annee.ToString();
            NouveauLivre.AppendChild(annee);

            return NouveauLivre;
        }


        override
        public string ToString()
        {
            return $"{Titre}, {Auteur}, ({Annee})";
        }
    }
}
