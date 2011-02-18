using Common.Dialogs;
using Modules.Albums.ViewModels;

namespace Modules.Albums.Views
{
    /// <summary>
    /// Interaction logic for GenreEditView.xaml
    /// </summary>
    public partial class GenreEditDialog : DialogWindow
    {
        public GenreEditDialog(IGenreEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void HeaderBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
