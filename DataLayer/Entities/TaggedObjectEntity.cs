using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class TaggedObjectEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ObjectType { get; set; }
    }
}
