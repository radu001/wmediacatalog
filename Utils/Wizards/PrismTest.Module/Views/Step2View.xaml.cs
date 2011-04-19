using System.Windows.Controls;
using PrismTest.Module.ViewModels;

namespace PrismTest.Module.Views
{
    /// <summary>
    /// Interaction logic for Step2View.xaml
    /// </summary>
    public partial class Step2View : UserControl
    {
        public Step2View(IStep2ViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
