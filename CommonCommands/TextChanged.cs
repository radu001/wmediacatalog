
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class TextChanged
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(TextChanged),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TextBox element = dependencyObject as TextBox;
            if (element != null)
            {
                TextChangedBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(TextChanged), new PropertyMetadata(CommandParameterProperty_Changed));

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
            TextBox element = dependencyObject as TextBox;

            if (element != null)
                GetOrCreateBehavior(element).CommandParameter = e.NewValue;
        }

        #endregion

        private static TextChangedBehavior GetOrCreateBehavior(TextBox element)
        {
            TextChangedBehavior behavior = element.GetValue(TextChangedBehaviourProperty) as TextChangedBehavior;
            if (behavior == null)
            {
                behavior = new TextChangedBehavior(element);
                element.SetValue(TextChangedBehaviourProperty, behavior);
            }

            return behavior;
        }

        public static TextChangedBehavior GetTextChangedCommandBehavior(DependencyObject obj)
        {
            return (TextChangedBehavior)obj.GetValue(TextChangedBehaviourProperty);
        }

        public static void SetTextChangedCommandBehavior(DependencyObject obj,
            TextChangedBehavior value)
        {
            obj.SetValue(TextChangedBehaviourProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextChangedBehaviourProperty =
            DependencyProperty.RegisterAttached("TextChangedBehaviour",
                typeof(TextChangedBehavior), typeof(TextChanged), null);
    }
}
