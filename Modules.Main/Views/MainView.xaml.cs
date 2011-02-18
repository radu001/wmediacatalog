using System.Windows.Controls;
using Modules.Main.ViewModels;

namespace Modules.Main.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView(IMainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
