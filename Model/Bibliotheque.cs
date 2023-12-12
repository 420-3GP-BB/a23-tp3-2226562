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

        public void ChargerFichierXml(string _nomFichier)
        {
            XmlDocument document = new XmlDocument();
            document.Load(_nomFichier);
            XmlElement racine = document.DocumentElement;
            DernierUser = racine.GetAttribute("dernierUtilisateur");
            XmlElement baliseLivres = racine["livres"];
            XmlNodeList lesLivres = baliseLivres.GetElementsByTagName("livre");
            foreach (XmlElement elem in lesLivres)
            {
                Dictionnaire.Add(elem.GetAttribute("ISBN-13"), new Livre(elem));
            }

            XmlElement baliseMembres = racine["membres"];
            XmlNodeList lesMembres = baliseMembres.GetElementsByTagName("membre");
            foreach (XmlElement elem in lesMembres)
            {
                LesMembres.Add(new Membre(elem, Dictionnaire));
            }
        }

        public void SauvegarderXml(string _nomfichier)
        {
            XmlDocument document = new XmlDocument();
            XmlElement racine = document.CreateElement("bibliothque");
            racine.SetAttribute("dernierUtilisateur", DernierUser);
            
            document.AppendChild(racine);

            XmlElement elementMembres = document.CreateElement("membres");
            racine.AppendChild(elementMembres);

            foreach (Membre unMembre in LesMembres)
            {
                XmlElement element = unMembre.VersXml(document);
                elementMembres.AppendChild(element);
            }

            XmlElement elementLivres = document.CreateElement("livres");
            racine.AppendChild(elementLivres);

            // source : https://waytolearnx.com/2019/09/parcourir-un-dictionnaire-en-csharp.html 
            foreach (KeyValuePair<string, Livre> unLivre in Dictionnaire)
            {
                XmlElement livre = unLivre.Value.VersXml(document);
                elementLivres.AppendChild(livre);

            }
            document.Save(_nomfichier);
        }
    }
}
