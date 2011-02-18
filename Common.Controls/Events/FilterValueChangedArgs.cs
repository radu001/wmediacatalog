using System;

namespace Common.Controls.Events
{
    public class FilterValueChangedArgs : EventArgs
    {
        public string FilterValue { get; set; }

        public FilterValueChangedArgs(string filterValue)
            : base()
        {
            this.FilterValue = filterValue;
        }
    }
}
