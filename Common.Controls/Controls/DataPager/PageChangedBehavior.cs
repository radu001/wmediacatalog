
using Microsoft.Practices.Prism.Commands;

namespace Common.Controls.Controls
{
    public class PageChangedBehavior : CommandBehaviorBase<Controls.DataPager>
    {
        public PageChangedBehavior(Controls.DataPager element)
            : base(element)
        {
            element.PageChangedEvent += new Controls.DataPager.PageChangedEventHandler(element_PageChangedEvent);
        }

        void element_PageChangedEvent(object sender, Controls.PageChangedEventArgs e)
        {
            PageChangedArgs args = new PageChangedArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
