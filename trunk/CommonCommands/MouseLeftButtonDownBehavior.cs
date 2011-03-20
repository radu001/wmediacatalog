using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Common.Commands
{
    public class MouseLeftButtonDownBehavior : CommandBehaviorBase<Control>
    {
        public MouseLeftButtonDownBehavior(Control element)
            : base(element)
        {
            element.MouseLeftButtonDown += new MouseButtonEventHandler(element_MouseLeftButtonDown);
        }

        void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseDownArgs args = new MouseDownArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
