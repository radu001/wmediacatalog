using System.Windows.Controls;
using Modules.Albums.ViewModels;

namespace Modules.Albums.Views
{
    /// <summary>
    /// Interaction logic for AlbumEditDialog.xaml
    /// </summary>
    public partial class AlbumEditView : UserControl
    {
        public AlbumEditView(IAlbumEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
