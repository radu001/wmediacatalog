
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
namespace Common.Commands
{
    public class MultiSelectionChanged
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MultiSelectionChanged),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Selector element = dependencyObject as Selector;
            if (element != null)
            {
                MultiSelectionChangedBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(MultiSelectionChanged), new PropertyMetadata(CommandParameterProperty_Changed));

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

        private static MultiSelectionChangedBehavior GetOrCreateBehavior(Selector element)
        {
            MultiSelectionChangedBehavior behavior = element.GetValue(MultiSelectionChangedBehaviourProperty) as MultiSelectionChangedBehavior;
            if (behavior == null)
            {
                behavior = new MultiSelectionChangedBehavior(element);
                element.SetValue(MultiSelectionChangedBehaviourProperty, behavior);
            }

            return behavior;
        }

        public static MultiSelectionChangedBehavior GetMultiSelectionChangedCommandBehavior(DependencyObject obj)
        {
            return (MultiSelectionChangedBehavior)obj.GetValue(MultiSelectionChangedBehaviourProperty);
        }

        public static void SetMultiSelectionChangedCommandBehavior(DependencyObject obj,
            MultiSelectionChangedBehavior value)
        {
            obj.SetValue(MultiSelectionChangedBehaviourProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MultiSelectionChangedBehaviourProperty =
            DependencyProperty.RegisterAttached("MultiSelectionChangedBehaviour",
                typeof(MultiSelectionChangedBehavior), typeof(MultiSelectionChanged), null);
    }
}
