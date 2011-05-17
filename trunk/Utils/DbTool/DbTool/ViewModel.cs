
using System;
using System.Windows;
using System.Windows.Input;
namespace DbTool
{
    public class ViewModel : IViewModel
    {
        public ViewModel()
        {
        }



        #region IViewModel Members

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        #endregion
    }

    public class DelegateCommand<T> : ICommand
    {
        public DelegateCommand(Action<T> action)
        {
            this.action = action;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (action != null)
                action((T)parameter);
        }

        #endregion

        private Action<T> action;
    }

    public class Loaded : DependencyObject
    {
        public static readonly DependencyProperty LoadedProperty =
            DependencyProperty.RegisterAttached("Loaded", typeof(ICommand), typeof(UIElement));

        public static void SetLoaded(UIElement element, ICommand value)
        {
            element.SetValue(LoadedProperty, value);
        }
        public static ICommand GetLoaded(UIElement element)
        {
            return (ICommand)element.GetValue(LoadedProperty);
        }

        public Loaded()
        {
        }
    }
}
