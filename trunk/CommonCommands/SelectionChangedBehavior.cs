
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
namespace Common.Commands
{
    public class SelectionChangedBehavior : CommandBehaviorBase<Selector>
    {
        public SelectionChangedBehavior(Selector selector)
            : base(selector)
        {
            selector.SelectionChanged += selector_SelectionChanged;
        }

        private void selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChangedArgs args = new SelectionChangedArgs()
            {
                Sender = sender,
                SelectedValue = e.AddedItems[0]
            };
            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
