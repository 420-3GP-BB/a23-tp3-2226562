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
    /// Interaction logic for CmdLivres.xaml
    /// </summary>
   
    // La fenetre pour commander un livre
    public partial class CmdLivres : Window
    {
        // une commande pour le bouton 
        public static RoutedCommand ConfirmerCommand = new RoutedCommand();

        ViewModelBibliotheque _viewModel;

        public CmdLivres(ViewModelBibliotheque _vm)
        {
            _viewModel = new ViewModelBibliotheque();

            InitializeComponent();
            _viewModel = _vm;
            
            // Associer le dataContext avec le livre qu'on veut commander
            DataContext = _viewModel.UnLivre;

        }

        private void ConfirmerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // on peut exécuter seulement si les méthodes qui vérifient l'ISBN13, l'année et les chaines sont True
             e.CanExecute = _viewModel.isbnValide() && _viewModel.anneeValide() && _viewModel.chaineValide();
        }
        public void ConfirmerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // lorsqu'on clique sur le bouton confirmer
            // Si le livre existe dans le dictionnaire 
            if(_viewModel.existeDansLeDic())
            {
                // Si le livre existe dans son compte
                if (_viewModel.existeDansSonCompte())
                {
                    MessageBox.Show("Le livre existe déjà dans votre compte");
                }
                else
                // S'il n'existe pas dans son compte on l'ajoute et on appelle la méthode qui effectuera cette étape
                {
                    MessageBox.Show("Livre ajouté aux commandes en attente avec succès");
                    _viewModel.ajouterAuxCommandes();

                    DataContext = _viewModel.UnLivre;
                    _viewModel.UnLivre = new Model.Livre();
                    
                }
            }
            // s'il n'existe pas dans le dictionnaire
            else
            {
                MessageBox.Show("Le livre n'existe pas dans la bibliotheque");
            }

            Close();
        }

        
    }
}
