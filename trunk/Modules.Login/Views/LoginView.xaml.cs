using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Modules.Login.ViewModels;

namespace Modules.Login.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(ILoginViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            Loaded += new RoutedEventHandler(LoginView_Loaded);
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.Focus();
        }

        private void UserNameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                PasswordBox.Focus();
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                LoginButton.Focus();
        }
    }
}
