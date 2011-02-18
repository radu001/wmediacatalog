
using System.Collections;
using System.Collections.Generic;
namespace Common.Commands
{
    public class MultiSelectionChangedArgs
    {
        public MultiSelectionChangedArgs(object sender, IList selectedItems)
        {
            this.selectedValues = selectedItems;
            this.Sender = sender;
        }

        public object Sender { get; set; }
        
        public List<T> GetSelectedValues<T>() where T : class
        {
            List<T> result = new List<T>();

            foreach (object obj in selectedValues)
            {
                T val = obj as T;
                if (val != null)
                {
                    result.Add(val);
                }
            }

            return result;
        }

        private IList selectedValues;
    }
}
