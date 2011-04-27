using System.Windows.Controls;
using Modules.Import.ViewModels.Wizard;

namespace Modules.Import.Views.Wizard
{
    /// <summary>
    /// Interaction logic for CompleteStep.xaml
    /// </summary>
    public partial class CompleteStep : UserControl
    {
        public CompleteStep(ICompleteViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
