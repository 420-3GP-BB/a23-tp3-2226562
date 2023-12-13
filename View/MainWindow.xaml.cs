﻿using Model;
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
    public partial class MainWindow : Window
    {
        public static RoutedCommand ModeAdministrateur = new RoutedCommand();
        ViewModelBibliotheque _viewModel;
        Livre livreSelectionne = new Livre();


        public MainWindow()
        {
            _viewModel = new ViewModelBibliotheque();
            InitializeComponent();

            DataContext = _viewModel;
            livresUtilisateurs.ItemsSource = _viewModel.UserActif.mesLivres;
            cmdAttenteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeEnAttente;
            cmdTraiteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeTraites;
            
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ModeAdministrateur_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(_viewModel.UserActif.Administrateur == true)
            {
                e.CanExecute = true;
            }
        }

        private void ModeAdministrateur_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Admin fenetreAdmin = new Admin(_viewModel);
            fenetreAdmin.Owner = this;
            fenetreAdmin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreAdmin.ShowDialog();
        }

        private void ChangerUser_Click(object sender, RoutedEventArgs e)
        {
            Utilisateurs fenetreChoix = new Utilisateurs(_viewModel);
            fenetreChoix.Owner = this;
            fenetreChoix.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreChoix.ShowDialog();
            _viewModel.ChangerUserActif(_viewModel.UserSelectionne.Nom);
            DataContext = _viewModel;
            livresUtilisateurs.ItemsSource = _viewModel.UserActif.mesLivres;
            cmdAttenteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeEnAttente;
            cmdTraiteUtilisateur.ItemsSource = _viewModel.UserActif.CommandeTraites;
        }

        private void Transferer_Click(object sender, RoutedEventArgs e)
        {
            Utilisateurs fenetreChoix = new Utilisateurs(_viewModel);
            fenetreChoix.Owner = this;
            fenetreChoix.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreChoix.ShowDialog();

            _viewModel.transfererLivre();
        }

        private void CommanderLivre_Click(object sender, RoutedEventArgs e)
        {
            CmdLivres fenetreCommander = new CmdLivres(_viewModel);
            fenetreCommander.Owner = this;
            fenetreCommander.WindowStartupLocation= WindowStartupLocation.CenterOwner;
            fenetreCommander.ShowDialog();
        }

        private void AnnulerCommande_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.annulerCmd(ListBox.SelectedItemProperty);
        }
    }
   
}

        
    
