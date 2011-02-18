
using System.Windows;
using System.Windows.Input;

namespace Common.Controls.Controls
{
    public class PageChanged
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(PageChanged),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            DataPager element = dependencyObject as DataPager;
            if (element != null)
            {
                PageChangedBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(PageChanged), new PropertyMetadata(CommandParameterProperty_Changed));

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
            DataPager element = dependencyObject as DataPager;

            if (element != null)
                GetOrCreateBehavior(element).CommandParameter = e.NewValue;
        }

        #endregion

        private static PageChangedBehavior GetOrCreateBehavior(DataPager element)
        {
            PageChangedBehavior behavior = element.GetValue(PageChangedBehaviorProperty) as PageChangedBehavior;
            if (behavior == null)
            {
                behavior = new PageChangedBehavior(element);
                element.SetValue(PageChangedBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static PageChangedBehavior GetTextChangedCommandBehavior(DependencyObject obj)
        {
            return (PageChangedBehavior)obj.GetValue(PageChangedBehaviorProperty);
        }

        public static void SetTextChangedCommandBehavior(DependencyObject obj,
            PageChangedBehavior value)
        {
            obj.SetValue(PageChangedBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageChangedBehaviorProperty =
            DependencyProperty.RegisterAttached("PageChangedBehavior",
                typeof(PageChangedBehavior), typeof(PageChanged), null);
    }
}
