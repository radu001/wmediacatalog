using System.Windows.Controls;
using Modules.Import.ViewModels.Wizard;

namespace Modules.Import.Views.Wizard
{
    /// <summary>
    /// Interaction logic for TagsProviderStep.xaml
    /// </summary>
    public partial class TagsProviderStep : UserControl
    {
        public TagsProviderStep(ITagsProviderStepViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
