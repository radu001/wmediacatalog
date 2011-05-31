using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules.Import.Model.Tree
{
    public class TreeItem
    {
        public object Data { get; private set; }

        public TreeItem(object data)
        {
            Data = data;
        }
    }
}
