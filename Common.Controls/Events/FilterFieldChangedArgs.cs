using System;
using Common.Entities.Pagination;

namespace Common.Controls.Events
{
    public class FilterFieldChangedArgs : EventArgs
    {
        public IField FilterField { get; set; }

        public FilterFieldChangedArgs(IField field)
            :base()
        {
            this.FilterField = field;
        }
    }
}
