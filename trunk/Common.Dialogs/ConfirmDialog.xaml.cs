
using System.Windows;
using System.Windows.Input;
namespace Common.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : DialogWindow
    {
        #region Dependency properties

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.RegisterAttached("HeaderText", typeof(string), typeof(DialogWindow));

        public string HeaderText
        {
            get
            {
                return (string)GetValue(HeaderTextProperty);
            }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }

        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.RegisterAttached("MessageText", typeof(string), typeof(DialogWindow));

        public string MessageText
        {
            get
            {
                return (string)GetValue(MessageTextProperty);
            }
            set
            {
                SetValue(MessageTextProperty, value);
            }
        }


        #endregion

        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
