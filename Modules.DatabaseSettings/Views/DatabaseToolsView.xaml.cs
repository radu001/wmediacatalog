using System.Windows.Controls;
using Modules.DatabaseSettings.ViewModels;

namespace Modules.DatabaseSettings.Views
{
    /// <summary>
    /// Interaction logic for DatabaseToolsView.xaml
    /// </summary>
    public partial class DatabaseToolsView : UserControl
    {
        public DatabaseToolsView(IDatabaseToolsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
