using System.Windows.Controls;
using Modules.Tags.ViewModels;

namespace Modules.Tags.Views
{
    /// <summary>
    /// Interaction logic for TagEditView.xaml
    /// </summary>
    public partial class TagEditView : UserControl
    {
        public TagEditView(ITagEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
