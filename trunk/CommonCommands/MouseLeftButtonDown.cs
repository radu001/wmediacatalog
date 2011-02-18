
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class MouseLeftButtonDown
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseLeftButtonDown),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control element = dependencyObject as Control;
            if (element != null)
            {
                MouseLeftButtonDownBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(MouseLeftButtonDown), new PropertyMetadata(CommandParameterProperty_Changed));

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

        private static MouseLeftButtonDownBehavior GetOrCreateBehavior(Control element)
        {
            MouseLeftButtonDownBehavior behavior = element.GetValue(MouseLeftButtonDownBehaviorProperty) as MouseLeftButtonDownBehavior;
            if (behavior == null)
            {
                behavior = new MouseLeftButtonDownBehavior(element);
                element.SetValue(MouseLeftButtonDownBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static MouseLeftButtonDownBehavior GetMouseLeftButtonDownBehavior(DependencyObject obj)
        {
            return (MouseLeftButtonDownBehavior)obj.GetValue(MouseLeftButtonDownBehaviorProperty);
        }

        public static void SetMouseLeftButtonDownBehavior(DependencyObject obj,
            MouseLeftButtonDownBehavior value)
        {
            obj.SetValue(MouseLeftButtonDownBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseLeftButtonDownBehaviorProperty =
            DependencyProperty.RegisterAttached("MouseLeftButtonDownBehavior",
                typeof(MouseLeftButtonDownBehavior), typeof(MouseLeftButtonDown), null);
    }
}
