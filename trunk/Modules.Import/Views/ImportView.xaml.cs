using System.Windows.Controls;
using Modules.Import.ViewModels;

namespace Modules.Import.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : UserControl
    {
        public ImportView(IImportViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
