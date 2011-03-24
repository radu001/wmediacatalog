using Common.Dialogs;
using Modules.Albums.ViewModels;
using System.Windows.Controls;

namespace Modules.Albums.Views
{
    /// <summary>
    /// Interaction logic for GenreEditView.xaml
    /// </summary>
    public partial class GenreEditView : UserControl
    {
        public GenreEditView(IGenreEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
