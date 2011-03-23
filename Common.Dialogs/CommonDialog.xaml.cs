
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
                DialogViewModelBase viewModel = newContent.DataContext as DialogViewModelBase;
                if (viewModel != null)
                {
                    SetupContent(viewModel, dialog, newContent);
                }
                else
                {
                    newContent.DataContextChanged += (sender, ec) =>
                    {
                        DialogViewModelBase contentViewModel = ec.NewValue as DialogViewModelBase;
                        if (contentViewModel != null)
                        {
                            SetupContent(contentViewModel, dialog, newContent);
                        }
                        else
                        {
                            dialog.CancelButton.Command = new DelegateCommand<object>(a =>
                            {
                                dialog.DialogResult = false;
                            });
                            dialog.CancelHeaderButton.Command = new DelegateCommand<object>(a =>
                            {
                                dialog.DialogResult = false;
                            });
                            dialog.OkButton.Command = new DelegateCommand<object>(a =>
                            {
                                dialog.DialogResult = true;
                            });
                        }
                    };
                }
            }
        }

        private static void SetupContent(DialogViewModelBase viewModel, CommonDialog dialog, FrameworkElement content)
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
            dialog.Width = content.Width;
            dialog.Height = content.Height + 100;
        }

        public CommonDialog()
        {
            InitializeComponent();
        }

        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
