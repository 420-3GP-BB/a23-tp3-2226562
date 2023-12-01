using Model;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Linq;
//using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bibliotheque _biblio;
        private string _pathFichier;
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;

        public MainWindow()
        {
            InitializeComponent();

            //_gestionnaire = new GestionnaireLivres();
            _pathFichier = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + DIR_SEPARATOR +
                          "Fichiers-3GP" + DIR_SEPARATOR + "bibliotheque.xml";

            ChargerFichierXml();

        }

        private void ChargerFichierXml()
        {
            XmlDocument document = new XmlDocument();
            document.Load(_pathFichier);
            XmlElement racine = document.DocumentElement;
            _biblio.DernierUser = racine.GetAttribute("dernierUtilisateur");
            XmlElement baliseLivres = racine["livres"];
            XmlNodeList lesLivres = baliseLivres.GetElementsByTagName("livre");
            foreach (XmlElement elem in lesLivres)
            {
                _biblio.Dictionnaire.Add(elem.GetAttribute("ISBN-13"), new Livre(elem));
            }

            XmlElement baliseMembres = racine["membres"];
            XmlNodeList lesMembres = baliseMembres.GetElementsByTagName("membre");
            foreach(XmlElement elem in lesMembres)
            {
                _biblio.LesMembres.Add(new Membre(elem, _biblio.Dictionnaire));
            }
        }

    }
    /*private void Quitter_Click(object sender, RoutedEventArgs e)
    {

    }*/
}

        
    
