
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
namespace Common.Commands
{
    public class KeyDownBehavior : CommandBehaviorBase<Control>
    {
        public KeyDownBehavior(Control element)
            : base(element)
        {
            element.KeyDown += new KeyEventHandler(element_KeyDown);
        }

        void element_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownArgs args = new KeyDownArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
