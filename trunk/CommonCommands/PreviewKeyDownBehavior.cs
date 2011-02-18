
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
namespace Common.Commands
{
    public class PreviewKeyDownBehavior : CommandBehaviorBase<Control>
    {
        public PreviewKeyDownBehavior(Control element)
            : base(element)
        {
            element.PreviewKeyDown += new KeyEventHandler(element_PreviewKeyDown);
        }

        void element_PreviewKeyDown(object sender, KeyEventArgs e)
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
