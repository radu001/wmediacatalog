using System.Windows;

namespace Common.Dialogs
{
    /// <summary>
    /// Interaction logic for NotificationDialog.xaml
    /// </summary>
    public partial class NotificationDialog : DialogWindow
    {
        public NotificationDialog(object dataContext)
        {
            InitializeComponent();

            this.DataContext = dataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void HeaderBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
