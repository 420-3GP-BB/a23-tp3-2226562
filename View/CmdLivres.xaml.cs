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
    public partial class CmdLivres : Window
    {
        public static RoutedCommand ConfirmerCommand = new RoutedCommand();

        ViewModelBibliotheque _viewModel;

        public CmdLivres(ViewModelBibliotheque _vm)
        {
            _viewModel = new ViewModelBibliotheque();

            InitializeComponent();
            _viewModel = _vm;
            
            DataContext = _viewModel.UnLivre;

        }

        private void ConfirmerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
             e.CanExecute = _viewModel.isbnValide() && _viewModel.anneeValide() && _viewModel.chaineValide();
        }
        public void ConfirmerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(_viewModel.existeDansLeDic())
            {
                if (_viewModel.existeDansSonCompte())
                {
                    MessageBox.Show("Le livre existe déjà dans votre compte");
                }
                else
                {
                    MessageBox.Show("Livre ajouté aux commandes en attente avec succès");
                    _viewModel.ajouterAuxCommandes();
                }
            }
            else
            {
                MessageBox.Show("Le livre n'existe pas dans la bibliotheque");
            }

            Close();
        }

        
    }
}
