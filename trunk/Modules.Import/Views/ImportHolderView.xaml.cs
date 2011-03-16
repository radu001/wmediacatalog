using System.Windows.Controls;
using Modules.Import.ViewModels;

namespace Modules.Import.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportHolderView : UserControl
    {
        public ImportHolderView(IImportViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
