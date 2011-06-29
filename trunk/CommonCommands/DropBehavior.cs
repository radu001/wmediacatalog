
using System.Windows;
namespace Common.Commands
{
    public class DropBehavior : CommandBehaviorBase<FrameworkElement>
    {
        public DropBehavior(FrameworkElement element)
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
