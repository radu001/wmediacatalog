using Common.Dialogs;
using Modules.Login.ViewModels;

namespace Modules.Login.Views
{
    /// <summary>
    /// Interaction logic for UserRegistrationView.xaml
    /// </summary>
    public partial class UserRegistrationView : DialogWindow, IUserRegistrationView
    {
        public UserRegistrationView(IUserRegistrationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void HeaderBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
