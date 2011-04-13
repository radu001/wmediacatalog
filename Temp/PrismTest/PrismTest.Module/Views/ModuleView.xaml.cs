using System.Windows.Controls;
using PrismTest.Module.ViewModels;

namespace PrismTest.Module.Views
{
    /// <summary>
    /// Interaction logic for ModuleView.xaml
    /// </summary>
    public partial class ModuleView : UserControl
    {
        public ModuleView(IModuleViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
