
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Composite.Presentation.Commands;
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
                    args.MenuOwner = ((Popup)parentMenu.Parent).PlacementTarget;
                }
            }

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
