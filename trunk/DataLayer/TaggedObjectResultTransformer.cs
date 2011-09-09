using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using System.Collections;
using DataLayer.Entities;

namespace DataLayer
{
    public class TaggedObjectResultTransformer : IResultTransformer
    {
        public IList TransformList(IList collection)
        {
            return collection;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            int entityID = Convert.ToInt32(tuple[0]);
            string entityName = Convert.ToString(tuple[1]);
            int entityType = Convert.ToInt32(tuple[2]);

            return new TaggedObjectEntity()
            {
                ID = entityID,
                Name = entityName,
                ObjectType = entityType
            };
        }
    }
}
