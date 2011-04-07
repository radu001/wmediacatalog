using System.Windows.Controls;
using PrismTest.Module.ViewModels;

namespace PrismTest.Module.Views
{
    /// <summary>
    /// Interaction logic for Step1View.xaml
    /// </summary>
    public partial class Step1View : UserControl
    {
        public Step1View(IStep1ViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
