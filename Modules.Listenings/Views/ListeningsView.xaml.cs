using System.Windows.Controls;
using Modules.Listenings.ViewModels;

namespace Modules.Listenings.Views
{
    /// <summary>
    /// Interaction logic for ListeningsView.xaml
    /// </summary>
    public partial class ListeningsView : UserControl
    {
        public ListeningsView(IListeningsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
