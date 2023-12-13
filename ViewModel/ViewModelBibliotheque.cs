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
        // déclaration

        public event PropertyChangedEventHandler? PropertyChanged;
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        private string _nomFichier;
        // biblitheque
        private Bibliotheque _biblio;
        // Le membre actif 
        private Membre? _userActif;
        // le livre selectionnée
        private Livre _livreSelectionne;
        // le membre sélectionné
        private Membre _userSelectionne;
        // la commande sélectionné
        private Commande _commandeSelectionnee;
        // unLivre est le livre qu'on veut commander
        public Livre UnLivre { get; set; }
        public Dictionary<string, Livre> Dictionnaire { get => _biblio.Dictionnaire; }
        public ObservableCollection<Membre> LesMembres { get => _biblio.LesMembres; }
        public ObservableCollection<Commande> LesCommandesEnAttente { get => _biblio.TousCommandesEnAttente; }
        public ObservableCollection<Commande> LesCommandesTraites { get => _biblio.TousCommandesTraites; }
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
        
        // Le constructeur
        public ViewModelBibliotheque()
        {
            _nomFichier = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "bibliotheque.xml";
            // initialiser la bibliotheque
            _biblio = new Bibliotheque();
            // charger le fichier 
            _biblio.ChargerFichierXml(_nomFichier);
            // appeler la methode pour mettre a jour l'utilisateur actif
            mettreAJourUserActif();
            UnLivre = new Livre();

        }

        // La méthode qui sert à mettre à jour l'utilisateur actif
        private void mettreAJourUserActif()
        {
            // pour chaque membre qui existe
            foreach (Membre membre in LesMembres)
            {
                // si son nom égale l'attribut dernierUser
                if (membre.Nom.Equals(DernierUser))
                {
                    // on met ce membre comme user actif
                    _userActif = membre;
                    // on lance l'évènement
                    OnPropertyChange(nameof(_userActif));
                }
            }

        }

        // la méthode pour mettre à jour 
        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // la méthode pour changer le nom du user actif
        public void ChangerUserActif(string nouveauUser)
        {
            DernierUser = nouveauUser;
            mettreAJourUserActif();
            _biblio.SauvegarderXml(_nomFichier);
            OnPropertyChange(nameof(DernierUser));
        }

        // la méthode qui valide l'ISBN-13
        public bool isbnValide()
        {
            bool valide = false;
            if(UnLivre.Isbn13.Length == 13)
            {
                valide = true;
            }
            return valide;
        }

        // La méthode qui valide les chaines : titre, auteur, editeur
        public bool chaineValide()
        {
            bool valide = false;
            if (!UnLivre.Titre.Equals("") && !UnLivre.Auteur.Equals("") && !UnLivre.Editeur.Equals(""))
            {
                valide = true;
            }
            return valide;
        }

        // la methode qui valide l'année
        public bool anneeValide()
        {
            bool valide = false;
            if(UnLivre.Annee >= -3000)
            {
                valide = true;
            }

            return valide;
        }

        // La méthode pour ajouter aux commandes
        public void ajouterAuxCommandes()
        {
            //on prend l'ISBN du livre commandé, on ramène le livre du dictionnaire puis on l'ajoute aux commande en attente de l'utilisateur
            UserActif.CommandeEnAttente.Add(Dictionnaire[UnLivre.Isbn13]);
            // on remplis toutes les commandes en attente
            _biblio.remplirTousCmdEnAttente();
            // on sauvegarde les changements
            _biblio.SauvegarderXml(_nomFichier);
        }

        // méthode qui vérifie si le livre existe dans le dictionnaire
        public bool existeDansLeDic()
        {
            bool existe = false;
            // on parcours chaque livre du dictionnaire puis on compare son ISBN13 avec le livre commandé
            foreach (KeyValuePair<string, Livre> livredic in Dictionnaire)
            {
                if (livredic.Key.Equals(UnLivre.Isbn13)){
                    existe = true;
                }   
            }
            return existe;
        }

        // une méthode qui vérifie si le livre existe dans le compte de l'utilisateur
        public bool existeDansSonCompte()
        {
            bool existe = false;
            // S'il existe dans ses livres
            foreach(Livre livre in UserActif.mesLivres)
            {
                if (livre.Isbn13.Equals(UnLivre.Isbn13))
                {
                    existe = true;
                }
            }
            // ou dans ses commandes en attente
            foreach(Livre livre in UserActif.CommandeEnAttente)
            {
                if (livre.Isbn13.Equals(UnLivre.Isbn13))
                {
                    existe = true;
                }
            }
            // ou dans les commandes traites
            foreach (Livre livre in UserActif.CommandeTraites)
            {
                if (livre.Isbn13.Equals(UnLivre.Isbn13))
                {
                    existe = true;
                }
            }
            return existe;
        }

        // une méthode pour annuler une commande
        public void annulerCmd(object selectedItemProperty)
        {
            // on l'enleve des commandes en attente de l'utilisateur
            UserActif.CommandeEnAttente.Remove(LivreSelectionne);
            // on met à jour les commande en attente
            _biblio.remplirTousCmdEnAttente();
            // on sauvegarde les changements
            _biblio.SauvegarderXml(_nomFichier);
        }

        // méthode pour transférer un livre
        public void transfererLivre()
        {
            // le user sélectionné est celui qu'on a selectionné dans le fenetre des choix d'utilisateur
            // on ajoute dans ses livre le livre qu'on a transféré
            UserSelectionne.mesLivres.Add(LivreSelectionne);
            // on enlève des livres de l'utilisateur actif le livre sélectionné
            UserActif.mesLivres.Remove(LivreSelectionne);
            _biblio.SauvegarderXml(_nomFichier);
        }

        // méthode pour transférer le livre de la liste en attente vers traités
        public void attenteVersTraite()
        {
            // on ajoute le livre aux commandes traités et on l'enlève des commandes en attente
            UserSelectionne.CommandeTraites.Add(LivreSelectionne);
            UserSelectionne.CommandeEnAttente.Remove(LivreSelectionne);
            _biblio.SauvegarderXml(_nomFichier);
            _biblio.remplirTousCmdEnAttente();
            _biblio.remplirTousCmdTraites();
        }

        // méthode pour transférer le livre de la liste traité vers la liste des livres de l'user sélectionné
        public void traiteVersLivres()
        {
            // on ajoute le livre aux livres et on l'enlève des commandes traitées
            UserSelectionne.CommandeTraites.Remove(LivreSelectionne);
            UserSelectionne.mesLivres.Add(LivreSelectionne);
            _biblio.SauvegarderXml(_nomFichier);
            _biblio.remplirTousCmdTraites();
        }
    }
}
