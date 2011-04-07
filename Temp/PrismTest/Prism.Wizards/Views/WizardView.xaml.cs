using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Regions;
using Prism.Wizards.ViewModels;

namespace Prism.Wizards.Views
{
    /// <summary>
    /// Interaction logic for WizardView.xaml
    /// </summary>
    public partial class WizardView : UserControl
    {
        public WizardView(IWizardViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            SetupStepsRegion(viewModel);
        }

        private void SetupStepsRegion(IWizardViewModel viewModel)
        {
            ContentControl c = new ContentControl();
            LayoutRoot.Children.Add(c);
            Grid.SetRow(c, 0);
            RegionManager.SetRegionName(c, viewModel.StepRegionName);
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var step = ((TextBlock)sender).DataContext;
            var viewModel = DataContext as IWizardViewModel;
            viewModel.MoveToStepCommand.Execute(step);
        }
    }
}
