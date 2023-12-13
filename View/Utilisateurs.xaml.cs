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
    /// Interaction logic for Utilisateurs.xaml
    /// </summary>
    // la fenetre du choix d'utilisateur
    public partial class Utilisateurs : Window
    {
        // on crée un viewmodel
        ViewModelBibliotheque _viewModel;

        public Utilisateurs(ViewModelBibliotheque _vm)
        {
            _viewModel = new ViewModelBibliotheque();
            InitializeComponent();
            // on l'associe avec le viewModel qu'on travaille avec
            _viewModel = _vm;
            // On met le datacontext avec le viewmodel
            DataContext = _viewModel;
            // et puis la liste des utilisateurs dans l'interface avec la liste des membres
            ComboBoxUtilisateurs.ItemsSource = _viewModel.LesMembres;
        }

        // Quand on clique pour confirmer l'utilisateur
        private void ConfirmerChoix_Click(object sender, RoutedEventArgs e)
        {
            // L'attribut userSelectionne est l'item qu'on a séléctionné de la liste des choix d'utilisateurs.
            _viewModel.UserSelectionne = ComboBoxUtilisateurs.SelectedItem as Membre;
            Close();
        }

    }
}
