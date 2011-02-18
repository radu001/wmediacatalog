using Common.Dialogs;

namespace Modules.DatabaseSettings.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DbSettingsView : DialogWindow
    {
        public DbSettingsView()
        {
            InitializeComponent();
        }

        private void HeaderBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void DialogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
