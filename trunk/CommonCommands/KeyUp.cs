
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class KeyUp
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(KeyUp),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control element = dependencyObject as Control;
            if (element != null)
            {
                KeyUpBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(KeyUp), new PropertyMetadata(CommandParameterProperty_Changed));

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

        private static KeyUpBehavior GetOrCreateBehavior(Control element)
        {
            KeyUpBehavior behavior = element.GetValue(KeyUpBehaviorProperty) as KeyUpBehavior;
            if (behavior == null)
            {
                behavior = new KeyUpBehavior(element);
                element.SetValue(KeyUpBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static KeyUpBehavior GetKeyUpBehavior(DependencyObject obj)
        {
            return (KeyUpBehavior)obj.GetValue(KeyUpBehaviorProperty);
        }

        public static void SetKeyUpBehavior(DependencyObject obj,
            KeyUpBehavior value)
        {
            obj.SetValue(KeyUpBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyUpBehaviorProperty =
            DependencyProperty.RegisterAttached("KeyUpBehavior",
                typeof(KeyUpBehavior), typeof(KeyUp), null);
    }
}
