using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Commands
{
    public class Checked
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(Checked),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            CheckBox element = dependencyObject as CheckBox;
            if (element != null)
            {
                var behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(Checked), new PropertyMetadata(CommandParameterProperty_Changed));

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
            CheckBox element = dependencyObject as CheckBox;

            if (element != null)
                GetOrCreateBehavior(element).CommandParameter = e.NewValue;
        }

        #endregion

        private static CheckedBehavior GetOrCreateBehavior(CheckBox element)
        {
            var behavior = element.GetValue(CheckedBehaviorProperty) as CheckedBehavior;
            if (behavior == null)
            {
                behavior = new CheckedBehavior(element);
                element.SetValue(CheckedBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static CheckedBehavior GetCheckedBehavior(DependencyObject obj)
        {
            return (CheckedBehavior)obj.GetValue(CheckedBehaviorProperty);
        }

        public static void SetCheckedBehavior(DependencyObject obj,
            CheckedBehavior value)
        {
            obj.SetValue(CheckedBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckedBehaviorProperty =
            DependencyProperty.RegisterAttached("CheckedBehavior",
                typeof(CheckedBehavior), typeof(Drop), null);
    }
}
