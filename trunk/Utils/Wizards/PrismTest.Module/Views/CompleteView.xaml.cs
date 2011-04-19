using System.Windows.Controls;
using PrismTest.Module.ViewModels;

namespace PrismTest.Module.Views
{
    /// <summary>
    /// Interaction logic for CompleteView.xaml
    /// </summary>
    public partial class CompleteView : UserControl
    {
        public CompleteView(ICompleteViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
