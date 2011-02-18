using System.Windows.Input;
using Common.Dialogs;
using Modules.Albums.ViewModels;

namespace Modules.Albums.Views
{
    /// <summary>
    /// Interaction logic for AlbumEditDialog.xaml
    /// </summary>
    public partial class AlbumEditDialog : DialogWindow
    {
        public AlbumEditDialog(IAlbumEditViewModel viewModel)
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
