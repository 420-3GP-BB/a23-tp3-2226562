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
using System.Xml.Serialization;
using ViewModel;
namespace View
{
    // La fenetre principale
    public partial class MainWindow : Window
    {
        // une commande pour le modeAdministateur
        public static RoutedCommand ModeAdministrateur = new RoutedCommand();
        // On crée le viewModel qu'on va utiliser 
        ViewModelBibliotheque _viewModel;


        public MainWindow()
        {
            _viewModel = new ViewModelBibliotheque();
            InitializeComponent();

            // On associe le datacontext avec ce viewModel
            DataContext = _viewModel;
            // la listBox livresUtilisateur sera associé avec les livres du membre actif
            livresUtilisateurs.ItemsSource = _viewModel.UserActif.mesLivres;
            // la listBox cmdAttenteUtilisateur sera associé avec les commandes en attente du membre actif
            cmdAttenteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeEnAttente;
            // la listBox cmdTraiteUtilisateur sera associé avec les commandes traites du membre actif
            cmdTraiteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeTraites;
            
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ModeAdministrateur_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // on peut éxecuter le bouton pour accéder à l'administrateur seulement si l'utilisateur actif est admin
            if(_viewModel.UserActif.Administrateur == true)
            {
                e.CanExecute = true;
            }
        }

        // On crée une fenetre d'administration lorsqu'on clique sur le bouton "Mode administrateur"
        private void ModeAdministrateur_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Admin fenetreAdmin = new Admin(_viewModel);
            fenetreAdmin.Owner = this;
            fenetreAdmin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreAdmin.ShowDialog();
        }

        // Lorsqu'on clique sur le bouton "Changer utilisateur"
        private void ChangerUser_Click(object sender, RoutedEventArgs e)
        {
            Utilisateurs fenetreChoix = new Utilisateurs(_viewModel);
            fenetreChoix.Owner = this;
            fenetreChoix.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreChoix.ShowDialog();

            // On appelle la méthode qui change l'utilisateur actif et lui donnant le nom du nouveau utilisateur (utilisateur sélectionné de la combobox)
            _viewModel.ChangerUserActif(_viewModel.UserSelectionne.Nom);

            // après avoir changé d'utilisateur
            // on refait le datacontext
            DataContext = _viewModel;
            // et associer les 3 listbox avec les collection du nouvel utilisateur, ça sert à mettre à jour l'affichage
            livresUtilisateurs.ItemsSource = _viewModel.UserActif.mesLivres;
            cmdAttenteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeEnAttente;
            cmdTraiteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeTraites;
        }

        // lorsqu'on clique sur le bouton "Transférer" on transfert un livre d'un utilisateur (l'actif) à un autre aux choix
        private void Transferer_Click(object sender, RoutedEventArgs e)
        {
            // on choisit l'utilisateur de la destination
            Utilisateurs fenetreChoix = new Utilisateurs(_viewModel);
            fenetreChoix.Owner = this;
            fenetreChoix.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreChoix.ShowDialog();
            // on appelle la méthode qui fait cette transaction
            _viewModel.transfererLivre();
        }

        // Lorsqu'on clique sur le bouton CommanderLivre, on crée une fenetre pour commander un nouveau livre.
        private void CommanderLivre_Click(object sender, RoutedEventArgs e)
        {
            CmdLivres fenetreCommander = new CmdLivres(_viewModel);
            fenetreCommander.Owner = this;
            fenetreCommander.WindowStartupLocation= WindowStartupLocation.CenterOwner;
            fenetreCommander.ShowDialog();
        }

        // lorsqu'on clique sur le bouton "Annuler commande"
        private void AnnulerCommande_Click(object sender, RoutedEventArgs e)
        {
            // on appelle la méthode qui effectue cette étape
            _viewModel.annulerCmd(ListBox.SelectedItemProperty);
        }
    }
   
}

        
    
