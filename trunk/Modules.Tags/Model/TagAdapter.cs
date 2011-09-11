using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagCloudLib;
using BusinessObjects;

namespace Modules.Tags.Model
{
    public class TagAdapter : ITag
    {
        #region ITag Members

        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Rank { get; private set; }
        public string Color { get; private set; }
        public string TextColor { get; private set; }

        #endregion

        public TagAdapter(Tag tag)
        {
            ID = tag.ID;
            Name = tag.Name;
            Rank = tag.AssociatedEntitiesCount;
            Color = tag.Color;
            TextColor = tag.TextColor;
        }

        public override bool Equals(object obj)
        {
            TagAdapter ta = obj as TagAdapter;
            if (ta == null)
                return false;

            bool result = ID == ta.ID;
            result &= Name == ta.Name;
            result &= Rank == ta.Rank;
            result &= Color == ta.Color;
            result &= TextColor == ta.TextColor;

            return result;
        }

        public override int GetHashCode()
        {
            if (fHashCode == 0)
            {
                fHashCode = 7 + 11 * ID;
                fHashCode ^= 11 + 13 * Rank;

                if (Name != null)
                    fHashCode ^= 23 * Name.GetHashCode();

                if (Color != null)
                    fHashCode ^= 19 * Color.GetHashCode();

                if (TextColor != null)
                    fHashCode ^= 11 * TextColor.GetHashCode();
            }

            return fHashCode;
        }

        private int fHashCode;
    }
}
