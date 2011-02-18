using System.Windows.Controls;
using Modules.DatabaseSettings.ViewModels;

namespace Modules.DatabaseSettings.Views
{
    /// <summary>
    /// Interaction logic for ConnectionSettingsView.xaml
    /// </summary>
    public partial class ConnectionSettingsView : UserControl
    {
        public ConnectionSettingsView(IConnectionSettingsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
