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
    public partial class Admin : Window
    {
        ViewModelBibliotheque _viewModel;
        public Admin(ViewModelBibliotheque vm)
        {
            InitializeComponent();
            _viewModel = vm;

            DataContext = _viewModel;

            commandAttente.ItemsSource = _viewModel.LesCommandesEnAttente;
            commandTraites.ItemsSource = _viewModel.LesCommandesTraites;
        }

        private void Revenir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmdAttente_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.CommandeSelectionnee = commandAttente.SelectedItem as Commande;
            _viewModel.UserSelectionne = _viewModel.CommandeSelectionnee.LeMembre;
            _viewModel.LivreSelectionne = _viewModel.CommandeSelectionnee.LeLivre;
                //_viewModel.LivreSelectionne = commandAttente.SelectedItem as Livre;
            _viewModel.attenteVersTraite();
            
        }

        private void cmdTraites_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.CommandeSelectionnee = commandTraites.SelectedItem as Commande;
            _viewModel.UserSelectionne = _viewModel.CommandeSelectionnee.LeMembre;
            _viewModel.LivreSelectionne = _viewModel.CommandeSelectionnee.LeLivre;
            
            _viewModel.traiteVersLivres();
            
        }
    }
}
