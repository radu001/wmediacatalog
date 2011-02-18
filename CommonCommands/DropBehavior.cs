
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;
namespace Common.Commands
{
    public class DropBehavior : CommandBehaviorBase<Control>
    {
        public DropBehavior(Control element)
            : base(element)
        {
            element.Drop += new DragEventHandler(element_Drop);
        }

        void element_Drop(object sender, DragEventArgs e)
        {
            DragArgs args = new DragArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
