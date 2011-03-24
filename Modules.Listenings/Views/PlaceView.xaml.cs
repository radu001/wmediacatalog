using System.Windows.Controls;
using Modules.Listenings.ViewModels;

namespace Modules.Listenings.Views
{
    /// <summary>
    /// Interaction logic for MoodDialog.xaml
    /// </summary>
    public partial class PlaceView : UserControl
    {
        public PlaceView(IPlaceViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
