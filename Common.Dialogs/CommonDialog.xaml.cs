
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
namespace Common.Dialogs
{
    /// <summary>
    /// Interaction logic for CommonDialog.xaml
    /// </summary>
    public partial class CommonDialog : DialogWindow
    {
        #region Dependency properties

        #region IsBusy

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(CommonDialog));

        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }

        #endregion

        #region DialogContent

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(FrameworkElement), typeof(CommonDialog),
            new PropertyMetadata(null, OnDependencyPropertyChanged));

        public FrameworkElement DialogContent
        {
            get
            {
                return (FrameworkElement)GetValue(DialogContentProperty);
            }
            set
            {
                SetValue(DialogContentProperty, value);
            }
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommonDialog dialog = d as CommonDialog;
            FrameworkElement newContent = e.NewValue as FrameworkElement;
            if (newContent != null)
            {
                IDialogViewModel viewModel = newContent.DataContext as IDialogViewModel;
                if (viewModel != null)
                {
                    SetupContent(viewModel, dialog, newContent);
                }
                else
                {
                    SetupContent(null, dialog, newContent);

                    newContent.DataContextChanged += (sender, ec) =>
                    {
                        IDialogViewModel contentViewModel = ec.NewValue as IDialogViewModel;
                        SetupContent(contentViewModel, dialog, newContent);
                    };
                }
            }
        }

        private static void SetupContent(IDialogViewModel viewModel, CommonDialog dialog, FrameworkElement content)
        {
            if (viewModel != null)
            {
                dialog.SetBinding(IsBusyProperty, new Binding()
                {
                    Source = viewModel,
                    Path = new PropertyPath("IsBusy")
                });
                dialog.CancelButton.Command = viewModel.CancelCommand;
                dialog.CancelHeaderButton.Command = viewModel.CancelCommand;
                dialog.OkButton.Command = viewModel.SuccessCommand;
                dialog.OkButton.CommandParameter = dialog;
                dialog.SetBinding(WindowResultProperty, new Binding()
                {
                    Source = viewModel,
                    Path = new PropertyPath("DialogResult"),
                    Mode = BindingMode.TwoWay
                });
            }
            else
            {
                dialog.CancelButton.Command = dialog.DefaultCancelCommand;
                dialog.CancelHeaderButton.Command = dialog.DefaultCancelCommand;
                dialog.OkButton.Command = dialog.DefaultOkCommand;
            }
            dialog.Width = content.Width;
            dialog.Height = content.Height + 100;
        }

        #endregion

        #region HeaderText

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(CommonDialog));

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

        #endregion

        #region ButtonsSettings

        public static readonly DependencyProperty DialogButtonsProperty =
            DependencyProperty.Register("DialogButtons", typeof(DialogButtons), typeof(CommonDialog),
            new PropertyMetadata(DialogButtons.Ok | DialogButtons.Cancel));

        public DialogButtons DialogButtons
        {
            get
            {
                return (DialogButtons)GetValue(DialogButtonsProperty);
            }
            set
            {
                SetValue(DialogButtonsProperty, value);
            }
        }

        #endregion

        #endregion

        public CommonDialog()
        {
            InitializeComponent();

            DefaultOkCommand = new DelegateCommand<object>(OnDefaultOkCommand);
            DefaultCancelCommand = new DelegateCommand<object>(OnDefaultCancelCommand);

            Closing += new System.ComponentModel.CancelEventHandler(CommonDialog_Closing);
        }

        private void CommonDialog_Closing(object sender, CancelEventArgs e)
        {
            this.Closing -= CommonDialog_Closing;

            if (DialogContent != null && DialogContent.DataContext != null)
            {
                var viewModel = DialogContent.DataContext as IDialogViewModel;
                if (viewModel != null)
                {
                    viewModel.DialogClosingCommand.Execute(this);
                }
            }
        }

        public DelegateCommand<object> DefaultOkCommand { get; private set; }

        public DelegateCommand<object> DefaultCancelCommand { get; private set; }

        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void OnDefaultOkCommand(object parameter)
        {
            WindowResult = true;
        }

        private void OnDefaultCancelCommand(object parameter)
        {
            WindowResult = false;
        }
    }
}
