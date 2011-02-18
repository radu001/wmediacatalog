
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class KeyDown
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(KeyDown),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control element = dependencyObject as Control;
            if (element != null)
            {
                KeyDownBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(KeyDown), new PropertyMetadata(CommandParameterProperty_Changed));

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

        private static KeyDownBehavior GetOrCreateBehavior(Control element)
        {
            KeyDownBehavior behavior = element.GetValue(KeyDownBehaviorProperty) as KeyDownBehavior;
            if (behavior == null)
            {
                behavior = new KeyDownBehavior(element);
                element.SetValue(KeyDownBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static KeyDownBehavior GetKeyDownBehavior(DependencyObject obj)
        {
            return (KeyDownBehavior)obj.GetValue(KeyDownBehaviorProperty);
        }

        public static void SetKeyDownBehavior(DependencyObject obj,
            KeyDownBehavior value)
        {
            obj.SetValue(KeyDownBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyDownBehaviorProperty =
            DependencyProperty.RegisterAttached("KeyDownBehavior",
                typeof(KeyDownBehavior), typeof(KeyDown), null);
    }
}
