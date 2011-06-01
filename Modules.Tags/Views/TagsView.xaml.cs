using System.Windows.Controls;
using Modules.Tags.ViewModels;

namespace Modules.Tags.Views
{
    /// <summary>
    /// Interaction logic for TagsView.xaml
    /// </summary>
    public partial class TagsView : UserControl
    {
        public TagsView(ITagsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
