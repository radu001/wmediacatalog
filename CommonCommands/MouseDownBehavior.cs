using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Commands
{
    public class MouseDownBehavior : CommandBehaviorBase<Control>
    {
        public MouseDownBehavior(Control element)
            : base(element)
        {
            element.MouseDown += new MouseButtonEventHandler(element_MouseDown);
        }

        void element_MouseDown(object sender, MouseButtonEventArgs e)
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
