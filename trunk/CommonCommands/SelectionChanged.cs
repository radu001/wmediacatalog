
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
namespace Common.Commands
{
    public class SelectionChanged
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(SelectionChanged),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Selector element = dependencyObject as Selector;
            if (element != null)
            {
                SelectionChangedBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(SelectionChanged), new PropertyMetadata(CommandParameterProperty_Changed));

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
            Selector element = dependencyObject as Selector;

            if (element != null)
                GetOrCreateBehavior(element).CommandParameter = e.NewValue;
        }

        #endregion

        private static SelectionChangedBehavior GetOrCreateBehavior(Selector element)
        {
            SelectionChangedBehavior behavior = element.GetValue(SelectionChangedBehaviourProperty) as SelectionChangedBehavior;
            if (behavior == null)
            {
                behavior = new SelectionChangedBehavior(element);
                element.SetValue(SelectionChangedBehaviourProperty, behavior);
            }

            return behavior;
        }

        public static SelectionChangedBehavior GetSelectionChangedBehavior(DependencyObject obj)
        {
            return (SelectionChangedBehavior)obj.GetValue(SelectionChangedBehaviourProperty);
        }

        public static void SetSelectionChangedBehavior(DependencyObject obj,
            SelectionChangedBehavior value)
        {
            obj.SetValue(SelectionChangedBehaviourProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionChangedBehaviourProperty =
            DependencyProperty.RegisterAttached("SelectionChangedBehaviour",
                typeof(SelectionChangedBehavior), typeof(SelectionChanged), null);
    }
}
