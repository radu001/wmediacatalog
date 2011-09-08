using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Entities
{
    public class TaggedBindingEntity
    {
        public virtual int TagID { get; set; }
        public virtual int EntityID { get; set; }
        public virtual int EntityType { get; set; }
        public virtual string TagName { get; set; }
        public virtual string EntityName { get; set; }

        public override bool Equals(object obj)
        {
            var te = obj as TaggedBindingEntity;
            if (te == null)
                return false;

            bool result = TagID == te.TagID;
            result &= EntityID == te.EntityID;
            result &= EntityType == te.EntityType;
            result &= TagName == te.TagName;
            result &= EntityName == te.EntityName;

            return result;
        }

        public override int GetHashCode()
        {
            if (fHashCode == 0)
            {
                fHashCode = 19 * TagID + 7;
                fHashCode ^= 11 * EntityID + 11;
                fHashCode ^= 17 * EntityType + 21;

                if (!String.IsNullOrEmpty(TagName))
                    fHashCode ^= TagName.GetHashCode();

                if (!String.IsNullOrEmpty(EntityName))
                    fHashCode ^= EntityName.GetHashCode();
            }

            return fHashCode;
        }

        private int fHashCode;
    }
}
