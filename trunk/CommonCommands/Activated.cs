
using System.Windows;
using System.Windows.Input;
namespace Common.Commands
{
    public class Activated
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(Activated),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Window element = dependencyObject as Window;
            if (element != null)
            {
                ActivatedBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(Activated), new PropertyMetadata(CommandParameterProperty_Changed));

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
            Window element = dependencyObject as Window;

            if (element != null)
                GetOrCreateBehavior(element).CommandParameter = e.NewValue;
        }

        #endregion

        private static ActivatedBehavior GetOrCreateBehavior(Window element)
        {
            ActivatedBehavior behavior = element.GetValue(ActivatedBehaviorProperty) as ActivatedBehavior;
            if (behavior == null)
            {
                behavior = new ActivatedBehavior(element);
                element.SetValue(ActivatedBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static ActivatedBehavior GetActivatedBehavior(DependencyObject obj)
        {
            return (ActivatedBehavior)obj.GetValue(ActivatedBehaviorProperty);
        }

        public static void SetActivatedBehavior(DependencyObject obj,
            ActivatedBehavior value)
        {
            obj.SetValue(ActivatedBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivatedBehaviorProperty =
            DependencyProperty.RegisterAttached("ActivatedBehavior",
                typeof(ActivatedBehavior), typeof(Activated), null);
    }
}
