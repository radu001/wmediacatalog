using System.Windows.Controls;
using Modules.Albums.ViewModels;

namespace Modules.Albums.Views
{
    /// <summary>
    /// Interaction logic for AlbumSearchDialog.xaml
    /// </summary>
    public partial class AlbumSearchView : UserControl
    {
        public AlbumSearchView(IAlbumSearchViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
