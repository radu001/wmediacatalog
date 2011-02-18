
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class MouseDown
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MouseDown),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control element = dependencyObject as Control;
            if (element != null)
            {
                MouseDownBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(MouseDown), new PropertyMetadata(CommandParameterProperty_Changed));

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

        private static MouseDownBehavior GetOrCreateBehavior(Control element)
        {
            MouseDownBehavior behavior = element.GetValue(MouseDownBehaviorProperty) as MouseDownBehavior;
            if (behavior == null)
            {
                behavior = new MouseDownBehavior(element);
                element.SetValue(MouseDownBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static MouseDownBehavior GetMouseDownBehavior(DependencyObject obj)
        {
            return (MouseDownBehavior)obj.GetValue(MouseDownBehaviorProperty);
        }

        public static void SetMouseDownBehavior(DependencyObject obj,
            MouseDownBehavior value)
        {
            obj.SetValue(MouseDownBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseDownBehaviorProperty =
            DependencyProperty.RegisterAttached("MouseDownBehavior",
                typeof(MouseDownBehavior), typeof(MouseDown), null);
    }
}
