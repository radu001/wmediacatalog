using System.Windows.Controls;
using Modules.Import.ViewModels.Wizard;

namespace Modules.Import.Views.Wizard
{
    /// <summary>
    /// Interaction logic for ScanProgressStep.xaml
    /// </summary>
    public partial class ScanProgressStep : UserControl
    {
        public ScanProgressStep(IScanProgressStepViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
