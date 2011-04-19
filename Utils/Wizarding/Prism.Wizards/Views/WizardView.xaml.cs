using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using Prism.Wizards.ViewModels;

namespace Prism.Wizards.Views
{
    /// <summary>
    /// Interaction logic for WizardView.xaml
    /// </summary>
    public partial class WizardView : UserControl
    {
        #region Dependency properties

        public static readonly DependencyProperty NavBarVisibleProperty =
            DependencyProperty.Register("NavBarVisible", typeof(Visibility), typeof(WizardView),
            new PropertyMetadata(Visibility.Collapsed));

        public Visibility NavBarVisible
        {
            get
            {
                return (Visibility)GetValue(NavBarVisibleProperty);
            }
            set
            {
                SetValue(NavBarVisibleProperty, value);
            }
        }
        

        #endregion

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

        //private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var step = ((TextBlock)sender).DataContext;
        //    var viewModel = DataContext as IWizardViewModel;
        //    viewModel.MoveToStepCommand.Execute(step);
        //}
    }
}
