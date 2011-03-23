using System.Windows.Controls;
using Modules.Artists.ViewModels;

namespace Modules.Artists.Views
{
    /// <summary>
    /// Interaction logic for ArtistEditDialog.xaml
    /// </summary>
    public partial class ArtistEditView : UserControl
    {
        public ArtistEditView(IArtistEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
