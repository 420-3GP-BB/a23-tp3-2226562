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


        public override string ToString()
        {
            return $"{Titre}, {Auteur}, ({Annee})";
        }
    }
}
