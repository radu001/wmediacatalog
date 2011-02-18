using System.Windows.Input;
using Common.Dialogs;
using Modules.Tags.ViewModels;

namespace Modules.Tags.Views
{
    /// <summary>
    /// Interaction logic for TagEditView.xaml
    /// </summary>
    public partial class TagEditView : DialogWindow
    {
        public TagEditView(ITagEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
