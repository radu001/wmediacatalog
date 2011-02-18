
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class PreviewKeyDown
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(PreviewKeyDown),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control element = dependencyObject as Control;
            if (element != null)
            {
                PreviewKeyDownBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(PreviewKeyDown), new PropertyMetadata(CommandParameterProperty_Changed));

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

        private static PreviewKeyDownBehavior GetOrCreateBehavior(Control element)
        {
            PreviewKeyDownBehavior behavior = element.GetValue(PreviewKeyDownBehaviorProperty) as PreviewKeyDownBehavior;
            if (behavior == null)
            {
                behavior = new PreviewKeyDownBehavior(element);
                element.SetValue(PreviewKeyDownBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static PreviewKeyDownBehavior GetPreviewKeyDownBehavior(DependencyObject obj)
        {
            return (PreviewKeyDownBehavior)obj.GetValue(PreviewKeyDownBehaviorProperty);
        }

        public static void SetPreviewKeyDownBehavior(DependencyObject obj,
            PreviewKeyDownBehavior value)
        {
            obj.SetValue(PreviewKeyDownBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviewKeyDownBehaviorProperty =
            DependencyProperty.RegisterAttached("PreviewKeyDownBehavior",
                typeof(PreviewKeyDownBehavior), typeof(PreviewKeyDown), null);
    }
}
