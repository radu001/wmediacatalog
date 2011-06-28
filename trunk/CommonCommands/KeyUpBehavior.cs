
using System.Windows.Controls;
using System.Windows.Input;
namespace Common.Commands
{
    public class KeyUpBehavior : CommandBehaviorBase<Control>
    {
        public KeyUpBehavior(Control element)
            : base(element)
        {
            element.KeyUp += new KeyEventHandler(element_KeyUp);
        }

        void element_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpArgs args = new KeyUpArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
