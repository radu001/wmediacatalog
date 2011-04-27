using System.Windows.Controls;
using Modules.Import.ViewModels.Wizard;

namespace Modules.Import.Views.Wizard
{
    /// <summary>
    /// Interaction logic for BulkImportStep.xaml
    /// </summary>
    public partial class BulkImportStep : UserControl
    {
        public BulkImportStep(IBulkImportViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
