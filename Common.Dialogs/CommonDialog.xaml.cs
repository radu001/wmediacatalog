
using System.Windows;
namespace Common.Dialogs
{
    /// <summary>
    /// Interaction logic for CommonDialog.xaml
    /// </summary>
    public partial class CommonDialog : DialogWindow
    {
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(CommonDialog));

        public object DialogContent
        {
            get
            {
                return GetValue(DialogContentProperty);
            }
            set
            {
                SetValue(DialogContentProperty, value);
            }
        }

        public CommonDialog()
        {
            InitializeComponent();
        }

        private void HeaderBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
