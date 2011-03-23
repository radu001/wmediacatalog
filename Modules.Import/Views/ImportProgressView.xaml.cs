using System.Windows.Controls;
using Modules.Import.ViewModels;

namespace Modules.Import.Views
{
    /// <summary>
    /// Interaction logic for ImportProgressView.xaml
    /// </summary>
    public partial class ImportProgressView : UserControl
    {
        public ImportProgressView(IImportProgressViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
