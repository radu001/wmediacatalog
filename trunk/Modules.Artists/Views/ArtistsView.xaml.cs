using System.Windows.Controls;
using Modules.Artists.ViewModels;

namespace Modules.Artists.Views
{
    /// <summary>
    /// Interaction logic for ArtistsView.xaml
    /// </summary>
    public partial class ArtistsView : UserControl
    {
        public ArtistsView(IArtistsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
