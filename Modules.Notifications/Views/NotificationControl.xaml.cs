using System.Windows.Controls;
using Modules.Notifications.ViewModels;

namespace Modules.Notifications.Views
{
    /// <summary>
    /// Interaction logic for NotificationControl.xaml
    /// </summary>
    public partial class NotificationControl : UserControl
    {
        public NotificationControl(INotificationsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
