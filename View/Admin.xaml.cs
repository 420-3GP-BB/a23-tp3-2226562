using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    
    // La fenetre d'administrateur
    public partial class Admin : Window
    {
        ViewModelBibliotheque _viewModel;
        public Admin(ViewModelBibliotheque vm)
        {
            InitializeComponent();
            _viewModel = vm;

            DataContext = _viewModel;

            // Associer la listbox des commandes en attente avec la liste de toutes les commandes en attente
            commandAttente.ItemsSource = _viewModel.LesCommandesEnAttente;
            // Associer la listbox des commandes traités avec la liste de toutes les commandes traitées
            commandTraites.ItemsSource = _viewModel.LesCommandesTraites;
        }

        private void Revenir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmdAttente_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // lorsqu'on double click sur une commande dans la listbox des commandes en attente
            // L'attribut commandeSelectionnee est l'item sélectionné
            _viewModel.CommandeSelectionnee = commandAttente.SelectedItem as Commande;
            // Le userSelectionne sera le membre dans la commande CommandeSelectionnee
            _viewModel.UserSelectionne = _viewModel.CommandeSelectionnee.LeMembre;
            // Le livreSelectionne sera le livre dans la commande CommandeSelectionnee
            _viewModel.LivreSelectionne = _viewModel.CommandeSelectionnee.LeLivre;
            // On appelle la méthode qui envoie la commande de la liste en attente vers la liste des traitées
            _viewModel.attenteVersTraite();
            
        }

        private void cmdTraites_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // lorsqu'on double click sur une commande dans la listbox des commandes traiteés
            // L'attribut commandeSelectionnee est l'item sélectionné
            _viewModel.CommandeSelectionnee = commandTraites.SelectedItem as Commande;
            _viewModel.UserSelectionne = _viewModel.CommandeSelectionnee.LeMembre;
            _viewModel.LivreSelectionne = _viewModel.CommandeSelectionnee.LeLivre;
            
            // On appelle la méthode qui envoie la commande de la liste des traitées vers la liste des livres du membre sélectionné.
            _viewModel.traiteVersLivres();
            
        }
    }
}
