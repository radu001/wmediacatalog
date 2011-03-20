using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Common.Commands
{
    public class MouseMoveBehavior : CommandBehaviorBase<Control>
    {
        public MouseMoveBehavior(Control element)
            : base(element)
        {
            element.MouseMove += new MouseEventHandler(element_MouseMove);
        }

        void element_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMoveArgs args = new MouseMoveArgs()
                {
                    Sender = sender,
                    Settings = e
                };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
