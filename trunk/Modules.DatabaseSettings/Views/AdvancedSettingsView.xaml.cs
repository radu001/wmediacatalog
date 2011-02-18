using System.Windows.Controls;
using Modules.DatabaseSettings.ViewModels;

namespace Modules.DatabaseSettings.Views
{
    /// <summary>
    /// Interaction logic for AdvancedSettingsView.xaml
    /// </summary>
    public partial class AdvancedSettingsView : UserControl
    {
        public AdvancedSettingsView(IAdvancedSettingsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
