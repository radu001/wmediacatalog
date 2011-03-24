using System.Windows.Controls;
using Modules.Listenings.ViewModels;

namespace Modules.Listenings.Views
{
    /// <summary>
    /// Interaction logic for MoodDialog.xaml
    /// </summary>
    public partial class MoodView : UserControl
    {
        public MoodView(IMoodViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
