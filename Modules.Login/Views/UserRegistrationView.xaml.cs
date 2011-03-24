using Common.Dialogs;
using Modules.Login.ViewModels;
using System.Windows.Controls;

namespace Modules.Login.Views
{
    /// <summary>
    /// Interaction logic for UserRegistrationView.xaml
    /// </summary>
    public partial class UserRegistrationView : UserControl
    {
        public UserRegistrationView(IUserRegistrationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
