using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModel
{
    public class ViewModelBibliotheque : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        private string _nomFichier;
        private Bibliotheque _biblio;
        private Membre? _userActif;

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
        public List<Membre> LesMembres { get => _biblio.LesMembres; }

        public ViewModelBibliotheque()
        {
            _nomFichier = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "bibliotheque.xml";
            _biblio = new Bibliotheque();
            _biblio.ChargerFichierXml(_nomFichier);

            mettreAJourUserActif();

        }

        private void mettreAJourUserActif()
        {
            foreach (Membre membre in LesMembres)
            {
                if (membre.Nom.Equals(DernierUser))
                {
                    _userActif = membre;
                    //OnPropertyChange(nameof(_userActif));
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

    }
}
