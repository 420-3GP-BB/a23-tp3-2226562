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
    public partial class Utilisateurs : Window
    {
        ViewModelBibliotheque _viewModel;

        public Utilisateurs(ViewModelBibliotheque _vm)
        {
            _viewModel = new ViewModelBibliotheque();
            InitializeComponent();
            _viewModel = _vm;
            DataContext = _viewModel;
            ComboBoxUtilisateurs.ItemsSource = _viewModel.LesMembres;
        }

        private void ConfirmerChoix_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UserSelectionne = ComboBoxUtilisateurs.SelectedItem as Membre;
            Close();
        }

        /*private void ComboBoxUtilisateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Equipe equipe = ComboBoxEquipes.SelectedItem as Equipe;
            //ListBoxUtilisateurs.ItemsSource = equipe.Joueurs;
        SelectionChanged="ComboBoxUtilisateurs_SelectionChanged"
        }
        */
    }
}
