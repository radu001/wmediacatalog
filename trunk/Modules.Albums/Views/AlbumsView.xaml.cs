using System.Windows.Controls;
using Modules.Albums.ViewModels;

namespace Modules.Albums.Views
{
    /// <summary>
    /// Interaction logic for AlbumsView.xaml
    /// </summary>
    public partial class AlbumsView : UserControl
    {
        public AlbumsView(IAlbumsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
