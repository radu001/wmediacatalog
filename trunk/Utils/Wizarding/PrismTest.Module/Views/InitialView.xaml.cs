using System.Windows.Controls;
using PrismTest.Module.ViewModels;

namespace PrismTest.Module.Views
{
    /// <summary>
    /// Interaction logic for InitialView.xaml
    /// </summary>
    public partial class InitialView : UserControl
    {
        public InitialView(IInitialViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
