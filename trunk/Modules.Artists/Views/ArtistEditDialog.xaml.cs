using System.Windows.Input;
using Common.Dialogs;
using Modules.Artists.ViewModels;

namespace Modules.Artists.Views
{
    /// <summary>
    /// Interaction logic for ArtistEditDialog.xaml
    /// </summary>
    public partial class ArtistEditDialog : DialogWindow
    {
        public ArtistEditDialog(IArtistEditViewModel viewModel)
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
