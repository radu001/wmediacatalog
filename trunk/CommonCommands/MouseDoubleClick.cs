
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class MouseDoubleClick
    {
        #region CommandProperty

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseDoubleClick),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control element = dependencyObject as Control;
            if (element != null)
            {
                MouseDoubleClickBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(MouseDoubleClick), new PropertyMetadata(CommandParameterProperty_Changed));

        public static object GetCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        private static void CommandParameterProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control element = dependencyObject as Control;

            if (element != null)
                GetOrCreateBehavior(element).CommandParameter = e.NewValue;
        }

        #endregion

        private static MouseDoubleClickBehavior GetOrCreateBehavior(Control element)
        {
            MouseDoubleClickBehavior behavior = element.GetValue(MouseDoubleClickBehaviorProperty) as MouseDoubleClickBehavior;
            if (behavior == null)
            {
                behavior = new MouseDoubleClickBehavior(element);
                element.SetValue(MouseDoubleClickBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static MouseDoubleClickBehavior GetMouseDoubleClickBehavior(DependencyObject obj)
        {
            return (MouseDoubleClickBehavior)obj.GetValue(MouseDoubleClickBehaviorProperty);
        }

        public static void SetMouseDoubleClickBehavior(DependencyObject obj,
            MouseDoubleClickBehavior value)
        {
            obj.SetValue(MouseDoubleClickBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseDoubleClickBehaviorProperty =
            DependencyProperty.RegisterAttached("MouseDoubleClickBehavior",
                typeof(MouseDoubleClickBehavior), typeof(MouseDoubleClick), null);
    }
}
