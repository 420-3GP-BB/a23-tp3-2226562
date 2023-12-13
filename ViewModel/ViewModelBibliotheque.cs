using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Metadata;
using System.Xml;

namespace ViewModel
{
    public class ViewModelBibliotheque : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        private string _nomFichier;
        private Bibliotheque _biblio;
        private Membre? _userActif;
        private Livre _livreSelectionne;
        private Membre _userSelectionne;
        private Commande _commandeSelectionnee;

        public Livre UnLivre { get; set; }

        public Membre? UserActif
        {
            set
            {
                _userActif = value;
                OnPropertyChange(nameof(_userActif));
            }
            get
            {
                return _userActif;
            }
        }

        

        public Livre LivreSelectionne
        {
            get { return _livreSelectionne; }
            set
            {
                _livreSelectionne = value;
                OnPropertyChange(nameof(LivreSelectionne));
            }
        }

        public Commande CommandeSelectionnee
        {
            get { return _commandeSelectionnee; }
            set
            {
                _commandeSelectionnee = value;
                OnPropertyChange(nameof(_commandeSelectionnee));
            }
        }



        public Membre UserSelectionne
        {
            get => _userSelectionne;
            set
            {
                _userSelectionne = value;
                OnPropertyChange(nameof(UserSelectionne));
            }
        }


        public string DernierUser 
        { 
            get => _biblio.DernierUser;
            set
            {
                _biblio.DernierUser = value;
                OnPropertyChange(nameof(DernierUser));
            }
        }
        public Dictionary<string, Livre> Dictionnaire { get => _biblio.Dictionnaire; }
        public ObservableCollection<Membre> LesMembres { get => _biblio.LesMembres; }
        public ObservableCollection<Commande> LesCommandesEnAttente { get => _biblio.TousCommandesEnAttente; }
        public ObservableCollection<Commande> LesCommandesTraites { get => _biblio.TousCommandesTraites; }


        public ViewModelBibliotheque()
        {
            _nomFichier = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "bibliotheque.xml";
            _biblio = new Bibliotheque();
            _biblio.ChargerFichierXml(_nomFichier);

            mettreAJourUserActif();
            UnLivre = new Livre();

        }

        private void mettreAJourUserActif()
        {
            foreach (Membre membre in LesMembres)
            {
                if (membre.Nom.Equals(DernierUser))
                {
                    _userActif = membre;
                    OnPropertyChange(nameof(_userActif));
                }
            }

        }

        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangerUserActif(string nouveauUser)
        {
            DernierUser = nouveauUser;
            mettreAJourUserActif();
            _biblio.SauvegarderXml(_nomFichier);
            OnPropertyChange(nameof(DernierUser));
        }

        public bool isbnValide()
        {
            bool valide = false;
            if(UnLivre.Isbn13.Length == 13)
            {
                valide = true;
            }
            return valide;
        }

        public bool chaineValide()
        {
            bool valide = false;
            if (!UnLivre.Titre.Equals("") && !UnLivre.Auteur.Equals("") && !UnLivre.Editeur.Equals(""))
            {
                valide = true;
            }
            return valide;
        }

        public bool anneeValide()
        {
            bool valide = false;
            if(UnLivre.Annee >= -3000)
            {
                valide = true;
            }

            return valide;
        }

        public void ajouterAuxCommandes()
        {
            UserActif.CommandeEnAttente.Add(Dictionnaire[UnLivre.Isbn13]);
            _biblio.remplirTousCmdEnAttente();
            _biblio.SauvegarderXml(_nomFichier);
        }

        public bool existeDansLeDic()
        {
            bool existe = false;
            foreach (KeyValuePair<string, Livre> livredic in Dictionnaire)
            {
                if (livredic.Key.Equals(UnLivre.Isbn13)){
                    existe = true;
                }   
            }
            return existe;
        }

        public bool existeDansSonCompte()
        {
            bool existe = false;
            foreach(Livre livre in UserActif.mesLivres)
            {
                if (livre.Isbn13.Equals(UnLivre.Isbn13))
                {
                    existe = true;
                }
            }

            foreach(Livre livre in UserActif.CommandeEnAttente)
            {
                if (livre.Isbn13.Equals(UnLivre.Isbn13))
                {
                    existe = true;
                }
            }

            foreach (Livre livre in UserActif.CommandeTraites)
            {
                if (livre.Isbn13.Equals(UnLivre.Isbn13))
                {
                    existe = true;
                }
            }
            return existe;
        }

        public void annulerCmd(object selectedItemProperty)
        {
            UserActif.CommandeEnAttente.Remove(LivreSelectionne);
            _biblio.remplirTousCmdEnAttente();
            _biblio.SauvegarderXml(_nomFichier);
        }

        public void transfererLivre()
        {
            UserSelectionne.mesLivres.Add(LivreSelectionne);
            UserActif.mesLivres.Remove(LivreSelectionne);
            _biblio.SauvegarderXml(_nomFichier);
        }

        public void attenteVersTraite()
        {
            UserSelectionne.CommandeTraites.Add(LivreSelectionne);
            UserSelectionne.CommandeEnAttente.Remove(LivreSelectionne);
            _biblio.SauvegarderXml(_nomFichier);
            _biblio.remplirTousCmdEnAttente();
            _biblio.remplirTousCmdTraites();
        }

        public void traiteVersLivres()
        {
            UserSelectionne.CommandeTraites.Remove(LivreSelectionne);
            UserSelectionne.mesLivres.Add(LivreSelectionne);
            _biblio.SauvegarderXml(_nomFichier);
            _biblio.remplirTousCmdTraites();
        }
    }
}
