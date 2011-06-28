
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
namespace Common.Commands
{
    public class MenuItemClickBehavior : CommandBehaviorBase<MenuItem>
    {
        public MenuItemClickBehavior(MenuItem element)
            : base(element)
        {
            element.Click += new RoutedEventHandler(element_Click);
        }

        void element_Click(object sender, RoutedEventArgs e)
        {
            MenuItemClickArgs args = new MenuItemClickArgs()
            {
                Sender = sender,
                Settings = e
            };

            if (sender is MenuItem)
            {
                MenuItem menuItem = sender as MenuItem;

                ContextMenu parentMenu = menuItem.Parent as ContextMenu;
                if (parentMenu != null)
                {
                    var menuOwner = ((Popup)parentMenu.Parent).PlacementTarget as FrameworkElement;
                    if (menuOwner != null)
                    {
                        args.MenuOwner = menuOwner;
                        args.DataItem = menuOwner.DataContext;
                    }
                }
            }

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
