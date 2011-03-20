
using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Prism.Commands;
namespace Common.Commands
{
    public class MultiSelectionChangedBehavior : CommandBehaviorBase<Selector>
    {
        public MultiSelectionChangedBehavior(Selector selector)
            : base(selector)
        {
            selector.SelectionChanged += selector_SelectionChanged;
        }

        private void selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = sender as Selector;

            if (selector == null)
                return;

            IList selectedItems = null;

            if (selector is ListView)
                selectedItems = (selector as ListView).SelectedItems;
            else
                if (selector is DataGrid)
                    selectedItems = (selector as DataGrid).SelectedItems;
                else
                    if (selector is ListBox)
                        selectedItems = (selector as ListBox).SelectedItems;
                    else
                        throw new NotImplementedException("Command is provided currently for ListView, ListBox and DataGrid only");


            MultiSelectionChangedArgs args = new MultiSelectionChangedArgs(sender, selectedItems);



            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
