
using System;
using System.Windows;
using Microsoft.Practices.Composite.Presentation.Commands;
namespace Common.Commands
{
    public class ActivatedBehavior : CommandBehaviorBase<Window>
    {
        public ActivatedBehavior(Window element)
            : base(element)
        {
            element.Activated += new EventHandler(element_Activated);
        }

        void element_Activated(object sender, EventArgs e)
        {
            ActivatedArgs args = new ActivatedArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
