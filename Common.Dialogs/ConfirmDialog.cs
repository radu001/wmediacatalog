
using System;
using System.Windows;
using Common.Dialogs.Views;
namespace Common.Dialogs
{
    public partial class ConfirmDialog : CommonDialog
    {
        #region Dependency properties

        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.RegisterAttached("MessageText", typeof(string), typeof(DialogWindow),
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
            ConfirmDialog dialog = d as ConfirmDialog;
            if (dialog != null)
            {
                ConfirmDialogView view = dialog.DialogContent as ConfirmDialogView;
                if (view != null)
                {
                    view.MessageText = e.NewValue != null ? e.NewValue.ToString() : String.Empty;
                }
            }
        }


        #endregion

        public ConfirmDialog()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            DialogContent = new ConfirmDialogView();

        }
    }
}
