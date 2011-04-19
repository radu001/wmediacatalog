using System.Windows.Controls;
using Modules.Import.ViewModels.Wizard;

namespace Modules.Import.Views.Wizard
{
    /// <summary>
    /// Interaction logic for InitialStep.xaml
    /// </summary>
    public partial class InitialStep : UserControl
    {
        public InitialStep(IInitialStepViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
