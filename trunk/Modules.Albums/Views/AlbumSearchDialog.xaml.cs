using Common.Dialogs;
using Modules.Albums.ViewModels;
using System.Windows.Input;

namespace Modules.Albums.Views
{
    /// <summary>
    /// Interaction logic for AlbumSearchDialog.xaml
    /// </summary>
    public partial class AlbumSearchDialog : DialogWindow
    {
        public AlbumSearchDialog(IAlbumSearchViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
