using System.Windows.Controls;
using Modules.WorkspaceSelector.ViewModels;

namespace Modules.WorkspaceSelector.Views
{
    /// <summary>
    /// Interaction logic for WorkspaceSelectorView.xaml
    /// </summary>
    public partial class WorkspaceSelectorView : UserControl
    {
        public WorkspaceSelectorView(IWorkspaceSelectorViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
