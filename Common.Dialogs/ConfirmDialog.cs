
using System.Windows;
using Common.Dialogs.Views;
namespace Common.Dialogs
{
    public partial class ConfirmDialog : CommonDialog
    {
        #region Dependency properties

        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register("MessageText", typeof(string), typeof(DialogWindow),
            new PropertyMetadata(OnMessageTextPropertyChanged));

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

        private static void OnMessageTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //ConfirmDialog dialog = d as ConfirmDialog;
            //if (dialog != null)
            //{
            //    ConfirmDialogView view = dialog.DialogContent as ConfirmDialogView;
            //    if (view != null)
            //    {
            //        view.MessageText = e.NewValue != null ? e.NewValue.ToString() : String.Empty;
            //    }
            //}
        }

        public static readonly DependencyProperty CheckBoxTextProperty =
            DependencyProperty.Register("CheckBoxText", typeof(string), typeof(ConfirmDialog));

        public string CheckBoxText
        {
            get
            {
                return (string)GetValue(CheckBoxTextProperty);
            }
            set
            {
                SetValue(CheckBoxTextProperty, value);
            }
        }

        public static readonly DependencyProperty CheckBoxCheckedProperty =
            DependencyProperty.Register("CheckBoxChecked", typeof(bool), typeof(ConfirmDialog));

        public bool CheckBoxChecked
        {
            get
            {
                return (bool)GetValue(CheckBoxCheckedProperty);
            }
            set
            {
                SetValue(CheckBoxCheckedProperty, value);
            }
        }

        #endregion

        public ConfirmDialog()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DialogContent = new ConfirmDialogView()
            {
                DataContext = this
            };
        }
    }
}
