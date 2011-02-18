
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class MenuItemClick
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(MenuItemClick),
                new PropertyMetadata(CommandProperty_Changed));

        private static void CommandProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            MenuItem element = dependencyObject as MenuItem;
            if (element != null)
            {
                MenuItemClickBehavior behavior = GetOrCreateBehavior(element);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty =
             DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
                typeof(MenuItemClick), new PropertyMetadata(CommandParameterProperty_Changed));

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
            MenuItem element = dependencyObject as MenuItem;

            if (element != null)
                GetOrCreateBehavior(element).CommandParameter = e.NewValue;
        }

        #endregion

        private static MenuItemClickBehavior GetOrCreateBehavior(MenuItem element)
        {
            MenuItemClickBehavior behavior = element.GetValue(MenuItemClickBehaviorProperty) as MenuItemClickBehavior;
            if (behavior == null)
            {
                behavior = new MenuItemClickBehavior(element);
                element.SetValue(MenuItemClickBehaviorProperty, behavior);
            }

            return behavior;
        }

        public static MenuItemClickBehavior GetMenuItemClickBehavior(DependencyObject obj)
        {
            return (MenuItemClickBehavior)obj.GetValue(MenuItemClickBehaviorProperty);
        }

        public static void SetMenuItemClickBehavior(DependencyObject obj,
            MenuItemClickBehavior value)
        {
            obj.SetValue(MenuItemClickBehaviorProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverCommandBehavior.
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuItemClickBehaviorProperty =
            DependencyProperty.RegisterAttached("MenuItemClickBehavior",
                typeof(MenuItemClickBehavior), typeof(MenuItemClick), null);
    }
}
