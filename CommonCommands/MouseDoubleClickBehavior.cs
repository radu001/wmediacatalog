using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Common.Commands
{
    public class MouseDoubleClickBehavior : CommandBehaviorBase<Control>
    {
        public MouseDoubleClickBehavior(Control element)
            : base(element)
        {
            element.MouseDoubleClick += new MouseButtonEventHandler(element_MouseDoubleClick);
        }

        void element_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MouseDoubleClickArgs args = new MouseDoubleClickArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
