using System.Windows.Controls;

namespace Common.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for ConfirmDialogView.xaml
    /// </summary>
    public partial class ConfirmDialogView : UserControl
    {
        public string MessageText { get; set; }

        public ConfirmDialogView()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
